using System.ComponentModel;

namespace CleanArchitecture.Blazor.Infrastructure.PermissionSet;

public static partial class Permissions
{
    [DisplayName("Customer Permissions")]
    [Description("Set permissions for Customer operations.")]
    public static class Customers
    {
        [Description("Allows viewing Customer details.")]
        public const string View = "Permissions.Customers.View";

        [Description("Allows creating new Customer records.")]
        public const string Create = "Permissions.Customers.Create";

        [Description("Allows modifying existing Customer details.")]
        public const string Edit = "Permissions.Customers.Edit";

        [Description("Allows deleting Customer records.")]
        public const string Delete = "Permissions.Customers.Delete";

        [Description("Allows searching for Customer records.")]
        public const string Search = "Permissions.Customers.Search";

        [Description("Allows exporting Customer records.")]
        public const string Export = "Permissions.Customers.Export";

        [Description("Allows importing Customer records.")]
        public const string Import = "Permissions.Customers.Import";
    }
}
public class CustomersAccessRights
{
    public bool View { get; set; }
    public bool Create { get; set; }
    public bool Edit { get; set; }
    public bool Delete { get; set; }
    public bool Search { get; set; }
    public bool Export { get; set; }
    public bool Import { get; set; }
}

