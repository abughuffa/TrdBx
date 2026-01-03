using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.DTOs;

[Description("DataCount")]
public class DataCountDto
{
    [Description("SProviders")]
    public int SProviders { get; set; } = 0;
    [Description("SPackages")]
    public int SPackages { get; set; } = 0;
    [Description("SimCards")]
    public int SimCards { get; set; } = 0;
    [Description("TrackingUnitModels")]
    public int TrackingUnitModels { get; set; } = 0;
    [Description("TrackingUnits")]
    public int TrackingUnits { get; set; } = 0;
    [Description("TrackedAssets")]
    public int TrackedAssets { get; set; } = 0;
    [Description("ParentCustomer")]
    public int ParentCustomer { get; set; } = 0;
    [Description("ChildCustomers")]
    public int ChildCustomers { get; set; } = 0;
    [Description("ServiceLogs")]
    public int ServiceLogs { get; set; } = 0;
    [Description("Subscriptions")]
    public int Subscriptions { get; set; } = 0;
    [Description("WialonTasks")]
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

