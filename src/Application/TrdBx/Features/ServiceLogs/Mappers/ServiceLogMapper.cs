
//using CleanArchitecture.Blazor.Application.Features.ServiceLogs.Commands.Create;
using CleanArchitecture.Blazor.Application.Features.ServiceLogs.Commands.Update;
using CleanArchitecture.Blazor.Application.Features.ServiceLogs.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.ServiceLogs.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class Mapper
{
    public static partial ServiceLogDto ToDto(ServiceLog source);
    public static partial ServiceLog FromDto(ServiceLogDto dto);
    //public static partial CusPrice FromEditCommand(AddEditCusPriceCommand command);
    //public static partial ServiceLog FromCreateCommand(CreateCusPriceCommand command);
    public static partial UpdateServiceLogCommand ToUpdateCommand(ServiceLogDto dto);
    //public static partial AddEditCusPriceCommand CloneFromDto(CusPriceDto dto);
    public static partial void ApplyChangesFrom(UpdateServiceLogCommand source, ServiceLog target);
    //public static partial void ApplyChangesFrom(AddEditCusPriceCommand source, CusPrice target);
    public static partial IQueryable<ServiceLogDto> ProjectTo(this IQueryable<ServiceLog> q);
}

