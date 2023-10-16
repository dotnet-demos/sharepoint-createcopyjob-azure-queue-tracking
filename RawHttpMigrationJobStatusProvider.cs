using System.Dynamic;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PnP.Core.Model.SharePoint;
using PnP.Core.Services;
class RawHttpMigrationJobStatusProvider : IMigrationJobStatusProvider
{
    private readonly ILogger<RawHttpMigrationJobStatusProvider> logger;
    public RawHttpMigrationJobStatusProvider(ILogger<RawHttpMigrationJobStatusProvider> logger)
    {
        this.logger=logger;
    }
    /// <summary>
    /// Get high level job status
    /// </summary>
    /// <param name="context"></param>
    /// <param name="info"></param>
    /// <returns></returns>
    /// <remarks>
    /// site..GetMigrationJobStatus(info.JobId);//This is not present in PnP.Core
    /// Details https://github.com/pnp/pnpcore/issues/1277
    /// API Reference https://learn.microsoft.com/en-us/previous-versions/office/sharepoint-csom/mt143033(v=office.15)
    /// </remarks>
    MigrationJobState IMigrationJobStatusProvider.Get(IPnPContext context, ICopyMigrationInfo info)
    {
        var requestBody = new
        {
            id = info.JobId
        };
        ApiRequest req = new ApiRequest(HttpMethod.Post, ApiRequestType.SPORest, $"_api/site/GetMigrationJobStatus", System.Text.Json.JsonSerializer.Serialize(requestBody));
        var res = context.Site.ExecuteRequest(req);
        dynamic jsonObj = JsonConvert.DeserializeObject<ExpandoObject>(res.Response, new ExpandoObjectConverter());

        MigrationJobState state = (MigrationJobState)(jsonObj.d.GetMigrationJobStatus);
        logger.LogInformation($"Status of Job {info.JobId} is {state}");
        return state;
    }
}
