using CleanArchitecture.Blazor.Application.Features.CusPrices.DTOs;
using CleanArchitecture.Blazor.Application.Features.Subscriptions.DTOs;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.ActivateTrackingUnit;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.DTOs;
using CleanArchitecture.Blazor.Application.Features.WialonTasks.Commands.Execute;
using CleanArchitecture.Blazor.Application.Features.WialonTasks.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.WialonTasks.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class Mapper
{
    [MapProperty(nameof(WialonTask.TrackingUnit.SNo), nameof(WialonTaskDto.TrackingUnit))]
    [MapProperty(nameof(WialonTask.ServiceLog.ServiceNo), nameof(WialonTaskDto.ServiceLog))]
    public static partial WialonTaskDto ToDto(WialonTask source);


    [MapperIgnoreSource(nameof(WialonTaskDto.ServiceLog))]
    [MapperIgnoreSource(nameof(WialonTaskDto.TrackingUnit))]
    public static partial WialonTask FromDto(WialonTaskDto dto);
    public static partial IQueryable<WialonTaskDto> ProjectTo(this IQueryable<WialonTask> q);



    [MapperIgnoreSource(nameof(WialonTaskDto.ServiceLog))]
    [MapperIgnoreSource(nameof(WialonTaskDto.TrackingUnit))]
    [MapperIgnoreSource(nameof(WialonTaskDto.ExcDate))]
    public static partial ExecuteWialonTaskCommand ToExecuteCommand(WialonTaskDto dto);
}

