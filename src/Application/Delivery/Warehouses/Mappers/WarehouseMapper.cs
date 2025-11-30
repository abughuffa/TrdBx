
using CleanArchitecture.Blazor.Application.Features.Warehouses.Commands.AddEdit;
using CleanArchitecture.Blazor.Application.Features.Warehouses.Commands.Create;
using CleanArchitecture.Blazor.Application.Features.Warehouses.Commands.Update;
using CleanArchitecture.Blazor.Application.Features.Warehouses.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.Warehouses.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class WarehouseMapper
{
    public static partial WarehouseDto ToDto(Warehouse source);
    public static partial Warehouse FromDto(WarehouseDto dto);
    public static partial Warehouse FromEditCommand(AddEditWarehouseCommand command);
    public static partial Warehouse FromCreateCommand(CreateWarehouseCommand command);
    public static partial UpdateWarehouseCommand ToUpdateCommand(WarehouseDto dto);
    public static partial AddEditWarehouseCommand CloneFromDto(WarehouseDto dto);
    public static partial void ApplyChangesFrom(UpdateWarehouseCommand source, Warehouse target);
    public static partial void ApplyChangesFrom(AddEditWarehouseCommand source, Warehouse target);
    public static partial IQueryable<WarehouseDto> ProjectTo(this IQueryable<Warehouse> q);
}

