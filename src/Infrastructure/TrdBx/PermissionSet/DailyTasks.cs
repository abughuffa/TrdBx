<<<<<<< HEAD
using System.ComponentModel;
=======
﻿using System.ComponentModel;
>>>>>>> agents/vscode-light-theme-setup
namespace CleanArchitecture.Blazor.Infrastructure.PermissionSet;

public static partial class Permissions
{
    [DisplayName("DailyTask Permissions")]
    [Description("Set permissions for DailyTask operations.")]
    public static class DailyTasks
    {

        [Description("Allows Install TrackingUnit.")]
        public const string InstallUnit = "Permissions.DailyTasks.InstallUnit";

        [Description("Allows Install / Replace Sim of TrackingUnit.")]
        public const string InstallOrReplaceSim = "Permissions.DailyTasks.InstallOrReplaceSim";

        [Description("Allows Transfer TrackingUnit.")]
        public const string TransferUnit = "Permissions.DailyTasks.TransferUnit";

        [Description("Allows Replace TrackingUnit.")]
        public const string ReplaceUnit = "Permissions.DailyTasks.ReplaceUnit";

        [Description("Allows Recover TrackingUnit.")]
        public const string RecoverUnit = "Permissions.DailyTasks.RecoverUnit";



        
        [Description("Allows Activate TrackingUnit.")]
        public const string ActivateUnit = "Permissions.DailyTasks.ActivateUnit";
        [Description("Allows Activate TrackingUnit For Gprs.")]
        public const string ActivateUnitForGprs = "Permissions.DailyTasks.ActivateUnitForGprs";

        [Description("Allows Activate TrackingUnit For Hosting .")]
        public const string ActivateUnitForHosting = "Permissions.DailyTasks.ActivateUnitForHosting";

        [Description("Allows Deactivate TrackingUnit.")]
        public const string DeactivateUnit = "Permissions.DailyTasks.DeactivateUnit";

        [Description("Allows Renew TrackingUnit Subscription.")]
        public const string RenewUnitSubscription = "Permissions.DailyTasks.RenewUnitSubscription";

        [Description("Allows Recover SIM from TrackingUnit.")]
        public const string RecoverSim = "Permissions.DailyTasks.RecoverSim";
    }
}
