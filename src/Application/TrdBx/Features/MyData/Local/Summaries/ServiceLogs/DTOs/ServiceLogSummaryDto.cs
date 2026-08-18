using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.Summaries.ServiceLogs.DTOs;

[Description("ServiceLogSummary")]
public class ServiceLogSummaryDto
{
   [Display(Name = "Checks")]
    public int Checks { get; set; }
    [Display(Name = "Installs")]
    public int Installs { get; set; }
    [Display(Name = "Replaces")]
    public int Replaces { get; set; }
    [Display(Name = "Support")]
    public int Supports { get; set; }
    [Display(Name = "Subscriptions")]
    public int Subscriptions { get; set; }
    [Display(Name = "Renews")]
    public int Renews { get; set; }

    [Display(Name = "Counts")]
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

