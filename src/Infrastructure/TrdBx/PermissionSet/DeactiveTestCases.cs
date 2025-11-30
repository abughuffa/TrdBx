using System.ComponentModel;
namespace CleanArchitecture.Blazor.Infrastructure.PermissionSet;

public static partial class Permissions
{
    [DisplayName("DeactivateTestCase Permissions")]
    [Description("Set permissions for DeactivateTestCase operations.")]
    public static class DeactivateTestCases
    {
        [Description("Allows viewing DeactivateTestCase details.")]
        public const string View = "Permissions.DeactivateTestCases.View";

        [Description("Allows deleting DeactivateTestCase records.")]
        public const string Delete = "Permissions.DeactivateTestCases.Delete";

        [Description("Allows searching for DeactivateTestCase records.")]
        public const string Search = "Permissions.DeactivateTestCases.Search";

        [Description("Allows Execute DeactivateTestCase records.")]
        public const string Execute = "Permissions.DeactivateTestCases.Execute";

        [Description("Allows importing DeactivateTestCase records.")]
        public const string Import = "Permissions.DeactivateTestCases.Import";
    }
}
public class DeactivateTestCasesAccessRights
{
    public bool View { get; set; }
    public bool Delete { get; set; }
    public bool Search { get; set; }
    public bool Execute { get; set; }
    public bool Import { get; set; }
}

