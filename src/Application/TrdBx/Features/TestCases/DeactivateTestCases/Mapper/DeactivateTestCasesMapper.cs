
using CleanArchitecture.Blazor.Application.Features.TestCases.DeactivateTestCases.DTOs;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.DeactivateTrackingUnit;

namespace CleanArchitecture.Blazor.Application.Features.TestCases.DeactivateTestCases.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class Mapper
{
    public static partial DeactivateTestCaseDto ToDto(DeactivateTestCase source);
    public static partial DeactivateTestCase FromDto(DeactivateTestCaseDto dto);

    public static partial DeactivateTrackingUnitCommand ToExecuteCommand(DeactivateTestCase source);
    public static partial IQueryable<DeactivateTestCaseDto> ProjectTo(this IQueryable<DeactivateTestCase> q);
}

