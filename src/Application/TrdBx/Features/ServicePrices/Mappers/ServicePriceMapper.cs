using CleanArchitecture.Blazor.Application.Features.ServicePrices.Commands.Update;
using CleanArchitecture.Blazor.Application.Features.ServicePrices.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.ServicePrices.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class Mapper
{
    public static partial ServicePriceDto ToDto(ServicePrice source);
    public static partial ServicePrice FromDto(ServicePriceDto dto);
    //public static partial CusPrice FromEditCommand(AddEditCusPriceCommand command);
    //public static partial CusPrice FromCreateCommand(CreateCuCommand command);
    public static partial UpdateServicePriceCommand ToUpdateCommand(ServicePriceDto dto);
    //public static partial AddEditCusPriceCommand CloneFromDto(CusPriceDto dto);
    public static partial void ApplyChangesFrom(UpdateServicePriceCommand source, ServicePrice target);
    //public static partial void ApplyChangesFrom(AddEditCusPriceCommand source, CusPrice target);
    public static partial IQueryable<ServicePriceDto> ProjectTo(this IQueryable<ServicePrice> q);
}

