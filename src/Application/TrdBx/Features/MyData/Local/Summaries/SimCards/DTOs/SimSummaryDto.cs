using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.Summaries.SimCards.DTOs;

[Description("SimCardSummary")]
public class SimCardSummaryDto
{
  [Display(Name = "News")]
    public int News { get; set; }
    [Display(Name = "Installeds")]
    public int Installeds { get; set; }
    [Display(Name = "Recovereds")]
    public int Recovereds { get; set; }
    [Display(Name = "Useds")]
    public int Useds { get; set; }
    [Display(Name = "Losts")]
    public int Losts { get; set; }

    [Display(Name = "Counts")]
    public int Counts { get; set; }

    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<SimCardSummary, SimCardSummaryDto>(MemberList.None);
    //        CreateMap<SimCardSummaryDto, SimCardSummary>(MemberList.None);
    //    }
    //}

}

