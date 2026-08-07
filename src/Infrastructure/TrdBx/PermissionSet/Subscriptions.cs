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

