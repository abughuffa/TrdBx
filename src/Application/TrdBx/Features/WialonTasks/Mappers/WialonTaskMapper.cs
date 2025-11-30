using CleanArchitecture.Blazor.Application.Features.CusPrices.DTOs;
using CleanArchitecture.Blazor.Application.Features.WialonTasks.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.WialonTasks.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class Mapper
{
    public static partial WialonTaskDto ToDto(WialonTask source);


    [MapperIgnoreSource(nameof(WialonTaskDto.ServiceLog))]
    [MapperIgnoreSource(nameof(WialonTaskDto.TrackingUnit))]
    public static partial WialonTask FromDto(WialonTaskDto dto);
    public static partial IQueryable<WialonTaskDto> ProjectTo(this IQueryable<WialonTask> q);
}

