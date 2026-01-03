using System.ComponentModel;
namespace CleanArchitecture.Blazor.Infrastructure.PermissionSet;

public static partial class Permissions
{
    [DisplayName("WialonUnit Permissions")]
    [Description("Set permissions for WialonUnit operations.")]
    public static class WialonUnits
    {
        [Description("Allows viewing WialonUnit details.")]
        public const string View = "Permissions.WialonUnits.View";

        [Description("Allows modifying existing WialonUnit details.")]
        public const string Edit = "Permissions.WialonUnits.Edit";

        [Description("Allows deleting WialonUnit records.")]
        public const string Delete = "Permissions.WialonUnits.Delete";

        [Description("Allows searching for WialonUnit records.")]
        public const string Search = "Permissions.WialonUnits.Search";

        [Description("Allows exporting WialonUnit records.")]
        public const string Export = "Permissions.WialonUnits.Export";

        [Description("Allows importing WialonUnit records.")]
        public const string Import = "Permissions.WialonUnits.Import";

        [Description("Allows Sync Wialon unit Names with local Tracking units names.")]
        public const string SyncData = "Permissions.WialonUnits.SyncData";
        
    }
}

public class WialonUnitsAccessRights
{
    public bool View { get; set; }
    public bool Edit { get; set; }
    public bool Delete { get; set; }
    public bool Search { get; set; }
    public bool Export { get; set; }
    public bool Import { get; set; }

    public bool SyncData { get; set; }
}



