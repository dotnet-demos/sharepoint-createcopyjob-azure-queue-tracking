using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PnP.Core.Model.SharePoint;
using PnP.Core.Services;

class SharePointCopyOperations
{
    private readonly ILogger<SharePointCopyOperations> logger;
    private readonly IConfiguration config;
    private readonly IPnPContextFactory factory;
    private readonly AzureStorageQueueCreateCopyJobTracker copyJobTracker;
    public SharePointCopyOperations(IPnPContextFactory factory, AzureStorageQueueCreateCopyJobTracker copyJobTracker, ILogger<SharePointCopyOperations> logger, IConfiguration configuration)
    {
        this.logger = logger;
        this.config = configuration;
        this.factory = factory;
        this.copyJobTracker= copyJobTracker;
    }
    internal async Task CopyAllDriveContentsBetweenSites()
    {
        string rootSiteUrl = config["SourceSiteRootUrl"];
        string rootSiteHostName = new Uri(rootSiteUrl).GetLeftPart(UriPartial.Authority);
        logger.LogInformation(rootSiteHostName);
        var context = await factory.CreateAsync(rootSiteUrl);
        var web = await context.Web.GetAsync();
        logger.LogInformation("Site name: {Name}", web.Title);
        var folders = await GetAllFoldersAtRoot(web, config["SourceLibraryName"]);

        var absoluteUrlOfSourceFolders = folders
        .Where(folder => !folder.ServerRelativeUrl.EndsWith("Forms"))
        .Select<IFolder, string>(folder => $"{rootSiteHostName}{folder.ServerRelativeUrl}")
        .ToArray();
        // absoluteUrlOfSourceFolders
        // .ToList()
        // .ForEach(folder=>logger.LogInformation($"{folder}"));
        var infos = await context.Site.CreateCopyJobsAsync(absoluteUrlOfSourceFolders,config["DestinationLibraryAbsoluteUrl"], GetCopyMigrationOptions());
        ICollection<Guid> trackingJobGuids = infos.Select(info => info.JobId).ToList();
                   do
                   {
                       logger.LogInformation("Going to sleep for 10 secs before checking status.");
                       Thread.Sleep(TimeSpan.FromSeconds(1));
                       //To Track the progress on each item, we need loop and pooling in there are more than 1 items in collection.
                       infos
                       .Where(info => trackingJobGuids.Contains(info.JobId))
                       .ToList()
                       .ForEach(info =>
                       {
                           //bool completed = IsJobCompleted(clientContext, info);
                           bool completed = copyJobTracker.IsJobCompleted(context, info);
                           if (completed)
                           {
                               trackingJobGuids.Remove(info.JobId);
                           }
                       });
                       logger.LogInformation($"Monitoring of jobs status iteration completed. pending {trackingJobGuids.Count}/{infos.Count}");
                   } while (trackingJobGuids.Count() != 0);
    }
    private static CopyMigrationOptions GetCopyMigrationOptions()
    {
        return new()
        {
            AllowSchemaMismatch = true,
            IsMoveMode = false,
            IgnoreVersionHistory = false,
            NameConflictBehavior = SPMigrationNameConflictBehavior.Replace
        };
    }
    internal async Task<IEnumerable<IFolder>> GetAllFoldersAtRoot(IWeb web, string libraryName)
    {
        IList list = await web.Lists.GetByTitleAsync(libraryName);
        return list.RootFolder.Folders.AsQueryable(); // This wont work when the items cross 5000. Use Caml query
    }
}