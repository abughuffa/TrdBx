
using CleanArchitecture.Blazor.Application.Features.Shipments.Commands.AddEdit;
using CleanArchitecture.Blazor.Application.Features.Shipments.Commands.Create;
using CleanArchitecture.Blazor.Application.Features.Shipments.Commands.Update;
using CleanArchitecture.Blazor.Application.Features.Shipments.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.Shipments.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class ShipmentMapper
{
    public static partial ShipmentDto ToDto(Shipment source);
    public static partial Shipment FromDto(ShipmentDto dto);
    public static partial Shipment FromEditCommand(AddEditShipmentCommand command);
    public static partial Shipment FromCreateCommand(CreateShipmentCommand command);
    public static partial UpdateShipmentCommand ToUpdateCommand(ShipmentDto dto);
    public static partial AddEditShipmentCommand CloneFromDto(ShipmentDto dto);
    public static partial void ApplyChangesFrom(UpdateShipmentCommand source, Shipment target);
    public static partial void ApplyChangesFrom(AddEditShipmentCommand source, Shipment target);
    public static partial IQueryable<ShipmentDto> ProjectTo(this IQueryable<Shipment> q);
}

