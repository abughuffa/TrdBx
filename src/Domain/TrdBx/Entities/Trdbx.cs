using System.ComponentModel;
using CleanArchitecture.Blazor.Domain.Common.Entities;
using CleanArchitecture.Blazor.Domain.Enums;
using CleanArchitecture.Blazor.Domain.Identity;

namespace CleanArchitecture.Blazor.Domain.Entities;

#region TrackingUnit
public class TrackingUnitModel : BaseEntity
{
    
    public required string WialonName { get; set; } = string.Empty;
    public required string Name { get; set; } = string.Empty;
    public required int WhwTypeId { get; set; } = 0;
    public required decimal DefualtHost { get; set; } = 0.0m;
    public required decimal DefualtGprs { get; set; } = 0.0m;
    public required decimal DefualtPrice { get; set; } = 0.0m;
    public List<TrackingUnit>? TrackingUnits { get; set; } = null;
    public List<CusPrice>? CusPrices { get; set; } = null;
    public int PortNo1 { get; set; } = 0;
    public int PortNo2 { get; set; } = 0;
    public int? OldId { get; set; } = null;
    
}
public class TrackingUnit : BaseAuditableEntity
{
    public required string SNo { get; set; } = string.Empty;
    public string? Imei { get; set; } = string.Empty;
    public string? UnitName { get; set; } = string.Empty;
    public int TrackingUnitModelId { get; set; }
    public UStatus UStatus { get; set; } = UStatus.New;
    public InsMode InsMode { get; set; } = InsMode.Null;
    public DateOnly? WryDate { get; set; } = null;
    public int? TrackedAssetId { get; set; } = null;
    public int? SimCardId { get; set; } = null;
    public int? CustomerId { get; set; } = null;
    public bool IsOnWialon { get; set; } = false;
    public WStatus? WStatus { get; set; }
    public int? WUnitId { get; set; }
    public int? OldId { get; set; } 
    public TrackingUnitModel? TrackingUnitModel { get; set; } = null;
    public SimCard? SimCard { get; set; } = null;
    public Customer? Customer { get; set; } = null;
    public TrackedAsset? TrackedAsset { get; set; } = null;
    public List<Subscription>? Subscriptions { get; set; } = null;
    public List<WialonTask>? WialonTasks { get; set; } = null;
}
#endregion

