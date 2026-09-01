using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Blazor.Domain.Common.Entities;
using CleanArchitecture.Blazor.Domain.Enums;
using CleanArchitecture.Blazor.Domain.Identity;

namespace CleanArchitecture.Blazor.Domain.Entities;

#region TrackingUnit
public class TrackingUnitModel : BaseEntity
{
    public string WialonName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int WhwTypeId { get; set; } = 0;
    public decimal DefaultHost { get; set; } = 0.0m;
    public decimal DefaultGprs { get; set; } = 0.0m;
    public decimal DefaultPrice { get; set; } = 0.0m;
    public int PortNo1 { get; set; } = 0;
    public int PortNo2 { get; set; } = 0;
    public int? OldId { get; set; } = null;
    
    // Navigation Properties
    public  List<TrackingUnit> TrackingUnits { get; set; } = null;
    public  List<CusPrice> CusPrices { get; set; } = null;
}




public class TrackingUnit : BaseAuditableEntity
{
    public string SNo { get; set; } = string.Empty;
    public string? Imei { get; set; }
    public string? UnitName { get; set; }
    public int TrackingUnitModelId { get; set; }
    public UStatus UStatus { get; set; } = UStatus.New;
    public InsMode InsMode { get; set; } = InsMode.Null;
    public DateOnly? WryDate { get; set; }
    public int? TrackedAssetId { get; set; }
    public int? SimCardId { get; set; }
    public int? CustomerId { get; set; }
    public bool IsOnWialon { get; set; } = false;
    public WStatus? WStatus { get; set; }
    public int? WUnitId { get; set; }
    public int? OldId { get; set; }
    
    // Navigation Properties
    public  TrackingUnitModel? TrackingUnitModel { get; set; }
    public  SimCard? SimCard { get; set; }
    public  Customer? Customer { get; set; }
    public  TrackedAsset? TrackedAsset { get; set; }
    public  List<Subscription> Subscriptions { get; set; } = null;
    public  List<WialonTask> WialonTasks { get; set; } = null;
    public  List<Ticket> Tickets { get; set; } = null;
}

#endregion

#region Cc

public class Customer : BaseAuditableEntity
{
    public int? ParentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Account { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public BillingPlan BillingPlan { get; set; }
    public bool IsTaxable { get; set; } = false;
    public bool IsRenewable { get; set; } = false;
    public int? WUserId { get; set; }
    public int? WUnitGroupId { get; set; }
    public string? Address { get; set; }
    public string? Mobile1 { get; set; }
    public string? Mobile2 { get; set; }
    public string? Email { get; set; }
    public bool IsAvailable { get; set; } = true;
    public int? OldId { get; set; }
    
    // Navigation Properties
    public  Customer? Parent { get; set; }
    public  List<Customer> Childs { get; set; } = null;
    public  List<Invoice> Invoices { get; set; } = null;
    public  List<CusPrice> CusPrices { get; set; } = null;
    public  List<ServiceLog> ServiceLogs { get; set; } = null;
    public  List<TrackingUnit> TrackingUnits { get; set; } = null;
}

public class CusPrice : BaseAuditableEntity
{
    public int CustomerId { get; set; }
    public int TrackingUnitModelId { get; set; }
    public decimal Host { get; set; } = 0.0m;
    public decimal Gprs { get; set; } = 0.0m;
    public decimal Price { get; set; } = 0.0m;
    
    // Navigation Properties
    public  Customer? Customer { get; set; } =null;
    public  TrackingUnitModel? TrackingUnitModel { get; set; } =null;
}

#endregion

#region SIM

public class SProvider : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    
    // Navigation Properties
    public  List<SPackage> SPackages { get; set; } = null;
}



public class SPackage : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public int SProviderId { get; set; }
    public int? OldId { get; set; } = null;
    
    // Navigation Properties
    public  SProvider? SProvider { get; set; } =null;
    public  List<SimCard> SimCards { get; set; } = null;
}


public class SimCard : BaseAuditableEntity
{
    public string SimCardNo { get; set; } = string.Empty;
    public string? ICCID { get; set; }
    public int SPackageId { get; set; }
    public SStatus SStatus { get; set; } = SStatus.New;
    public bool IsOwned { get; set; } = true;
    public DateOnly? ExDate { get; set; }
    public int? OldId { get; set; }
    
