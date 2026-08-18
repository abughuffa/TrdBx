using System.ComponentModel;
namespace CleanArchitecture.Blazor.Application.Common.Security;

public static partial class Permissions
{
    [DisplayName("SimCard Permissions")]
    [Description("Set permissions for SimCard operations.")]
    public static class SimCards
    {
        [Description("Allows viewing SimCard details.")]
        public const string View = "Permissions.SimCards.View";

        [Description("Allows creating new SimCard records.")]
        public const string Create = "Permissions.SimCards.Create";

        [Description("Allows modifying existing SimCard details.")]
        public const string Edit = "Permissions.SimCards.Edit";

        [Description("Allows deleting SimCard records.")]
        public const string Delete = "Permissions.SimCards.Delete";



        [Description("Allows exporting SimCard records.")]
        public const string Export = "Permissions.SimCards.Export";

        [Description("Allows importing SimCard records.")]
        public const string Import = "Permissions.SimCards.Import";

        [Description("Allows Recharge SimCard records.")]
        public const string RechargeSim = "Permissions.SimCards.RechargeSim";

        [Description("Allows SyncExpiryDate of SimCard records.")]
        public const string SyncExpiryDate = "Permissions.SimCards.SyncExpiryDate";
    }
}
public class SimCardsAccessRights
{
    public bool View { get; set; }
    public bool Create { get; set; }
    public bool Edit { get; set; }
    public bool Delete { get; set; }
    public bool Search { get; set; }
    public bool Export { get; set; }
    public bool Import { get; set; }
}

