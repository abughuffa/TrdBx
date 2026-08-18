using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.TrdBxData.DTOs;

[Description("DataCount")]
public class DataCountDto
{
    [Display(Name = "SProviders")]
    public int SProviders { get; set; } = 0;
    [Display(Name = "SPackages")]
    public int SPackages { get; set; } = 0;
    [Display(Name = "SimCards")]
    public int SimCards { get; set; } = 0;
    [Display(Name = "TrackingUnitModels")]
    public int TrackingUnitModels { get; set; } = 0;
    [Display(Name = "TrackingUnits")]
    public int TrackingUnits { get; set; } = 0;
    [Display(Name = "TrackedAssets")]
    public int TrackedAssets { get; set; } = 0;
    [Display(Name = "ParentCustomer")]
    public int ParentCustomer { get; set; } = 0;
    [Display(Name = "ChildCustomers")]
    public int ChildCustomers { get; set; } = 0;
    [Display(Name = "ServiceLogs")]
    public int ServiceLogs { get; set; } = 0;
    [Display(Name = "Subscriptions")]
    public int Subscriptions { get; set; } = 0;
    [Display(Name = "WialonTasks")]
    public int WialonTasks { get; set; } = 0;


    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<InvoiceSummary, InvoiceSummaryDto>(MemberList.None);
    //        CreateMap<InvoiceSummaryDto, InvoiceSummary>(MemberList.None);
    //    }
    //}


}