    // Navigation Properties
    public  SPackage? SPackage { get; set; }=null;
    public  TrackingUnit? TrackingUnit { get; set; }=null;
}

#endregion

#region TrackedAsset

public class TrackedAsset : BaseAuditableEntity
{
    public string TrackedAssetNo { get; set; } = string.Empty;
    public string? TrackedAssetCode { get; set; }
    public string? VinSerNo { get; set; }
    public string? PlateNo { get; set; }
    public string TrackedAssetDesc { get; set; } = string.Empty;
    public bool IsAvailable { get; set; } = true;
    public int? OldId { get; set; }
    public string? OldVehicleNo { get; set; }
    
    // Navigation Properties
    public  List<TrackingUnit> TrackingUnits { get; set; } = new List<TrackingUnit>();
}
// public class TrackedAsset : BaseAuditableEntity
// {
//     public string TrackedAssetNo { get; set; } = string.Empty;
//     public string? TrackedAssetCode { get; set; } = string.Empty;
//     public string? VinSerNo { get; set; } = string.Empty;
//     public string? PlateNo { get; set; } = string.Empty;
//     public required string TrackedAssetDesc { get; set; } = string.Empty;
//     public bool IsAvailable { get; set; } = true;
//     public int? OldId { get; set; } = null;
//     public string? OldVehicleNo { get; set; } = null;
//     public List<TrackingUnit>? TrackingUnits { get; set; } = null;
// }
#endregion

#region Servcies
public class ServiceLog : BaseAuditableEntity
{
    public string ServiceNo { get; set; } = string.Empty;
    public ServiceTask ServiceTask { get; set; }
    public int CustomerId { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateOnly SerDate { get; set; }
    public bool IsDeserved { get; set; } = true;
    public bool IsBilled { get; set; } = false;
    public decimal Amount { get; set; } = 0.0m;
    
    // Navigation Properties
    public  Customer? Customer { get; set; } =null;
    public  InvoiceItemGroup? InvoiceItemGroup { get; set; } =null;
    public  ApplicationUser? CreatedByUser { get; set; }
    public  List<Subscription> Subscriptions { get; set; } = null;
    public  List<WialonTask> WialonTasks { get; set; } = null;

}



public class ServicePrice : BaseAuditableEntity
{
    public ServiceTask ServiceTask { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; } = 0.0m;
}


public class Subscription : BaseEntity
{
    public int ServiceLogId { get; set; }
    public int TrackingUnitId { get; set; }
    public int CaseCode { get; set; }
    public SubPackageFees LastPaidFees { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateOnly SsDate { get; set; }
    public DateOnly SeDate { get; set; }
    public decimal DailyFees { get; set; } = 0.0m;
    
    // Computed Properties - Not Mapped to Database
    public int Days => (int)(SeDate.ToDateTime(TimeOnly.MinValue) - SsDate.ToDateTime(TimeOnly.MinValue)).TotalDays;
    public decimal Amount => Math.Round(Days * DailyFees, 3, MidpointRounding.AwayFromZero);
    
    // Navigation Properties
    public  ServiceLog? ServiceLog { get; set; }=null;
    public  TrackingUnit? TrackingUnit { get; set; }=null;
    public  InvoiceItem? InvoiceItem { get; set; } =null;
}


public class WialonTask : BaseEntity
{
    public int ServiceLogId { get; set; }
    public int TrackingUnitId { get; set; }
    public string Description { get; set; } = string.Empty;
    public WialonAPIAction? WialonAPIAction { get; set; }
    public DateOnly ExcDate { get; set; }
    public bool IsExecuted { get; set; } = false;
    
    // Navigation Properties
    public  ServiceLog? ServiceLog { get; set; } =null;
    public  TrackingUnit? TrackingUnit { get; set; } =null;
}

#endregion



#region Invoices

public class Invoice : BaseAuditableEntity
{
    public string InvoiceNo { get; set; } = string.Empty;
    public DateOnly InvoiceDate { get; set; }
    public DateOnly DueDate { get; set; }
    public DateOnly? PaymentDate { get; set; }
    public decimal PaidAmount { get; set; } = 0.0m;
    public InvoiceType InvoiceType { get; set; }
    public IStatus IStatus { get; set; }
    public string DisplayCusName { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsTaxable { get; set; } = false;
    public bool IsTaxIgnored { get; set; } = true;
    public decimal Total { get; set; } = 0.0m;
    public decimal DiscountRate { get; set; } = 0.0m;
    public decimal DiscountAmount { get; set; } = 0.0m;
    public decimal TaxRate { get; set; } = 0.0m;
    public decimal TaxAmount { get; set; } = 0.0m;
    public decimal TaxableAmount { get; set; } = 0.0m;
    public decimal GrandTotal { get; set; } = 0.0m;
    
    // Navigation Properties
    public  Customer? Customer { get; set; } =null;
    public  List<InvoiceItemGroup> InvoiceItemGroups { get; set; } = null;
}

// InvoiceItemGroup.cs - Clean POCO
public class InvoiceItemGroup : BaseEntity
{
    public int SerialIndex { get; set; }
    public int InvoiceId { get; set; }
    public int ServiceLogId { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; } = 0.0m;
    public decimal SubTotal { get; set; } = 0.0m;
    
