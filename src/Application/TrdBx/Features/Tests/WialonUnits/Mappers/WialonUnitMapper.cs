
using CleanArchitecture.Blazor.Application.Features.WialonUnits.Commands.Update;
using CleanArchitecture.Blazor.Application.Features.WialonUnits.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.WialonUnits.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class Mapper
{
    public static partial WialonUnitDto ToDto(WialonUnit source);
    public static partial WialonUnit FromDto(WialonUnitDto dto);
    public static partial UpdateWialonUnitCommand ToUpdateCommand(WialonUnitDto dto);
    public static partial void ApplyChangesFrom(UpdateWialonUnitCommand source, WialonUnit target);
    public static partial IQueryable<WialonUnitDto> ProjectTo(this IQueryable<WialonUnit> q);
}