#region Cc
public class Customer : BaseAuditableEntity
{
    public int? ParentId { get; set; } = null;
    public string Name { get; set; } = string.Empty;
    public string Account { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public BillingPlan BillingPlan { get; set; }
    public bool IsTaxable { get; set; } = false;
    public bool IsRenewable { get; set; } = false;
    public int? WUserId { get; set; } = null;
    public int? WUnitGroupId { get; set; } = null;
    public string? Address { get; set; } = string.Empty;
    public string? Mobile1 { get; set; } = string.Empty;
    public string? Mobile2 { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public bool IsAvaliable { get; set; } = true;
    public int? OldId { get; set; } = null;

    public Customer? Parent { get; set; } = null;
    public List<Customer>? Childs { get; set; } = null;
}
public class CusPrice : BaseAuditableEntity
{
    //public AssignedTo AssignedTo { get; set; } = AssignedTo.Null;
    public int CustomerId { get; set; }
    public int TrackingUnitModelId { get; set; }
    public decimal Host { get; set; } = 0.0m;
    public decimal Gprs { get; set; } = 0.0m;
    public decimal Price { get; set; } = 0.0m;
    public Customer? Customer { get; set; } = null;
    public TrackingUnitModel? TrackingUnitModel { get; set; } = null;
}
#endregion

#region SIM
public class SProvider : BaseEntity
{
    public required string Name { get; set; } = string.Empty;
    public List<SPackage>? SPackages { get; set; } = null;
}
public class SPackage : BaseEntity
{
    public required string Name { get; set; } = string.Empty;
    public int SProviderId { get; set; }
    public SProvider? SProvider { get; set; } = null;
    public int? OldId { get; set; } = null;
    public List<SimCard>? SimCards { get; set; } = null;
}
public class SimCard : BaseAuditableEntity
{
    public required string SimCardNo { get; set; } = string.Empty;
    public string? ICCID { get; set; } = string.Empty;
    public int SPackageId { get; set; }
    public SPackage? SPackage { get; set; } = null;
    public SStatus SStatus { get; set; } = SStatus.New;
    //public DateOnly? JoDate { get; set; } = null;
    //public DateOnly? BExDate { get; set; } = null;
    //public DateOnly? DExDate { get; set; } = null;
    //public DateOnly? DOExDate { get; set; } = null;
    public bool IsOwen { get; set; } = true;
    public DateOnly? ExDate { get; set; } = null;


    public int? OldId { get; set; } = null;
    public TrackingUnit? TrackingUnits { get; set; } = null;
}
#endregion

#region TrackedAsset
public class TrackedAsset : BaseAuditableEntity
{
    public string TrackedAssetNo { get; set; } = string.Empty;
    public string? TrackedAssetCode { get; set; } = string.Empty;
    public string? VinSerNo { get; set; } = string.Empty;
    public string? PlateNo { get; set; } = string.Empty;
    public required string TrackedAssetDesc { get; set; } = string.Empty;
    public bool IsAvaliable { get; set; } = true;
    public int? OldId { get; set; } = null;
    public string? OldVehicleNo { get; set; } = null;
    public List<TrackingUnit>? TrackingUnits { get; set; } = null;
}
#endregion

#region Servcies
public class ServiceLog : BaseAuditableEntity
{
    public string ServiceNo { get; set; } = string.Empty;
    public ServiceTask ServiceTask { get; set; }
    public int CustomerId { get; set; }
    //public required string InstallerId { get; set; }
    public string Desc { get; set; } = string.Empty;
    public DateOnly SerDate { get; set; }
    public bool IsDeserved { get; set; } = true;
    public bool IsBilled { get; set; } = false;
    public decimal Amount { get; set; } = 0.0m;
    public Customer? Customer { get; set; } = null;
    //public ApplicationUser? Installer { get; set; } = null;
    public InvoiceItem? InvoiceItem { get; set; } = null;
    public List<Subscription>? Subscriptions { get; set; } = null;
    public List<WialonTask>? WialonTasks { get; set; } = null;
    public virtual ApplicationUser? CreatedByUser { get; set; }
}
public class ServicePrice : BaseAuditableEntity
{
    public ServiceTask ServiceTask { get; set; }
    public string Desc { get; set; } = string.Empty;
    public decimal Price { get; set; } = 0.0m;
}
public class Subscription : BaseEntity
{
    public int ServiceLogId { get; set; }
    public int TrackingUnitId { get; set; }
    public int CaseCode { get; set; }
    public SubPackageFees LastPaidFees { get; set; }
    public string Desc { get; set; } = string.Empty;
    public DateOnly SsDate { get; set; }
    public DateOnly SeDate { get; set; }
    public int Days { get; set; }
    public decimal DailyFees { get; set; } = 0.0m;
    public decimal Amount { get; set; }
    public ServiceLog? ServiceLog { get; set; } = null;
    public TrackingUnit? TrackingUnit { get; set; } = null;
}
public class WialonTask : BaseEntity
{
    public int ServiceLogId { get; set; }
    public int TrackingUnitId { get; set; }
    public string Desc { get; set; } = string.Empty;
    public APITask? APITask { get; set; }
    public DateOnly ExcDate { get; set; }
    public bool IsExecuted { get; set; } = false;
    public ServiceLog? ServiceLog { get; set; } = null;
    public TrackingUnit? TrackingUnit { get; set; } = null;
}
#endregion

#region Invoices
public class Invoice : BaseAuditableEntity
{
    public string InvNo { get; set; } = string.Empty;
    public DateOnly InvDate { get; set; }
    public DateOnly DueDate { get; set; }
    public InvoiceType InvoiceType { get; set; }
    public IStatus IStatus { get; set; }
    public int CustomerId { get; set; }
    public string InvDesc { get; set; } = string.Empty;
    public decimal Total { get; set; } = 0.0m;
    public decimal Taxes { get; set; } = 0.0m;
    public decimal GrangTotal { get; set; } = 0.0m;
    public bool IsTaxable { get; set; } = false;
    public Customer? Customer { get; set; }
    public List<InvoiceItem>? InvoiceItems { get; set; } = null;
}
public class InvoiceItem : BaseEntity
{
    public int InvoiceId { get; set; }
    public int ServiceLogId { get; set; }
    public decimal Amount { get; set; } = 0.0m;
    public Invoice? Invoice { get; set; }
    public ServiceLog? ServiceLog { get; set; }
}
#endregion

#region Invoices
//public class Invoice : BaseAuditableEntity
//{
//    public string InvNo { get; set; } = string.Empty;
//    public DateOnly InvDate { get; set; }
//    public DateOnly DueDate { get; set; }
//    public InvoiceType InvoiceType { get; set; }
//    public IStatus IStatus { get; set; }
//    public int CustomerId { get; set; }
//    public string InvDesc { get; set; } = string.Empty;
//    public decimal Total { get; set; } = 0.0m;
//    public decimal Taxes { get; set; } = 0.0m;
//    public decimal GrangTotal { get; set; } = 0.0m;
//    public bool IsTaxable { get; set; } = false;
//    public Customer? Customer { get; set; }
//    public List<ItemGroup>? ItemGroups { get; set; } = null;
//}
//public class ItemGroup : BaseEntity
//{
//    public int InvoiceId { get; set; }
//    public int ServiceLogId { get; set; }
//    public decimal Amount { get; set; } = 0.0m;
//    public Invoice? Invoice { get; set; }
//    public ServiceLog? ServiceLog { get; set; }
//    public List<InvoiceItem>? InvoiceItems { get; set; } = null;
//}
#endregion
//public class InvoiceItem : BaseEntity
//{
//    public int ItemGroupId { get; set; }
//    public int Serial { get; set; }
//    public int SubSerial { get; set; }
//    public string? Desc { get; set; }
//    public decimal SubTotal { get; set; } = 0.0m;
//    public decimal ItemTotal { get; set; } = 0.0m;
//    public ItemGroup? ItemGroup { get; set; }
//}



#region Tickets
public enum TicketStatus
{
    All = 0,
    JustCreated = 1,
    Approved = 2,
    Assigned = 3,
    Released = 4,
    Rejected = 5,
    OnProcess = 6,
    Completed = 7
}
public class Ticket : BaseAuditableEntity
{
    public string TicketNo { get; set; } = string.Empty;
    public ServiceTask ServiceTask { get; set; }
    public string Desc { get; set; } = string.Empty;
    public TicketStatus TicketStatus { get; set; }
    public int TrackingUnitId { get; set; }
    public DateOnly TcDate { get; set; }
    public DateOnly? TaDate { get; set; }
    //public string? InstallerId { get; set; }
    public DateOnly? TeDate { get; set; }
    public string? Note { get; set; } = string.Empty;
    public TrackingUnit? TrackingUnit { get; set; }
    public virtual ApplicationUser? CreatedByUser { get; set; }
    public virtual ApplicationUser? LastModifiedByUser { get; set; }
}
#endregion


#region Database Diagnostic
public class LibyanaSimCard : BaseEntity
{
    public string? SimCardNo { get; set; } = string.Empty;
    //public string? SimCardStatus { get; set; } = string.Empty;
    public SLStatus? SimCardStatus { get; set; }
    public decimal? Balance { get; set; } = 0.0m;
    public DateTime? BExDate { get; set; } = null;
    public DateTime? JoinDate { get; set; } = null;
    public string? Package { get; set; } = string.Empty;
    public DateTime? DExDate { get; set; } = null;
    public string? DataOffer { get; set; } = string.Empty;
    public DateTime? DOExpired { get; set; } = null;

}
public class WialonUnit : BaseEntity
{
    public string? UnitName { get; set; } = string.Empty;
    public string? Account { get; set; } = string.Empty;
    public string? UnitSNo { get; set; } = string.Empty;
    public DateTime? Deactivation { get; set; } = null;
    public string? SimCardNo { get; set; } = string.Empty;
    public WStatus? StatusOnWialon { get; set; } 
    public string? Note { get; set; } = string.Empty;
}
public class DataMatch : IEntity
{
    public string? Account { get; set; }
    public string? Client { get; set; }
    public string? Customer { get; set; }
    public string? WUnitSNo { get; set; }
    public string? TUnitSNo { get; set; }
    public string? WSimCardNo { get; set; }
    public string? TSimCardNo { get; set; }
    public WStatus? StatusOnWialon { get; set; }
    public UStatus? StatusOnTrdBx { get; set; }
    public string? TNote { get; set; }
    public string? WNote { get; set; }
}
public class DataDiagnosis : IEntity
{
    [Description("Account")]
    public string? Account { get; set; }
    [Description("Client")]
    public string? Client { get; set; }
    [Description("Customer")]
    public string? Customer { get; set; }

    [Description("UnitSNo")]
    public string? UnitSNo { get; set; }

    [Description("SimCardNo")]
    public string? SimCardNo { get; set; }

    [Description("StatusOnWialon")]
    public WStatus? StatusOnWialon { get; set; }

    [Description("StatusOnTrdBx")]
    public UStatus? StatusOnTrdBx { get; set; }

    [Description("SimCardStatus")]
    public SLStatus? SimCardStatus { get; set; }
    [Description("LDExDate")]
    public DateTime? LDExDate { get; set; }
    [Description("LDOExpired")]
    public DateTime? LDOExpired { get; set; }

    [Description("WNote")]
    public string? WNote { get; set; }
    [Description("Balance")]
    public decimal? Balance { get; set; }

}
#endregion


