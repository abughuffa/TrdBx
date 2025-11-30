using System.ComponentModel;
namespace CleanArchitecture.Blazor.Infrastructure.PermissionSet;

public static partial class Permissions
{
    [DisplayName("ActivateTestCase Permissions")]
    [Description("Set permissions for ActivateTestCase operations.")]
    public static class ActivateTestCases
    {
        [Description("Allows viewing ActivateTestCase details.")]
        public const string View = "Permissions.ActivateTestCases.View";

        [Description("Allows deleting ActivateTestCase records.")]
        public const string Delete = "Permissions.ActivateTestCases.Delete";

        [Description("Allows searching for ActivateTestCase records.")]
        public const string Search = "Permissions.ActivateTestCases.Search";

        [Description("Allows Execute ActivateTestCase records.")]
        public const string Execute = "Permissions.ActivateTestCases.Execute";

        [Description("Allows importing ActivateTestCase records.")]
        public const string Import = "Permissions.ActivateTestCases.Import";
    }
}
public class ActivateTestCasesAccessRights
{
    public bool View { get; set; }
    public bool Delete { get; set; }
    public bool Search { get; set; }
    public bool Execute { get; set; }
    public bool Import { get; set; }
}

