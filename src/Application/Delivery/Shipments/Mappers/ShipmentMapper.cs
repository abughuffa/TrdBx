
//using CleanArchitecture.Blazor.Application.Features.Shipments.Commands.AddEdit;
using CleanArchitecture.Blazor.Application.Features.Shipments.Commands.Create;
using CleanArchitecture.Blazor.Application.Features.Shipments.Commands.Update;
using CleanArchitecture.Blazor.Application.Features.Shipments.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.Shipments.Mappers;
//#region Assembly Riok.Mapperly.Abstractions, Version=4.3.0.0, Culture=neutral, PublicKeyToken=null
//// C:\Users\user\.nuget\packages\riok.mapperly\4.3.0\lib\netstandard2.0\Riok.Mapperly.Abstractions.dll
//// Decompiled with ICSharpCode.Decompiler 9.1.0.7988
//#endregion
#pragma warning disable RMG020
#pragma warning disable RMG012
//[Mapper]
//public static partial class ShipmentMapper
//{
//    public static partial ShipmentDto ToDto(Shipment source);
//    public static partial Shipment FromDto(ShipmentDto dto);
//    public static partial Shipment FromEditCommand(AddEditShipmentCommand command);
//    public static partial Shipment FromCreateCommand(CreateShipmentCommand command);
//    public static partial UpdateShipmentCommand ToUpdateCommand(ShipmentDto dto);
//    public static partial AddEditShipmentCommand CloneFromDto(ShipmentDto dto);
//    public static partial void ApplyChangesFrom(UpdateShipmentCommand source, Shipment target);
//    public static partial void ApplyChangesFrom(AddEditShipmentCommand source, Shipment target);
//    public static partial IQueryable<ShipmentDto> ProjectTo(this IQueryable<Shipment> q);
//}

[Mapper]
public static partial class ShipmentMapper
{
    public static partial ShipmentDto ToDto(this Shipment shipment);

    public static partial IQueryable<ShipmentDto> ProjectToDto(this IQueryable<Shipment> query);

    public static partial UpdateShipmentCommand ToUpdateCommand(this ShipmentDto dto);

    public static partial CreateShipmentCommand ToCreateCommand(this ShipmentDto dto);

    // Custom mapping for Shipment to ShipmentDto with vehicle types
    public static ShipmentDto ToDtoWithVehicleTypes(this Shipment shipment)
    {
        var dto = shipment.ToDto();
        dto.RecVehicleType = shipment.VehicleTypes.Select(vt => vt.VehicleTypeId).ToArray();
        dto.RecVehicleTypeNames = shipment.VehicleTypes.Select(vt => vt.VehicleType.Name).ToArray();
        return dto;
    }

    // Custom mapping for collections
    public static List<ShipmentDto> ToDtosWithVehicleTypes(this IEnumerable<Shipment> shipments)
    {
        return shipments.Select(ToDtoWithVehicleTypes).ToList();
    }

    // For mapping from command to entity (for create/update)
    public static Shipment ToEntity(this CreateShipmentCommand command)
    {
        var shipment = new Shipment
        {
            ShipmentNo = command.ShipmentNo,
            StartLocation = command.StartLocation,
            EndLocation = command.EndLocation,
            Price = command.Price,
            IsBidable = command.IsBidable
        };
        return shipment;
    }

    public static void ToEntity(this UpdateShipmentCommand command, Shipment shipment)
    {
        shipment.ShipmentNo = command.ShipmentNo;
        shipment.StartLocation = command.StartLocation;
        shipment.EndLocation = command.EndLocation;
        shipment.Price = command.Price;
        shipment.IsBidable = command.IsBidable;
    }
}

