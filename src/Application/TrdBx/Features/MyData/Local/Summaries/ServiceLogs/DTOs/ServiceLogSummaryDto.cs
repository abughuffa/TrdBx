using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.Summaries.ServiceLogs.DTOs;

[Description("ServiceLogSummary")]
public class ServiceLogSummaryDto
{
    [Description("Checks")]
    public int Checks { get; set; }
    [Description("Installs")]
    public int Installs { get; set; }
    [Description("Replaces")]
    public int Replaces { get; set; }
    [Description("Support")]
    public int Supports { get; set; }
    [Description("Subscriptions")]
    public int Subscriptions { get; set; }
    [Description("Renews")]
    public int Renews { get; set; }

    [Description("Counts")]
    public int Counts { get; set; }

    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<ServiceLogSummary, ServiceLogSummaryDto>(MemberList.None);
    //        CreateMap<ServiceLogSummaryDto, ServiceLogSummary>(MemberList.None);
    //    }
    //}


}

