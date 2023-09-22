class CopyJobLog
    {
        public CopyJobLogEvent Event { get; set; }
        public int TotalRetryCount { get; set; }//Set for Events:JobProgress,
        public DateTime Time { get; set; }
        public Guid JobId { get; set; }
        public Guid SiteId { get; set; }
        public Guid DbId { get; set; }
        public CopyJobLogMigrationType MigrationType { get; set; }
        public CopyJobLogMigrationDirection MigrationDirection { get; set; }
        public string Url { get; set; }
        public int FilesCreated { get; set; }//Set for Events:JobProgress,
        public long BytesProcessed { get; set; }//Set for Events:JobProgress,
        public long TotalExpectedBytes { get; set; }
        public int ObjectsProcessed { get; set; }//Set for Events:JobProgress,
        public int TotalExpectedSPObjects { get; set; }//Set for Events:JobProgress,
        public int TotalErrors { get; set; }//Set for Events:JobProgress,
        public int TotalWarnings { get; set; }//Set for Events:JobProgress,
        public double TotalDurationInMs { get; set; }//Set for Events:JobProgress,
        public int CpuDurationInMs { get; set; }//Set for Events:JobProgress,
        public int SqlDurationInMs { get; set; }//Set for Events:JobProgress,
        public int SqlQueryCount { get; set; }//Set for Events:JobProgress,
        public int WaitTimeOnSqlThrottlingMilliseconds { get; set; }
        public string FileName { get; set; }
        public bool IsShallowCopy { get; set; }//Set for Events:JobProgress,
        public Guid CorrelationId { get; set; }
        public string SourceObjectFullUrl { get; set; }
        public string TargetServerUrl { get; set; }
        public string TargetListId { get; set; }
        public string TargetObjectSiteRelativeUrl { get; set; }
        public string TargetObjectUniqueId { get; set; }
        public string TargetSiteName { get; set; }
        // For JobStart
        public Guid FarmId { get; set; }
        public Guid SubscriptionId { get; set; }
        // For JobWarning
        public string ObjectType { get; set; }
        public Guid Id { get; set; }
        public int SourceListItemIntId { get; set; }
        public int TargetListItemIntId { get; set; }
        public string Message { get; set; }
        public string ManifestFileName { get; set; }//Set for Events:FinishManifestFileUpload
    }