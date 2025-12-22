using System.ComponentModel;

namespace CleanArchitecture.Blazor.Domain.Enums;
public enum ShipmentStatus
{
    [Description("null")] Null = 0,
    [Description("JustCreated")] JustCreated = 1,
    [Description("PubToBid")] PubToBid = 2,
    [Description("PubToTrans")] PubToTrans = 3,
    [Description("Assigned")] Assigned = 4,
    [Description("Accpted")] Accpted = 5,
    [Description("Delivering")] Delivering = 6,
    [Description("Unloading")] Unloading = 7,
    [Description("Delivered")] Delivered = 8
}


