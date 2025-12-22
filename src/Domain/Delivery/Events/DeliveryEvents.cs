

namespace CleanArchitecture.Blazor.Domain.Events;

#region POI
public class POICreatedEvent : DomainEvent
{
    public POICreatedEvent(POI item)
    {
        Item = item;
    }

    public POI Item { get; }
}

public class POIUpdatedEvent : DomainEvent
{
    public POIUpdatedEvent(POI item)
    {
        Item = item;
    }

    public POI Item { get; }
}

public class POIDeletedEvent : DomainEvent
{
    public POIDeletedEvent(POI item)
    {
        Item = item;
    }

    public POI Item { get; }
}
#endregion

#region Shipment
public class ShipmentCreatedEvent : DomainEvent
{
    public ShipmentCreatedEvent(Shipment item)
    {
        Item = item;
    }

    public Shipment Item { get; }
}

public class ShipmentUpdatedEvent : DomainEvent
{
    public ShipmentUpdatedEvent(Shipment item)
    {
        Item = item;
    }

    public Shipment Item { get; }
}

public class ShipmentDeletedEvent : DomainEvent
{
    public ShipmentDeletedEvent(Shipment item)
    {
        Item = item;
    }

    public Shipment Item { get; }
}
#endregion

#region Vehicle
public class VehicleCreatedEvent : DomainEvent
{
    public VehicleCreatedEvent(Vehicle item)
    {
        Item = item;
    }

    public Vehicle Item { get; }
}

public class VehicleUpdatedEvent : DomainEvent
{
    public VehicleUpdatedEvent(Vehicle item)
    {
        Item = item;
    }

    public Vehicle Item { get; }
}

public class VehicleDeletedEvent : DomainEvent
{
    public VehicleDeletedEvent(Vehicle item)
    {
        Item = item;
    }

    public Vehicle Item { get; }
}
#endregion

#region VehicleType
public class VehicleTypeCreatedEvent : DomainEvent
{
    public VehicleTypeCreatedEvent(VehicleType item)
    {
        Item = item;
    }

    public VehicleType Item { get; }
}

public class VehicleTypeUpdatedEvent : DomainEvent
{
    public VehicleTypeUpdatedEvent(VehicleType item)
    {
        Item = item;
    }

    public VehicleType Item { get; }
}

public class VehicleTypeDeletedEvent : DomainEvent
{
    public VehicleTypeDeletedEvent(VehicleType item)
    {
        Item = item;
    }

    public VehicleType Item { get; }
}
#endregion

#region BidRecord
public class BidRecordCreatedEvent : DomainEvent
{
    public BidRecordCreatedEvent(BidRecord item)
    {
        Item = item;
    }

    public BidRecord Item { get; }
}

public class BidRecordUpdatedEvent : DomainEvent
{
    public BidRecordUpdatedEvent(BidRecord item)
    {
        Item = item;
    }

    public BidRecord Item { get; }
}

public class BidRecordDeletedEvent : DomainEvent
{
    public BidRecordDeletedEvent(BidRecord item)
    {
        Item = item;
    }

    public BidRecord Item { get; }
}
#endregion

