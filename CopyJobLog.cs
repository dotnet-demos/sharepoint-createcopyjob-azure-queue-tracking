///<Remarks>
/// This object is designed from the docs at https://learn.microsoft.com/en-us/sharepoint/dev/apis/migration-api-overview#azurequeuereporturi
/// </Remarks>
class CopyJobLog
    {
        public DateTime Time { get; set; } // Set for events: All
        public Guid JobId { get; set; } // Set for events: All
        public Guid CorrelationId { get; set; }// Set for events: All
        public CopyJobLogEvent Event { get; set; }
        public int TotalRetryCount { get; set; }//Set for Events:JobStart,JobPostponed,JobProgress,JobWarning,JobFatalError,JobError,JobCancelled,JobEnd
        
        public Guid SiteId { get; set; } //Set for Events: JobQueued, JobStart,JobPostponed
        public Guid WebId { get; set; } //Set for Events: JobStart
        public Guid DbId { get; set; }//Set for Events: JobQueued,JobStart,JobPostponed
        public Guid FarmId { get; set; }//Set for Events: JobStart
        public Guid ServerId { get; set; }//Set for Events: JobStart
        public Guid SubscriptionId { get; set; }//Set for Events: JobStart

        public CopyJobLogMigrationType MigrationType { get; set; }//Set for Events:JobStart,JobPostponed,JobProgress,JobWarning,JobFatalError,JobCancelled,JobEnd
        public CopyJobLogMigrationDirection MigrationDirection { get; set; }//Set for Events:JobStart,JobPostponed,JobProgress,JobWarning,JobFatalError,JobError,JobCancelled,JobEnd
        public DateTime NextPickupTime { get; set; } // Set for events: JobPostponed
        public string Url { get; set; }//Set for Events:JobWarning,JobError
        public int FilesCreated { get; set; }//Set for Events:JobProgress,JobEnd
        public long BytesProcessed { get; set; }//Set for Events:JobProgress,JobEnd
        public long TotalExpectedBytes { get; set; }//Set for Events:JobProgress,JobEnd
        public int ObjectsProcessed { get; set; }//Set for Events:JobProgress,JobEnd
        public int TotalExpectedSPObjects { get; set; }//Set for Events:JobProgress,JobEnd
        public int TotalErrors { get; set; }//Set for Events:JobProgress,JobEnd
        public int TotalWarnings { get; set; }//Set for Events:JobProgress,JobEnd
        public double TotalDurationInMs { get; set; }//Set for Events:JobProgress,JobEnd
        public int CpuDurationInMs { get; set; }//Set for Events:JobProgress,JobEnd
        public int SqlDurationInMs { get; set; }//Set for Events:JobProgress,JobEnd
        public int SqlQueryCount { get; set; }//Set for Events:JobProgress,JobEnd
        public int WaitTimeOnSqlThrottlingMilliseconds { get; set; } //Set for Events:JobProgress,JobEnd
        public string FileName { get; set; }
        public bool IsShallowCopy { get; set; }//Set for Events:JobProgress,
        public string CancelledByUser { get; set; }//Set for Events: JobCancelled
        public string SourceObjectFullUrl { get; set; }
        public string TargetServerUrl { get; set; }
        public string TargetListId { get; set; }
        public string TargetObjectSiteRelativeUrl { get; set; }
        public string TargetObjectUniqueId { get; set; }
        public string TargetSiteName { get; set; }
        // For JobWarning
        public string ObjectType { get; set; }//Set for Events:JobWarning,JobError
        public Guid Id { get; set; }//Set for Events:JobWarning,JobError
        public int SourceListItemIntId { get; set; }
        public int TargetListItemIntId { get; set; }
        public int ErrorCode{get;set;}//Set for Events:JobWarning,JobFatalError,JobError
        public int ErrorType{get;set;}//Set for Events:JobWarning,JobFatalError,JobError
        public string Message { get; set; }//Set for Events:JobWarning,JobFatalError,JobError
        public string ManifestFileName { get; set; }//Set for Events:FinishManifestFileUpload
    }