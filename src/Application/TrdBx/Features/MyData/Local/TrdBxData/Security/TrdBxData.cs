using System.ComponentModel;
namespace CleanArchitecture.Blazor.Application.Common.Security;
public static partial class Permissions
{
    [DisplayName("TrdBxData Permissions")]
    [Description("Set permissions for TrdBxData operations.")]
    public static class TrdBxData
    {
        [Description("Allows viewing TrdBxData.")]
        public const string View = "Permissions.TrdBxData.View";

        [Description("Allows exporting TrdBxData records.")]
        public const string Export = "Permissions.TrdBxData.Export";

        [Description("Allows importing TrdBxData records.")]
        public const string Import = "Permissions.TrdBxData.Import";

        [Description("Allows Delete all TrdBxData records.")]
        public const string Delete = "Permissions.TrdBxData.Delete";
    }
}

public class TrdBxDataAccessRights
{
    public bool View { get; set; }
    public bool Export { get; set; }
    public bool Import { get; set; }
    public bool Delete { get; set; }
}




