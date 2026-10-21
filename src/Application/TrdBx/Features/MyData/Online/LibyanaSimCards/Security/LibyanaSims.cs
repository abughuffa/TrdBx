using System.ComponentModel;

namespace CleanArchitecture.Blazor.Application.Common.Security;
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

        [Description("Allows exporting LibyanaSimCard records.")]
        public const string Export = "Permissions.LibyanaSimCards.Export";

        [Description("Allows importing LibyanaSimCard records.")]
        public const string Import = "Permissions.LibyanaSimCards.Import";

    }
}
public class LibyanaSimCardsAccessRights
{
    public bool View { get; set; }
    public bool Delete { get; set; }
    public bool Export { get; set; }
    public bool Import { get; set; }
    // public bool SyncData { get; set; }
}





