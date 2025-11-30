
using CleanArchitecture.Blazor.Application.Features.TrackingUnitModels.Commands.AddEdit;
//using CleanArchitecture.Blazor.Application.Features.TrackingUnitModels.Commands.Create;
//using CleanArchitecture.Blazor.Application.Features.TrackingUnitModels.Commands.Update;
using CleanArchitecture.Blazor.Application.Features.TrackingUnitModels.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.TrackingUnitModels.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class Mapper
{
    public static partial TrackingUnitModelDto ToDto(TrackingUnitModel source);
    public static partial TrackingUnitModel FromDto(TrackingUnitModelDto dto);
    public static partial TrackingUnitModel FromEditCommand(AddEditTrackingUnitModelCommand command);
    public static partial AddEditTrackingUnitModelCommand ToEditCommand(TrackingUnitModelDto dto);

    //public static partial TrackingUnitModel FromCreateCommand(CreateTrackingUnitModelCommand command);
    //public static partial UpdateTrackingUnitModelCommand ToUpdateCommand(TrackingUnitModelDto dto);
    public static partial AddEditTrackingUnitModelCommand CloneFromDto(TrackingUnitModelDto dto);
    //public static partial void ApplyChangesFrom(UpdateTrackingUnitModelCommand source, TrackingUnitModel target);
    public static partial void ApplyChangesFrom(AddEditTrackingUnitModelCommand source, TrackingUnitModel target);
    public static partial IQueryable<TrackingUnitModelDto> ProjectTo(this IQueryable<TrackingUnitModel> q);
}

