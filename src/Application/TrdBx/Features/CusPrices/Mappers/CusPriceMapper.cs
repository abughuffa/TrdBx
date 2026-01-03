
using CleanArchitecture.Blazor.Application.Features.CusPrices.Commands.Create;
using CleanArchitecture.Blazor.Application.Features.CusPrices.Commands.Update;
using CleanArchitecture.Blazor.Application.Features.CusPrices.DTOs;
using CleanArchitecture.Blazor.Application.Features.SimCards.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.CusPrices.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class Mapper
{
    [MapProperty(nameof(CusPrice.Customer.Name), nameof(CusPriceDto.Customer))]
    [MapProperty(nameof(CusPrice.TrackingUnitModel.Name), nameof(CusPriceDto.TrackingUnitModel))]
    public static partial CusPriceDto ToDto(CusPrice source);

    [MapperIgnoreSource(nameof(CusPriceDto.Customer))]
    [MapperIgnoreSource(nameof(CusPriceDto.TrackingUnitModel))]
    public static partial CusPrice FromDto(CusPriceDto dto);
    //public static partial CusPrice FromEditCommand(AddEditCusPriceCommand command);
    public static partial CusPrice FromCreateCommand(CreateCusPriceCommand command);

    public static partial UpdateCusPriceCommand ToUpdateCommand(CusPriceDto dto);
    //public static partial AddEditCusPriceCommand CloneFromDto(CusPriceDto dto);
    public static partial void ApplyChangesFrom(UpdateCusPriceCommand source, CusPrice target);
    //public static partial void ApplyChangesFrom(AddEditCusPriceCommand source, CusPrice target);
    public static partial IQueryable<CusPriceDto> ProjectTo(this IQueryable<CusPrice> q);
}

