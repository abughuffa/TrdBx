// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.TestCases.ActivateGprsTestCases.DTOs;

[Description("AGTestCases")]
public class ActivateGprsTestCaseDto
{
    [Description("Id")]
    public int Id { get; set; }
    [Description("TrackingUnitId")]
    public int? TrackingUnitId { get; set; }

    [Description("InstallerId")]
    public  string InstallerId { get; set; } = string.Empty;

    [Description("SNo")]
    public string? SNo { get; set; }
    [Description("TsDate")]
    public DateOnly TsDate { get; set; }

    [Description("CaseCode")]
    public int? CaseCode { get; set; }

    [Description("IsSucssed")]
    public bool? IsSucssed { get; set; }
    [Description("Message")]
    public string? Message { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<ActivateGprsTestCase, ActivateGprsTestCaseDto>(MemberList.None);
            CreateMap<ActivateGprsTestCaseDto, ActivateGprsTestCase>(MemberList.None);
        }
    }

}

