using System.ComponentModel;
namespace CleanArchitecture.Blazor.Infrastructure.PermissionSet;

public static partial class Permissions
{
    [DisplayName("ImpulseChart Permissions")]
    [Description("Set permissions for ImpulseChart operations.")]
    public static class ImpulseCharts
    {
        [Description("Allows viewing ImpulseChart.")]
        public const string View = "Permissions.ImpulseCharts.View";

        [Description("Allows exporting ImpulseChart records.")]
        public const string Export = "Permissions.ImpulseCharts.Export";
    }
}

public class ChartsAccessRights
{
    public bool View { get; set; }
    public bool Export { get; set; }
}




