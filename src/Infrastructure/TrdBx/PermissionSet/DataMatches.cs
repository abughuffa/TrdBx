using System.ComponentModel;
namespace CleanArchitecture.Blazor.Infrastructure.PermissionSet;

public static partial class Permissions
{
    [DisplayName("DataMatch Permissions")]
    [Description("Set permissions for DataMatch operations.")]
    public static class DataMatches
    {
        [Description("Allows viewing DataMatch details.")]
        public const string View = "Permissions.DataMatches.View";

        [Description("Allows searching for DataMatch records.")]
        public const string Search = "Permissions.DataMatches.Search";

        [Description("Allows exporting DataMatch records.")]
        public const string Export = "Permissions.DataMatches.Export";



    }
}

public class DbMatchingsAccessRights
{
    public bool View { get; set; }
    public bool Search { get; set; }
    public bool Export { get; set; }
}




