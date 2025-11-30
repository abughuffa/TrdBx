
using CleanArchitecture.Blazor.Application.Features.SPackages.Commands.AddEdit;
//using CleanArchitecture.Blazor.Application.Features.SPackages.Commands.Create;
//using CleanArchitecture.Blazor.Application.Features.SPackages.Commands.Update;
using CleanArchitecture.Blazor.Application.Features.SPackages.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.SPackages.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class Mapper
{
    public static partial SPackageDto ToDto(SPackage source);
    public static partial SPackage FromDto(SPackageDto dto);
    public static partial SPackage FromEditCommand(AddEditSPackageCommand command);
    public static partial AddEditSPackageCommand ToEditCommand(SPackageDto dto);
    //public static partial SPackage FromCreateCommand(CreateSPackageCommand command);
    //public static partial UpdateSPackageCommand ToUpdateCommand(SPackageDto dto);
    public static partial AddEditSPackageCommand CloneFromDto(SPackageDto dto);
    //public static partial void ApplyChangesFrom(UpdateSPackageCommand source, SPackage target);
    public static partial void ApplyChangesFrom(AddEditSPackageCommand source, SPackage target);
    public static partial IQueryable<SPackageDto> ProjectTo(this IQueryable<SPackage> q);
}

