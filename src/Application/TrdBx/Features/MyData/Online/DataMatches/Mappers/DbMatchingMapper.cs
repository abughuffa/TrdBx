
using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.DataMatches.DTOs;



namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.DataMatches.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class Mapper
{
    public static partial DataMatchDto ToDto(DataMatch source);

    public static partial DataMatch FromDto(DataMatchDto source);
    public static partial IQueryable<DataMatchDto> ProjectTo(this IQueryable<DataMatch> q);
}

