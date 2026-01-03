using System.ComponentModel;

namespace CleanArchitecture.Blazor.Infrastructure.PermissionSet;

public static partial class Permissions
{
    [DisplayName("DataDiagnosis Permissions")]
    [Description("Set permissions for DataDiagnosis operations.")]
    public static class DataDiagnosises
    {
        [Description("Allows viewing DataDiagnosis details.")]
        public const string View = "Permissions.DataDiagnosises.View";

        [Description("Allows searching for DataDiagnosis records.")]
        public const string Search = "Permissions.DataDiagnosises.Search";

        [Description("Allows exporting DataDiagnosis records.")]
        public const string Export = "Permissions.DataDiagnosises.Export";



    }
}

public class DiagnosticsAccessRights
{
    public bool View { get; set; }
    public bool Search { get; set; }
    public bool Export { get; set; }
}




