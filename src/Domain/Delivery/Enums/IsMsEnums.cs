using System.ComponentModel;

namespace CleanArchitecture.Blazor.Domain.Enums;
public enum ShipmentStatus
{
    [Description("null")] Null = 0,
    [Description("JustCreated")] JustCreated = 1,
    [Description("Assigned")] Assigned = 2,
    [Description("Delivering")] Delivering = 3,
    [Description("Unloading")] Unloading = 4,
    [Description("Completed")] Completed = 4
}


