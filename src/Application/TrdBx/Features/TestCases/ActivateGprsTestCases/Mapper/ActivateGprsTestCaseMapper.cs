using CleanArchitecture.Blazor.Application.Features.TestCases.ActivateGprsTestCases.DTOs;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.ActivateTrackingUnitForGprs;

namespace CleanArchitecture.Blazor.Application.Features.TestCases.ActivateGprsTestCases.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class Mapper
{
    public static partial ActivateGprsTestCaseDto ToDto(ActivateGprsTestCase source);
    public static partial ActivateGprsTestCase FromDto(ActivateGprsTestCaseDto dto);

    public static partial ActivateTrackingUnitForGprsCommand ToExecuteCommand(ActivateGprsTestCase source);
    
    public static partial IQueryable<ActivateGprsTestCaseDto> ProjectTo(this IQueryable<ActivateGprsTestCase> q);
}

