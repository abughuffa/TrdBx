
using CleanArchitecture.Blazor.Application.Features.SimCards.Commands.AddEdit;
using CleanArchitecture.Blazor.Application.Features.SimCards.Commands.Create;
using CleanArchitecture.Blazor.Application.Features.SimCards.Commands.Update;
using CleanArchitecture.Blazor.Application.Features.SimCards.DTOs;
using CleanArchitecture.Blazor.Application.Features.WialonTasks.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.SimCards.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class Mapper
{
    public static partial SimCardDto ToDto(SimCard source);

    [MapperIgnoreSource(nameof(SimCardDto.SPackage))]
    public static partial SimCard FromDto(SimCardDto dto);
    public static partial SimCard FromEditCommand(AddEditSimCardCommand command);
    public static partial SimCard FromCreateCommand(CreateSimCardCommand command);
    public static partial UpdateSimCardCommand ToUpdateCommand(SimCardDto dto);
    public static partial AddEditSimCardCommand CloneFromDto(SimCardDto dto);
    public static partial void ApplyChangesFrom(UpdateSimCardCommand source, SimCard target);
    public static partial void ApplyChangesFrom(AddEditSimCardCommand source, SimCard target);
    public static partial IQueryable<SimCardDto> ProjectTo(this IQueryable<SimCard> q);
}

