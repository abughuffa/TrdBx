using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.Summaries.TrackingUnits.DTOs;

[Description("TrackingUnitSummary")]
public class TrackingUnitSummaryDto
{
    [Description("News")]
    public int News { get; set; } = 0;
    [Description("Reserveds")]
    public int Reserveds { get; set; } = 0;
    [Description("InstalledActiveGprss")]
    public int InstalledActiveGprss { get; set; } = 0;
    [Description("InstalledActiveHostings")]
    public int InstalledActiveHostings { get; set; } = 0;
    [Description("InstalledActives")]
    public int InstalledActives { get; set; } = 0;
    [Description("InstalledInactives")]
    public int InstalledInactives { get; set; } = 0;
    [Description("Recovereds")]
    public int Recovereds { get; set; } = 0;
    [Description("Useds")]
    public int Useds { get; set; } = 0;
    [Description("Damageds")]
    public int Damageds { get; set; } = 0;
    [Description("Losts")]
    public int Losts { get; set; } = 0;

    [Description("Counts")]
    public int Counts { get; set; } = 0;

    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<TrackingUnitSummary, TrackingUnitSummaryDto>(MemberList.None);
    //        CreateMap<TrackingUnitSummaryDto, TrackingUnitSummary>(MemberList.None);
    //    }
    //}

}

