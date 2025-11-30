namespace CleanArchitecture.Blazor.Application.Features.TestCases.ActivateTestCases.DTOs;

[Description("ATestCases")]
public class ActivateTestCaseDto
{
    [Description("Id")]
    public int Id { get; set; }
    [Description("TrackingUnitId")]
    public int? TrackingUnitId { get; set; }
    [Description("InstallerId")]
    public string InstallerId { get; set; } = string.Empty;
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

    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<ActivateTestCase, ActivateTestCaseDto>(MemberList.None);
    //        CreateMap<ActivateTestCaseDto, ActivateTestCase>(MemberList.None);
    //    }
    //}

}

