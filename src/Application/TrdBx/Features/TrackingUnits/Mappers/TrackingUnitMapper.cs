

using CleanArchitecture.Blazor.Application.Features.Tickets.DTOs;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.Create;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.ActivateTrackingUnit;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.ActivateTrackingUnitForGprs;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.ActivateTrackingUnitForHosting;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.DeactivateTrackingUnit;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.Install;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.Recover;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.Replace;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.Reserve;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.Transfer;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.Update;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class Mapper
{
    public static partial TrackingUnitDto ToDto(TrackingUnit source);

    [MapperIgnoreSource(nameof(TrackingUnitDto.Customer))]
    [MapperIgnoreSource(nameof(TrackingUnitDto.SimCard))]
    [MapperIgnoreSource(nameof(TrackingUnitDto.TrackedAsset))]
    [MapperIgnoreSource(nameof(TrackingUnitDto.TrackingUnitModel))]
    public static partial TrackingUnit FromDto(TrackingUnitDto dto);
    public static partial TrackingUnit FromCreateCommand(CreateTrackingUnitCommand command);
    public static partial UpdateTrackingUnitCommand ToUpdateCommand(TrackingUnitDto dto);
    public static partial void ApplyChangesFrom(UpdateTrackingUnitCommand source, TrackingUnit target);
    public static partial IQueryable<TrackingUnitDto> ProjectTo(this IQueryable<TrackingUnit> q);


    [MapperIgnoreSource(nameof(TrackingUnitDto.Customer))]
    [MapperIgnoreSource(nameof(TrackingUnitDto.SimCard))]
    [MapperIgnoreSource(nameof(TrackingUnitDto.TrackedAsset))]
    [MapperIgnoreSource(nameof(TrackingUnitDto.TrackingUnitModel))]
    public static partial ActivateTrackingUnitCommand ToActivateCommand(TrackingUnitDto dto);
    [MapperIgnoreSource(nameof(TrackingUnitDto.Customer))]
    [MapperIgnoreSource(nameof(TrackingUnitDto.SimCard))]
    [MapperIgnoreSource(nameof(TrackingUnitDto.TrackedAsset))]
    [MapperIgnoreSource(nameof(TrackingUnitDto.TrackingUnitModel))]
    public static partial ActivateTrackingUnitForHostingCommand ToActivateHostingCommand(TrackingUnitDto dto);
    [MapperIgnoreSource(nameof(TrackingUnitDto.Customer))]
    [MapperIgnoreSource(nameof(TrackingUnitDto.SimCard))]
    [MapperIgnoreSource(nameof(TrackingUnitDto.TrackedAsset))]
    [MapperIgnoreSource(nameof(TrackingUnitDto.TrackingUnitModel))]
    public static partial ActivateTrackingUnitForGprsCommand ToActivateGprsCommand(TrackingUnitDto dto);
    [MapperIgnoreSource(nameof(TrackingUnitDto.Customer))]
    [MapperIgnoreSource(nameof(TrackingUnitDto.SimCard))]
    [MapperIgnoreSource(nameof(TrackingUnitDto.TrackedAsset))]
    [MapperIgnoreSource(nameof(TrackingUnitDto.TrackingUnitModel))]
    public static partial DeactivateTrackingUnitCommand ToDeactivateCommand(TrackingUnitDto dto);



    [MapperIgnoreSource(nameof(TrackingUnitDto.Customer))]
    [MapperIgnoreSource(nameof(TrackingUnitDto.SimCard))]
    [MapperIgnoreSource(nameof(TrackingUnitDto.TrackedAsset))]
    [MapperIgnoreSource(nameof(TrackingUnitDto.TrackingUnitModel))]
    public static partial InstallTrackingUnitCommand ToInstallCommand(TrackingUnitDto dto);
    [MapperIgnoreSource(nameof(TrackingUnitDto.Customer))]
    [MapperIgnoreSource(nameof(TrackingUnitDto.SimCard))]
    [MapperIgnoreSource(nameof(TrackingUnitDto.TrackedAsset))]
    [MapperIgnoreSource(nameof(TrackingUnitDto.TrackingUnitModel))]
    public static partial RecoverTrackingUnitCommand ToRecoverCommand(TrackingUnitDto dto);
    [MapperIgnoreSource(nameof(TrackingUnitDto.Customer))]
    [MapperIgnoreSource(nameof(TrackingUnitDto.SimCard))]
    [MapperIgnoreSource(nameof(TrackingUnitDto.TrackedAsset))]
    [MapperIgnoreSource(nameof(TrackingUnitDto.TrackingUnitModel))]
    public static partial TransferTrackingUnitCommand ToTransferCommand(TrackingUnitDto dto);
    [MapperIgnoreSource(nameof(TrackingUnitDto.Customer))]
    [MapperIgnoreSource(nameof(TrackingUnitDto.SimCard))]
    [MapperIgnoreSource(nameof(TrackingUnitDto.TrackedAsset))]
    [MapperIgnoreSource(nameof(TrackingUnitDto.TrackingUnitModel))]
    public static partial ReplaceTrackingUnitCommand ToReplaceCommand(TrackingUnitDto dto);

    [MapperIgnoreSource(nameof(TrackingUnitDto.Customer))]
    [MapperIgnoreSource(nameof(TrackingUnitDto.SimCard))]
    [MapperIgnoreSource(nameof(TrackingUnitDto.TrackedAsset))]
    [MapperIgnoreSource(nameof(TrackingUnitDto.TrackingUnitModel))]
    public static partial ReserveTrackingUnitCommand ToReserveCommand(TrackingUnitDto dto);

    
}

