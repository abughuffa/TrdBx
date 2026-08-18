using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.Summaries.TrackingUnits.DTOs;

[Description("TrackingUnitSummary")]
public class TrackingUnitSummaryDto
{
    [Display(Name = "News")]
    public int News { get; set; } = 0;
    [Display(Name = "Reserveds")]
    public int Reserveds { get; set; } = 0;
    [Display(Name = "InstalledActiveGprss")]
    public int InstalledActiveGprss { get; set; } = 0;
    [Display(Name = "InstalledActiveHostings")]
    public int InstalledActiveHostings { get; set; } = 0;
    [Display(Name = "InstalledActives")]
    public int InstalledActives { get; set; } = 0;
    [Display(Name = "InstalledInactives")]
    public int InstalledInactives { get; set; } = 0;
    [Display(Name = "Recovereds")]
    public int Recovereds { get; set; } = 0;
    [Display(Name = "Useds")]
    public int Useds { get; set; } = 0;
    [Display(Name = "Damageds")]
    public int Damageds { get; set; } = 0;
    [Display(Name = "Losts")]
    public int Losts { get; set; } = 0;

    [Display(Name = "Counts")]
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

