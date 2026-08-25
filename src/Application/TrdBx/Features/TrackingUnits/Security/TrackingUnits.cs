using System.ComponentModel;
namespace CleanArchitecture.Blazor.Application.Common.Security;
public static partial class Permissions
{
    [DisplayName("TrackingUnit Permissions")]
    [Description("Set permissions for TrackingUnit operations.")]
    public static class TrackingUnits
    {
        [Description("Allows viewing TrackingUnit details.")]
        public const string View = "Permissions.TrackingUnits.View";

        [Description("Allows creating new TrackingUnit records.")]
        public const string Create = "Permissions.TrackingUnits.Create";

        [Description("Allows modifying existing TrackingUnit details.")]
        public const string Edit = "Permissions.TrackingUnits.Edit";

        [Description("Allows deleting TrackingUnit records.")]
        public const string Delete = "Permissions.TrackingUnits.Delete";
                [Description("Allows Reserve TrackingUnit.")]
        public const string Reserve = "Permissions.TrackingUnits.Reserve";

        [Description("Allows MarkAsDamaged TrackingUnit.")]
        public const string MarkAsDamaged = "Permissions.TrackingUnits.MarkAsDamaged";


        [Description("Allows MarkAsUsed TrackingUnit.")]
        public const string MarkAsUsed = "Permissions.TrackingUnits.MarkAsUsed";

        [Description("Allows MarkAsLost TrackingUnit.")]
        public const string MarkAsLost = "Permissions.TrackingUnits.MarkAsLost";

        [Description("Allows ReassignOwner TrackingUnit.")]
        public const string ReassignOwner = "Permissions.TrackingUnits.ReassignOwner";

        [Description("Allows exporting TrackingUnit records.")]
        public const string Export = "Permissions.TrackingUnits.Export";

        [Description("Allows importing TrackingUnit records.")]
        public const string Import = "Permissions.TrackingUnits.Import";

        [Description("Allows SyncUnitNames of TrackingUnit records.")]
        public const string SyncUnitNames = "Permissions.TrackingUnits.SyncUnitNames";

    }
}

public class TrackingUnitsAccessRights
{
    public bool View { get; set; }
    public bool Create { get; set; }
    public bool Edit { get; set; }
    public bool Delete { get; set; }
    public bool Export { get; set; }
    public bool Import { get; set; }

    public bool MarkAsDamaged { get; set; }
    public bool MarkAsUsed { get; set; }

      public bool Reserve { get; set; }

        public bool MarkAsLost { get; set; }
    public bool ReassignOwner { get; set; }

      public bool SyncUnitNames { get; set; }

    


}

