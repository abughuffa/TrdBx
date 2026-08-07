using System.ComponentModel;
namespace CleanArchitecture.Blazor.Infrastructure.PermissionSet;

public static partial class Permissions
{
    [DisplayName("DailyTask Permissions")]
    [Description("Set permissions for DailyTask operations.")]
    public static class DailyTasks
    {
        [Description("Allows Install TrackingUnit.")]
        public const string Install = "Permissions.DailyTasks.Install";

        [Description("Allows Transfer TrackingUnit.")]
        public const string Transfer = "Permissions.DailyTasks.Transfer";

        [Description("Allows Replace TrackingUnit.")]
        public const string Replace = "Permissions.DailyTasks.Replace";

        [Description("Allows Recover TrackingUnit.")]
        public const string Recover = "Permissions.DailyTasks.Recover";

        [Description("Allows Reserve TrackingUnit.")]
        public const string Reserve = "Permissions.DailyTasks.Reserve";

        [Description("Allows Activate TrackingUnit.")]
        public const string Activate = "Permissions.DailyTasks.Activate";
        [Description("Allows ActivateForGprs TrackingUnit.")]
        public const string ActivateForGprs = "Permissions.DailyTasks.ActivateForGprs";

        [Description("Allows ActivateForHosting TrackingUnit.")]
        public const string ActivateForHosting = "Permissions.DailyTasks.ActivateForHosting";

        [Description("Allows Deactivate TrackingUnit.")]
        public const string Deactivate = "Permissions.DailyTasks.Deactivate";

        [Description("Allows Renew Subscription of TrackingUnit.")]
        public const string RenewSubscription = "Permissions.DailyTasks.RenewSubscription";

        [Description("Allows Sync Wialon unit Names with local Tracking units names.")]
        public const string SyncUnitNames = "Permissions.DailyTasks.SyncUnitNames";

        [Description("Allows Sync Libyana Sim Card Expairy dates with local Sim Card Expairy dates.")]
        public const string SyncSIMExpairyData = "Permissions.DailyTasks.SyncSIMExpairyData";
    }
}

public class DailyTasksAccessRights
{
    public bool View { get; set; }
    public bool Create { get; set; }
    public bool Edit { get; set; }
    public bool Delete { get; set; }
    public bool Search { get; set; }
    public bool Export { get; set; }
    public bool Import { get; set; }

    public bool Install { get; set; }
    public bool Transfer { get; set; }
    public bool Replace { get; set; }
    public bool Recover { get; set; }
    public bool Reserve { get; set; }
    public bool Activate { get; set; }
    public bool ActivateForHosting { get; set; }

    public bool ActivateForGprs { get; set; }
    public bool Deactivate { get; set; }
    public bool MarkAsDamaged { get; set; }
    public bool MarkAsUsed { get; set; }
    public bool ReassignOwner { get; set; }
    public bool RenewSubscription { get; set; }
    public bool AssignDailyTaskToOtherUser { get; set; }

    public bool CreateTicket { get; set; }
    


}

