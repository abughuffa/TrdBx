
using CleanArchitecture.Blazor.Application.Features.TestCases.ActivateHostingTestCases.DTOs;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.ActivateTrackingUnitForHosting;

namespace CleanArchitecture.Blazor.Application.Features.TestCases.ActivateHostingTestCases.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class Mapper
{
    public static partial ActivateHostingTestCaseDto ToDto(ActivateHostingTestCase source);
    public static partial ActivateHostingTestCase FromDto(ActivateHostingTestCaseDto dto);

    public static partial ActivateTrackingUnitForHostingCommand ToExecuteCommand(ActivateHostingTestCase source);
    public static partial IQueryable<ActivateHostingTestCaseDto> ProjectTo(this IQueryable<ActivateHostingTestCase> q);
}

