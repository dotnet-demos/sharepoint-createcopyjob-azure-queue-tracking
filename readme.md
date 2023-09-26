# Purpose
- Demo how can the SharePoint CopyJobProgress can be done via Azure Storage Queue
  
# How to run
- Clone the project
- Replace values in appsettings.json
  - SourceSiteRootUrl - This needs to be the absolute URL
  - SourceLibraryName - name of drive/document library/list whatever you used to call it.
  - DestinationLibraryAbsoluteUrl - This is the library in the destination site.
  - Azure AD - Currently the sample uses ROPC flow to authenticate to SharePoint. Feel free to change the code in case you need to use the Azure app registration based Workload identity mechanism.
    
# Environment
- .Net 6
- Visual Studio Code
- SharePoint Online
