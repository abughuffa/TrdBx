using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.Summaries.SimCards.DTOs;

[Description("SimCardSummary")]
public class SimCardSummaryDto
{
    [Description("News")]
    public int News { get; set; }
    [Description("Installeds")]
    public int Installeds { get; set; }
    [Description("Recovereds")]
    public int Recovereds { get; set; }
    [Description("Useds")]
    public int Useds { get; set; }
    [Description("Losts")]
    public int Losts { get; set; }

    [Description("Counts")]
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

