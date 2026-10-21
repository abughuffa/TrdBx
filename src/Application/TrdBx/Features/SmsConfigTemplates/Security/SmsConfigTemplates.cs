using System.ComponentModel;
namespace CleanArchitecture.Blazor.Application.Common.Security;
public static partial class Permissions
{
    [DisplayName("SmsConfigTemplate Permissions")]
    [Description("Set permissions for SmsConfigTemplate operations.")]
    public static class SmsConfigTemplates
    {
        [Description("Allows viewing SmsConfigTemplate details.")]
        public const string View = "Permissions.SmsConfigTemplates.View";

        [Description("Allows creating new SmsConfigTemplate records.")]
        public const string Create = "Permissions.SmsConfigTemplates.Create";

        [Description("Allows modifying existing SmsConfigTemplate details.")]
        public const string Edit = "Permissions.SmsConfigTemplates.Edit";

        [Description("Allows deleting SmsConfigTemplate records.")]
        public const string Delete = "Permissions.SmsConfigTemplates.Delete";
                

    }
}

public class SmsConfigTemplatesAccessRights
{
    public bool View { get; set; }
    public bool Create { get; set; }
    public bool Edit { get; set; }
    public bool Delete { get; set; }
   


}

