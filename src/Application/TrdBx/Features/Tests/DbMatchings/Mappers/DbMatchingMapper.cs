
//using CleanArchitecture.Blazor.Application.Features.DbMatchings.Commands.AddEdit;
//using CleanArchitecture.Blazor.Application.Features.DbMatchings.Commands.Create;
//using CleanArchitecture.Blazor.Application.Features.DbMatchings.Commands.Update;
using CleanArchitecture.Blazor.Application.Features.DbMatchings.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.DbMatchings.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class Mapper
{
    public static partial DbMatchingDto ToDto(DbMatching source);
    public static partial DbMatching FromDto(DbMatchingDto dto);
    //public static partial DbMatching FromEditCommand(AddEditDbMatchingCommand command);
    ////public static partial DbMatching FromCreateCommand(CreateDbMatchingCommand command);
    ////public static partial UpdateDbMatchingCommand ToUpdateCommand(DbMatchingDto dto);
    //////public static partial AddEditDbMatchingCommand CloneFromDto(DbMatchingDto dto);
    ////public static partial void ApplyChangesFrom(UpdateDbMatchingCommand source, DbMatching target);
    //public static partial void ApplyChangesFrom(AddEditDbMatchingCommand source, DbMatching target);
    public static partial IQueryable<DbMatchingDto> ProjectTo(this IQueryable<DbMatching> q);
}

