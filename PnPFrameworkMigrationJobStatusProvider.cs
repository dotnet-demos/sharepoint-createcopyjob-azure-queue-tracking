using Microsoft.Extensions.Logging;
using Microsoft.SharePoint.Client;
using PnP.Core.Model.SharePoint;
using PnP.Core.Services;
using PnP.Framework;
class PnPFrameworkMigrationJobStatusProvider : IMigrationJobStatusProvider
{
    private readonly ILogger<PnPFrameworkMigrationJobStatusProvider> logger;
    public PnPFrameworkMigrationJobStatusProvider(ILogger<PnPFrameworkMigrationJobStatusProvider> logger)
    {
        this.logger=logger;
    }
    PnP.Core.Model.SharePoint.MigrationJobState IMigrationJobStatusProvider.Get(IPnPContext pnpCoreContext, ICopyMigrationInfo info)
    {
        using(ClientContext clientContext = PnPCoreSdk.Instance.GetClientContext(pnpCoreContext as PnPContext))
        {
            var result = clientContext.Site.GetMigrationJobStatus(info.JobId);
            clientContext.ExecuteQuery();
            Microsoft.SharePoint.Client.MigrationJobState status = result.Value;
            PnP.Core.Model.SharePoint.MigrationJobState pnpStatus = (PnP.Core.Model.SharePoint.MigrationJobState) status;
            logger.LogInformation($"Status of Job {info.JobId} is {pnpStatus}");
            return pnpStatus;
        }
    }
}