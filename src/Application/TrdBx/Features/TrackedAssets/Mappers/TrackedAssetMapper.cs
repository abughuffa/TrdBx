
//using CleanArchitecture.Blazor.Application.Features.TrackedAssets.Commands.AddEdit;
using CleanArchitecture.Blazor.Application.Features.TrackedAssets.Commands.Create;
using CleanArchitecture.Blazor.Application.Features.TrackedAssets.Commands.Update;
using CleanArchitecture.Blazor.Application.Features.TrackedAssets.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.TrackedAssets.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class Mapper
{
    public static partial TrackedAssetDto ToDto(TrackedAsset source);
    public static partial TrackedAsset FromDto(TrackedAssetDto dto);
    //public static partial TrackedAsset FromEditCommand(AddEditTrackedAssetCommand command);
    public static partial TrackedAsset FromCreateCommand(CreateTrackedAssetCommand command);
    public static partial UpdateTrackedAssetCommand ToUpdateCommand(TrackedAssetDto dto);
    //public static partial AddEditTrackedAssetCommand CloneFromDto(TrackedAssetDto dto);
    public static partial void ApplyChangesFrom(UpdateTrackedAssetCommand source, TrackedAsset target);
    //public static partial void ApplyChangesFrom(AddEditTrackedAssetCommand source, TrackedAsset target);
    public static partial IQueryable<TrackedAssetDto> ProjectTo(this IQueryable<TrackedAsset> q);
}

