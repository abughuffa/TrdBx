using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Blazor.Application.Features.ServiceLogs.DTOs;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.Subscriptions.DTOs;

[Description("Subscriptions")]
public class SubscriptionDto
{
    [Display(Name = "Id")]
    public int Id { get; set; }
    [Display(Name = "ServiceLogId")]
    public int ServiceLogId { get; set; }
    [Display(Name = "TrackingUnitId")]
    public int TrackingUnitId { get; set; }
    [Display(Name = "CaseCode")]
    public int CaseCode { get; set; }
    [Display(Name = "LastPaidFees")]
    public SubPackageFees LastPaidFees { get; set; }

    [Display(Name = "Description")]
    public string Description { get; set; } = string.Empty;

    [Display(Name = "SsDate")]
    public DateOnly SsDate { get; set; }
    [Display(Name = "SeDate")]
    public DateOnly SeDate { get; set; }
    //[Display(Name = "IsBilled")]
    //public bool IsBilled { get; set; }
    [Display(Name = "DailyFees")]
    public decimal DailyFees { get; set; }

    [Display(Name = "Days")]
    public int Days { get; set; }

    [Display(Name = "Amount")]

    public decimal Amount { get; set; }

    //public ServiceLogDto? ServiceLog { get; set; }


    [Display(Name = "ServiceLog")] public string? ServiceLog { get; set; }
    [Display(Name = "TrackingUnit")] public string? TrackingUnit { get; set; }

    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<Subscription, SubscriptionDto>(MemberList.None)
    //        .ForMember(dest => dest.ServiceLog,
    //                  opt => opt.MapFrom(src => (src.ServiceLog == null ? null : src.ServiceLog.ServiceNo)))
    //            .ForMember(dest => dest.TrackingUnit,
    //                  opt => opt.MapFrom(src => (src.TrackingUnit == null ? null : src.TrackingUnit.SNo)));

    //        CreateMap<SubscriptionDto, Subscription>(MemberList.None);
    //    }
    //}

}

