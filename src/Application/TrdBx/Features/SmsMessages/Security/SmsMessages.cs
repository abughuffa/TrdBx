using System.ComponentModel;
namespace CleanArchitecture.Blazor.Application.Common.Security;
public static partial class Permissions
{
    [DisplayName("SmsMessage Permissions")]
    [Description("Set permissions for SmsMessage operations.")]
    public static class SmsMessages
    {
        [Description("Allows viewing SmsMessage details.")]
        public const string View = "Permissions.SmsMessages.View";
        [Description("Allows sending SmsMessage records.")]
        public const string Send = "Permissions.SmsMessages.Send";

        [Description("Allows to update Gateway settings SmsMessage.")]
        public const string UpdateSettings = "Permissions.SmsMessages.UpdateSettings";
        
        [Description("Allows deleting SmsMessage.")]
        public const string Delete = "Permissions.SmsMessages.Delete";
        [Description("Allows exporting SmsMessage records.")]
        public const string Export = "Permissions.SmsMessages.Export";

    }
}
public class SmsMessagesAccessRights
{
    public bool View { get; set; }
    public bool Send { get; set; }
    public bool UpdateSettings { get; set; }
    public bool Delete { get; set; }
    public bool Export { get; set; }
}

