using System.ComponentModel;
namespace CleanArchitecture.Blazor.Infrastructure.PermissionSet;

public static partial class Permissions
{
    [DisplayName("ServiceLog Permissions")]
    [Description("Set permissions for ServiceLog operations.")]
    public static class ServiceLogs
    {
        [Description("Allows viewing ServiceLog details.")]
        public const string View = "Permissions.ServiceLogs.View";

        [Description("Allows creating new ServiceLog records.")]
        public const string Create = "Permissions.ServiceLogs.Create";

        [Description("Allows modifying existing ServiceLog details.")]
        public const string Edit = "Permissions.ServiceLogs.Edit";

        [Description("Allows deleting ServiceLog records.")]
        public const string Delete = "Permissions.ServiceLogs.Delete";

        [Description("Allows searching for ServiceLog records.")]
        public const string Search = "Permissions.ServiceLogs.Search";

        [Description("Allows exporting ServiceLog records.")]
        public const string Export = "Permissions.ServiceLogs.Export";

        [Description("Allows importing ServiceLog records.")]
        public const string Import = "Permissions.ServiceLogs.Import";
    }
}
public class ServiceLogsAccessRights
{
    public bool View { get; set; }
    public bool Create { get; set; }
    public bool Edit { get; set; }
    public bool Delete { get; set; }
    public bool Search { get; set; }
    public bool Export { get; set; }
    public bool Import { get; set; }
}

