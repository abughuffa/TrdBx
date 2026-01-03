
using CleanArchitecture.Blazor.Application.Features.SProviders.Commands.AddEdit;
//using CleanArchitecture.Blazor.Application.Features.SProviders.Commands.Create;
//using CleanArchitecture.Blazor.Application.Features.SProviders.Commands.Update;
using CleanArchitecture.Blazor.Application.Features.SProviders.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.SProviders.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class Mapper
{
    public static partial SProviderDto ToDto(SProvider source);
    public static partial SProvider FromDto(SProviderDto dto);
    public static partial SProvider FromEditCommand(AddEditSProviderCommand command);
    public static partial AddEditSProviderCommand ToEditCommand(SProviderDto dto);
    //public static partial SProvider FromCreateCommand(CreateSProviderCommand command);
    //public static partial UpdateSProviderCommand ToUpdateCommand(SProviderDto dto);
    public static partial AddEditSProviderCommand CloneFromDto(SProviderDto dto);
    //public static partial void ApplyChangesFrom(UpdateSProviderCommand source, SProvider target);
    public static partial void ApplyChangesFrom(AddEditSProviderCommand source, SProvider target);
    public static partial IQueryable<SProviderDto> ProjectTo(this IQueryable<SProvider> q);
}

