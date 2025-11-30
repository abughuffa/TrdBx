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

        [Description("Allows modifying existing Ticket details.")]
        public const string Edit = "Permissions.Tickets.Edit";

        [Description("Allows deleting Ticket records.")]
        public const string Delete = "Permissions.Tickets.Delete";

        [Description("Allows searching for Ticket records.")]
        public const string Search = "Permissions.Tickets.Search";

        [Description("Allows exporting Ticket records.")]
        public const string Export = "Permissions.Tickets.Export";

        [Description("Allows Approve Ticket record.")]
        public const string Approve = "Permissions.Tickets.Approve";

        [Description("Allows Assign Ticket record.")]
        public const string Assign = "Permissions.Tickets.Assign";

        [Description("Allows Release Ticket record.")]
        public const string Release = "Permissions.Tickets.Release";

        [Description("Allows Reject Ticket record.")]
        public const string Reject = "Permissions.Tickets.Reject";

        [Description("Allows UnReject Ticket record.")]
        public const string UnReject = "Permissions.Tickets.UnReject";

        [Description("Allows Execute Ticket record.")]
        public const string Execute = "Permissions.Tickets.Execute";

        [Description("Allows Complete Ticket record.")]
        public const string Complete = "Permissions.Tickets.Complete";

        [Description("Allows Strat Ticket record.")]
        public const string Strat = "Permissions.Tickets.Strat";

        [Description("Allows Stop Ticket record.")]
        public const string Stop = "Permissions.Tickets.Stop";

    }
}
public class TicketsAccessRights
{
    public bool View { get; set; }
    public bool Create { get; set; }
    public bool Edit { get; set; }
    public bool Delete { get; set; }
    public bool Search { get; set; }
    public bool Export { get; set; }


    
          public bool Approve { get; set; }
    public bool Assign { get; set; }
    public bool Release { get; set; }
    public bool Reject { get; set; }
    public bool UnReject { get; set; }
    public bool Execute { get; set; }

    public bool Strat { get; set; }
    public bool Stop { get; set; }
    public bool Complete { get; set; }

    

}

