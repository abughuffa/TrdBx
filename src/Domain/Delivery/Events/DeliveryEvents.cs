

namespace CleanArchitecture.Blazor.Domain.Events;

#region Warehouse
public class WarehouseCreatedEvent : DomainEvent
{
    public WarehouseCreatedEvent(Warehouse item)
    {
        Item = item;
    }

    public Warehouse Item { get; }
}

public class WarehouseUpdatedEvent : DomainEvent
{
    public WarehouseUpdatedEvent(Warehouse item)
    {
        Item = item;
    }

    public Warehouse Item { get; }
}

public class WarehouseDeletedEvent : DomainEvent
{
    public WarehouseDeletedEvent(Warehouse item)
    {
        Item = item;
    }

    public Warehouse Item { get; }
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