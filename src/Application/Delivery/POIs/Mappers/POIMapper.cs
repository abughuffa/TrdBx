
using CleanArchitecture.Blazor.Application.Features.POIs.Commands.AddEdit;
using CleanArchitecture.Blazor.Application.Features.POIs.Commands.Create;
using CleanArchitecture.Blazor.Application.Features.POIs.Commands.Update;
using CleanArchitecture.Blazor.Application.Features.POIs.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.POIs.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class POIMapper
{
    public static partial POIDto ToDto(POI source);
    public static partial POI FromDto(POIDto dto);
    public static partial POI FromEditCommand(AddEditPOICommand command);
    public static partial POI FromCreateCommand(CreatePOICommand command);
    public static partial UpdatePOICommand ToUpdateCommand(POIDto dto);
    public static partial AddEditPOICommand CloneFromDto(POIDto dto);
    public static partial void ApplyChangesFrom(UpdatePOICommand source, POI target);
    public static partial void ApplyChangesFrom(AddEditPOICommand source, POI target);
    public static partial IQueryable<POIDto> ProjectTo(this IQueryable<POI> q);
}

