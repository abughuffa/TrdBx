using System.ComponentModel;
namespace CleanArchitecture.Blazor.Infrastructure.PermissionSet;

public static partial class Permissions
{
    [DisplayName("ServicePrice Permissions")]
    [Description("Set permissions for ServicePrice operations.")]
    public static class ServicePrices
    {
        [Description("Allows viewing ServicePrice details.")]
        public const string View = "Permissions.ServicePrices.View";

        [Description("Allows creating new ServicePrice records.")]
        public const string Create = "Permissions.ServicePrices.Create";

        [Description("Allows modifying existing ServicePrice details.")]
        public const string Edit = "Permissions.ServicePrices.Edit";

        [Description("Allows deleting ServicePrice records.")]
        public const string Delete = "Permissions.ServicePrices.Delete";

        [Description("Allows searching for ServicePrice records.")]
        public const string Search = "Permissions.ServicePrices.Search";

        [Description("Allows exporting ServicePrice records.")]
        public const string Export = "Permissions.ServicePrices.Export";

        [Description("Allows importing ServicePrice records.")]
        public const string Import = "Permissions.ServicePrices.Import";
    }
}
public class ServicePricesAccessRights
{
    public bool View { get; set; }
    public bool Create { get; set; }
    public bool Edit { get; set; }
    public bool Delete { get; set; }
    public bool Search { get; set; }
    public bool Export { get; set; }
    public bool Import { get; set; }
}