    // Navigation Properties
    public  Invoice? Invoice { get; set; } =null;
    public  ServiceLog? ServiceLog { get; set; }=null;
    public  List<InvoiceItem> InvoiceItems { get; set; } = null;
}

// InvoiceItem.cs - Clean POCO
public class InvoiceItem : BaseEntity
{
    public int SubSerialIndex { get; set; }
    public int InvoiceItemGroupId { get; set; }
    public int SubscriptionId { get; set; }
    public string? Description { get; set; }
    public decimal Amount { get; set; } = 0.0m;
    
    // Navigation Properties
    public  Subscription? Subscription { get; set; } =null;
    public  InvoiceItemGroup? InvoiceItemGroup { get; set; } =null;
}

#endregion




#region Tickets

public class Ticket : BaseAuditableEntity
{
    public string TicketNo { get; set; } = string.Empty;
    public ServiceTask ServiceTask { get; set; }
    public string Description { get; set; } = string.Empty;
    public TicketStatus TicketStatus { get; set; }
    public int TrackingUnitId { get; set; }
    public DateOnly TcDate { get; set; }
    public DateOnly? TaDate { get; set; }
    public DateOnly? TeDate { get; set; }
    public string? Note { get; set; }
    
    // Navigation Properties
    public  TrackingUnit? TrackingUnit { get; set; } = null;
    public  ApplicationUser? CreatedByUser { get; set; }
    public  ApplicationUser? LastModifiedByUser { get; set; }
}

#endregion


#region Database Diagnostic

public class LibyanaSimCard : BaseEntity
{
    public string? SimCardNo { get; set; }
    public SLStatus? SimCardStatus { get; set; }
    public decimal? Balance { get; set; } = 0.0m;
    public DateTime? BExDate { get; set; }
    public DateTime? JoinDate { get; set; }
    public string? Package { get; set; }
    public DateTime? DExDate { get; set; }
    public string? DataOffer { get; set; }
    public DateTime? DOExpired { get; set; }
}

// WialonUnit.cs - Clean POCO
public class WialonUnit : BaseEntity
{
    public string? UnitName { get; set; }
    public string? Account { get; set; }
    public string? UnitSNo { get; set; }
    public DateTime? Deactivation { get; set; }
    public string? SimCardNo { get; set; }
    public WStatus? StatusOnWialon { get; set; }
    public string? Note { get; set; }
}




// DataMatch.cs - Keyless Entity for Read-Only View
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
    [Display(Name = "Account")]
    public string? Account { get; set; }
    
    [Display(Name = "Client")]
    public string? Client { get; set; }
    
    [Display(Name = "Customer")]
    public string? Customer { get; set; }
    
    [Display(Name = "UnitSNo")]
    public string? UnitSNo { get; set; }
    
    [Display(Name = "SimCardNo")]
    public string? SimCardNo { get; set; }
    
    [Display(Name = "StatusOnWialon")]
    public WStatus? StatusOnWialon { get; set; }
    
    [Display(Name = "StatusOnTrdBx")]
    public UStatus? StatusOnTrdBx { get; set; }
    
    [Display(Name = "SimCardStatus")]
    public SLStatus? SimCardStatus { get; set; }
    
    [Display(Name = "LDExDate")]
    public DateTime? LDExDate { get; set; }
    
    [Display(Name = "LDOExpired")]
    public DateTime? LDOExpired { get; set; }
    
    [Display(Name = "WNote")]
    public string? WNote { get; set; }
    
    [Display(Name = "Balance")]
    public decimal? Balance { get; set; }
}

#endregion


