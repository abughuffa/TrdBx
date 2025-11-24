
using CleanArchitecture.Blazor.Application.Features.VehicleTypes.Commands.AddEdit;
using CleanArchitecture.Blazor.Application.Features.VehicleTypes.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.VehicleTypes.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class VehicleTypeMapper
{
    public static partial VehicleTypeDto ToDto(VehicleType source);
    public static partial VehicleType FromDto(VehicleTypeDto dto);
    public static partial VehicleType FromEditCommand(AddEditVehicleTypeCommand command);
    public static partial AddEditVehicleTypeCommand CloneFromDto(VehicleTypeDto dto);
    public static partial void ApplyChangesFrom(AddEditVehicleTypeCommand source, VehicleType target);
    public static partial IQueryable<VehicleTypeDto> ProjectTo(this IQueryable<VehicleType> q);
}

