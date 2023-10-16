///<Remarks>
/// This object is designed from the docs at https://learn.microsoft.com/en-us/sharepoint/dev/apis/migration-api-overview#azurequeuereporturi
/// </Remarks>
enum CopyJobLogEvent
    {
        JobQueued,
        JobPostponed,
        JobStart,
        JobLogFileCreate,
        FinishManifestFileUpload,
        JobProgress,
        JobEnd,
        JobDeleted,
        JobCancelled,
        JobFinishedObjectInfo,
        JobError,
        JobFatalError,
        JobWarning,
    }
