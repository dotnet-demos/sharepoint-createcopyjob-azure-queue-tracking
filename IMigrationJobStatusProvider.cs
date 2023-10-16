using PnP.Core.Model.SharePoint;
using PnP.Core.Services;

interface IMigrationJobStatusProvider
{
MigrationJobState Get(IPnPContext context, ICopyMigrationInfo info);
}
