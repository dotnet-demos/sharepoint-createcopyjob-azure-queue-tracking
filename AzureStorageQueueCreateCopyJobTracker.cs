using System.Net.Http.Json;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Azure;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PnP.Core.Model.SharePoint;
using PnP.Core.Services;
class AzureStorageQueueCreateCopyJobTracker
{
    IMigrationJobStatusProvider migrationJobStatusProvider;
    ILogger<AzureStorageQueueCreateCopyJobTracker> logger;

    public AzureStorageQueueCreateCopyJobTracker(IMigrationJobStatusProvider provider, ILogger<AzureStorageQueueCreateCopyJobTracker> logger)
    {
        this.logger = logger;
        this.migrationJobStatusProvider = provider;
    }
    public bool IsJobCompleted(PnPContext context, ICopyMigrationInfo info)
    {
        //ICopyJobProgress result = context.Site.GetCopyJobProgressAsync(info); /This gives detailed status.But here we are checking from Azure queue where the job reports.

        MigrationJobState state = migrationJobStatusProvider.Get(context, info);
        if (state == MigrationJobState.None) //Job level says None is completed (success/failure)
        {
            IEnumerable<CopyJobLog> logs = GetCopyJobLogsFromAzureStorageQueue(info);
            //If there is any entry as follows it translate to successful completion.
            var completionLogs = logs
                .Where((log) => log.Event == CopyJobLogEvent.JobEnd && log.MigrationDirection == CopyJobLogMigrationDirection.Import);

            if (completionLogs.Count() == 1)
            {
                completionLogs.ToList().ForEach(log =>
                {
                    if (log.TotalWarnings > 0)
                    {
                        logger.LogWarning($"{info.JobId} Completed. Warnings:{log.TotalWarnings},FilesCreated:{log.FilesCreated}, TotalExpectedSPOObjects:{log.TotalExpectedSPObjects}, objectsProcessed:{log.ObjectsProcessed}, TotalExpectedBytes:{log.TotalExpectedBytes}, BytesProcessed:{log.BytesProcessed}, TotalDurationInMs:{log.TotalDurationInMs}");
                    }
                    else if (log.TotalErrors > 0)
                    {
                        logger.LogError($"{info.JobId} completed with Errors: {log.TotalErrors}. FilesCreated:{log.FilesCreated}, TotalExpectedSPOObjects:{log.TotalExpectedSPObjects}, objectsProcessed:{log.ObjectsProcessed}, TotalExpectedBytes:{log.TotalExpectedBytes}, BytesProcessed:{log.BytesProcessed}, TotalDurationInMs:{log.TotalDurationInMs}");
                    }
                    else
                    {
                        logger.LogInformation($"{info.JobId} completed without warnings/errors. FilesCreated:{log.FilesCreated}, TotalExpectedSPOObjects:{log.TotalExpectedSPObjects}, objectsProcessed:{log.ObjectsProcessed}, TotalExpectedBytes:{log.TotalExpectedBytes}, BytesProcessed:{log.BytesProcessed}, TotalDurationInMs:{log.TotalDurationInMs}");
                    }
                });
                return true;
            }
            else
            {
                logger.LogWarning($"There are multiple JobEnds with MigrationDirection.Import");
                throw new InvalidOperationException("There are multiple JobEnds with MigrationDirection.Import");
            }
        }
        return false;
    }



    private IEnumerable<CopyJobLog> GetCopyJobLogsFromAzureStorageQueue(ICopyMigrationInfo info)
    {
        Response<QueueMessage[]> messages;
        List<CopyJobLog> result = new();
        QueueClient client = new(new Uri(info.JobQueueUri.ToString()));
        do
        {
            messages = client.ReceiveMessages(maxMessages: 25);
            IEnumerable<CopyJobLog> logs = DecryptAndDeserializeMessages(messages, info.EncryptionKey);
            result.AddRange(logs);
        } while (messages.Value.Length != 0);
        return result;
    }
    private IEnumerable<CopyJobLog> DecryptAndDeserializeMessages(Response<QueueMessage[]> messages, byte[] encryptionKey)
    {
        foreach (QueueMessage msg in messages.Value)
        {
            var base64DecodedArray = Convert.FromBase64String(msg.Body.ToString());
            var jsonBody = Encoding.UTF8.GetString(base64DecodedArray);
            AzureJobProgress progress = System.Text.Json.JsonSerializer.Deserialize<AzureJobProgress>(jsonBody);
            string progressString = progress.Decrypt(encryptionKey);
            yield return JsonConvert.DeserializeObject<CopyJobLog>(progressString);
        }
    }
}