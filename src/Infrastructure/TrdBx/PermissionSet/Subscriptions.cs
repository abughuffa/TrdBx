using System.ComponentModel;
namespace CleanArchitecture.Blazor.Infrastructure.PermissionSet;

public static partial class Permissions
{
    [DisplayName("Subscription Permissions")]
    [Description("Set permissions for Subscription operations.")]
    public static class Subscriptions
    {
        [Description("Allows viewing Subscription details.")]
        public const string View = "Permissions.Subscriptions.View";

        [Description("Allows creating new Subscription records.")]
        public const string Create = "Permissions.Subscriptions.Create";

        [Description("Allows modifying existing Subscription details.")]
        public const string Edit = "Permissions.Subscriptions.Edit";

        [Description("Allows deleting Subscription records.")]
        public const string Delete = "Permissions.Subscriptions.Delete";

        [Description("Allows searching for Subscription records.")]
        public const string Search = "Permissions.Subscriptions.Search";

        [Description("Allows exporting Subscription records.")]
        public const string Export = "Permissions.Subscriptions.Export";

        [Description("Allows importing Subscription records.")]
        public const string Import = "Permissions.Subscriptions.Import";
    }
}
public class SubscriptionsAccessRights
{
    public bool View { get; set; }
    public bool Create { get; set; }
    public bool Edit { get; set; }
    public bool Delete { get; set; }
    public bool Search { get; set; }
    public bool Export { get; set; }
    public bool Import { get; set; }
}

