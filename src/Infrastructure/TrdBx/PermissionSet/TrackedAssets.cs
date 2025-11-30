
using System.ComponentModel;
namespace CleanArchitecture.Blazor.Infrastructure.PermissionSet;

public static partial class Permissions
{
    [DisplayName("TrackedAsset Permissions")]
    [Description("Set permissions for TrackedAsset operations.")]
    public static class TrackedAssets
    {
        [Description("Allows viewing TrackedAsset details.")]
        public const string View = "Permissions.TrackedAssets.View";

        [Description("Allows creating new TrackedAsset records.")]
        public const string Create = "Permissions.TrackedAssets.Create";

        [Description("Allows modifying existing TrackedAsset details.")]
        public const string Edit = "Permissions.TrackedAssets.Edit";

        [Description("Allows deleting TrackedAsset records.")]
        public const string Delete = "Permissions.TrackedAssets.Delete";

        [Description("Allows searching for TrackedAsset records.")]
        public const string Search = "Permissions.TrackedAssets.Search";

        [Description("Allows exporting TrackedAsset records.")]
        public const string Export = "Permissions.TrackedAssets.Export";

        [Description("Allows importing TrackedAsset records.")]
        public const string Import = "Permissions.TrackedAssets.Import";
    }
}
public class TrackedAssetsAccessRights
{
    public bool View { get; set; }
    public bool Create { get; set; }
    public bool Edit { get; set; }
    public bool Delete { get; set; }
    public bool Search { get; set; }
    public bool Export { get; set; }
    public bool Import { get; set; }
}

