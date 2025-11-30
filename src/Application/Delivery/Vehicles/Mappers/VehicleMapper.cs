
using CleanArchitecture.Blazor.Application.Features.Vehicles.Commands.AddEdit;
using CleanArchitecture.Blazor.Application.Features.Vehicles.Commands.Create;
using CleanArchitecture.Blazor.Application.Features.Vehicles.Commands.Update;
using CleanArchitecture.Blazor.Application.Features.Vehicles.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.Vehicles.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class VehicleMapper
{
    public static partial VehicleDto ToDto(Vehicle source);
    public static partial Vehicle FromDto(VehicleDto dto);
    public static partial Vehicle FromEditCommand(AddEditVehicleCommand command);
    public static partial Vehicle FromCreateCommand(CreateVehicleCommand command);
    public static partial UpdateVehicleCommand ToUpdateCommand(VehicleDto dto);
    public static partial AddEditVehicleCommand CloneFromDto(VehicleDto dto);
    public static partial void ApplyChangesFrom(UpdateVehicleCommand source, Vehicle target);
    public static partial void ApplyChangesFrom(AddEditVehicleCommand source, Vehicle target);
    public static partial IQueryable<VehicleDto> ProjectTo(this IQueryable<Vehicle> q);
}

