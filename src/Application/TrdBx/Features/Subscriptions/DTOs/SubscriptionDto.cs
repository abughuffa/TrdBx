using CleanArchitecture.Blazor.Application.Features.ServiceLogs.DTOs;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.Subscriptions.DTOs;

[Description("Subscriptions")]
public class SubscriptionDto
{
    [Description("Id")]
    public int Id { get; set; }
    [Description("ServiceLogId")]
    public int ServiceLogId { get; set; }
    [Description("TrackingUnitId")]
    public int TrackingUnitId { get; set; }
    [Description("CaseCode")]
    public int CaseCode { get; set; }
    [Description("LastPaidFees")]
    public SubPackageFees LastPaidFees { get; set; }

    [Description("Desc")]
    public string Desc { get; set; } = string.Empty;

    [Description("SsDate")]
    public DateOnly SsDate { get; set; }
    [Description("SeDate")]
    public DateOnly SeDate { get; set; }
    //[Description("IsBilled")]
    //public bool IsBilled { get; set; }
    [Description("DailyFees")]
    public decimal DailyFees { get; set; }

    [Description("Days")]
    public int Days { get; set; }

    [Description("Amount")]

    public decimal Amount { get; set; }

    //public ServiceLogDto? ServiceLog { get; set; }


    [Description("ServiceLog")] public string? ServiceLog { get; set; }
    [Description("TrackingUnit")] public string? TrackingUnit { get; set; }

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

