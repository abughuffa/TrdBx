using CleanArchitecture.Blazor.Application.Features.WayPoints.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.WayPoints.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class WayPointMapper
{
    public static partial WayPointDto ToDto(WayPoint source);
    public static partial WayPoint FromDto(WayPointDto dto);
    public static partial IQueryable<WayPointDto> ProjectTo(this IQueryable<WayPoint> q);
    public static partial ICollection<WayPoint> ProjectFrom(this ICollection<WayPointDto> q);
}

