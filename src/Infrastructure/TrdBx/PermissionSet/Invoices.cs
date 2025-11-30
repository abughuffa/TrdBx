using System.ComponentModel;
namespace CleanArchitecture.Blazor.Infrastructure.PermissionSet;

public static partial class Permissions
{
    [DisplayName("Invoice Permissions")]
    [Description("Set permissions for Invoice operations.")]
    public static class Invoices
    {
        [Description("Allows viewing Invoice details.")]
        public const string View = "Permissions.Invoices.View";

        [Description("Allows creating new Invoice records.")]
        public const string Create = "Permissions.Invoices.Create";

        [Description("Allows modifying existing Invoice details.")]
        public const string Edit = "Permissions.Invoices.Edit";

        [Description("Allows deleting Invoice records.")]
        public const string Delete = "Permissions.Invoices.Delete";

        [Description("Allows searching for Invoice records.")]
        public const string Search = "Permissions.Invoices.Search";

        [Description("Allows exporting Invoice records.")]
        public const string Export = "Permissions.Invoices.Export";



    }
}
public class InvoicesAccessRights
{
    public bool View { get; set; }
    public bool Create { get; set; }
    public bool Edit { get; set; }
    public bool Delete { get; set; }
    public bool Search { get; set; }
    public bool Export { get; set; }
}

