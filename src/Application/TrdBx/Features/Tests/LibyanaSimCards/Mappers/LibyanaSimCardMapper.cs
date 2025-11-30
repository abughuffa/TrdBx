using CleanArchitecture.Blazor.Application.Features.LibyanaSimCards.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.LibyanaSimCards.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class Mapper
{
    public static partial LibyanaSimCardDto ToDto(LibyanaSimCard source);
    public static partial LibyanaSimCard FromDto(LibyanaSimCardDto dto);
    public static partial IQueryable<LibyanaSimCardDto> ProjectTo(this IQueryable<LibyanaSimCard> q);
}

