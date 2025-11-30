
using CleanArchitecture.Blazor.Application.Features.Drivers.DTOs;
using CleanArchitecture.Blazor.Domain.Identity;

namespace CleanArchitecture.Blazor.Application.Features.Drivers.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class DriverMapper
{
    public static partial DriverDto ToDto(ApplicationUser source);
    public static partial IQueryable<DriverDto> ProjectTo(this IQueryable<ApplicationUser> q);
}

