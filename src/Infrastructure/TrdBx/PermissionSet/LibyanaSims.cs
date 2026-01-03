using System.ComponentModel;

namespace CleanArchitecture.Blazor.Infrastructure.PermissionSet;

public static partial class Permissions
{
    [DisplayName("LibyanaSimCard Permissions")]
    [Description("Set permissions for LibyanaSimCard operations.")]
    public static class LibyanaSimCards
    {
        [Description("Allows viewing LibyanaSimCard details.")]
        public const string View = "Permissions.LibyanaSimCards.View";

        [Description("Allows deleting LibyanaSimCard records.")]
        public const string Delete = "Permissions.LibyanaSimCards.Delete";

        [Description("Allows searching for LibyanaSimCard records.")]
        public const string Search = "Permissions.LibyanaSimCards.Search";

        [Description("Allows exporting LibyanaSimCard records.")]
        public const string Export = "Permissions.LibyanaSimCards.Export";

        [Description("Allows importing LibyanaSimCard records.")]
        public const string Import = "Permissions.LibyanaSimCards.Import";

        [Description("Allows Sync Libyana Sim Card Expairy dates with local Sim Card Expairy dates.")]
        public const string SyncData = "Permissions.LibyanaSimCards.SyncData";
    }
}
public class LibyanaSimCardsAccessRights
{
    public bool View { get; set; }
    public bool Delete { get; set; }
    public bool Search { get; set; }
    public bool Export { get; set; }
    public bool Import { get; set; }
    public bool SyncData { get; set; }
}





