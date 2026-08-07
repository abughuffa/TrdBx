using System.ComponentModel;
namespace CleanArchitecture.Blazor.Infrastructure.PermissionSet;

public static partial class Permissions
{
    [DisplayName("Ticket Permissions")]
    [Description("Set permissions for Ticket operations.")]
    public static class Tickets
    {
        [Description("Allows viewing Ticket details.")]
        public const string View = "Permissions.Tickets.View";

        [Description("Allows creating new Ticket records.")]
        public const string Create = "Permissions.Tickets.Create";

        [Description("Allows deleting Ticket records.")]
        public const string Delete = "Permissions.Tickets.Delete";

        [Description("Allows exporting Ticket records.")]
        public const string Export = "Permissions.Tickets.Export";

        [Description("Allows Approve Ticket record.")]
        public const string Approve = "Permissions.Tickets.Approve";

        [Description("Allows Reject Ticket record.")]
        public const string Reject = "Permissions.Tickets.Reject";

        [Description("Allows UnReject Ticket record.")]
        public const string UnReject = "Permissions.Tickets.UnReject";

        [Description("Allows Execute Ticket record.")]
        public const string Execute = "Permissions.Tickets.Execute";

    }
}
public class TicketsAccessRights
{
    public bool View { get; set; }
    public bool Create { get; set; }
    public bool Delete { get; set; }
    public bool Export { get; set; }


    
    public bool Approve { get; set; }
    public bool Reject { get; set; }
    public bool UnReject { get; set; }
    public bool Execute { get; set; }

    

}

