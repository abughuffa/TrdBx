
using CleanArchitecture.Blazor.Application.Features.TestCases.ActivateTestCases.DTOs;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.ActivateTrackingUnit;

namespace CleanArchitecture.Blazor.Application.Features.TestCases.ActivateTestCases.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class Mapper
{
    public static partial ActivateTestCaseDto ToDto(ActivateTestCase source);
    public static partial ActivateTestCase FromDto(ActivateTestCaseDto dto);
    public static partial ActivateTrackingUnitCommand ToExecuteCommand(ActivateTestCase source);
    public static partial IQueryable<ActivateTestCaseDto> ProjectTo(this IQueryable<ActivateTestCase> q);
}

