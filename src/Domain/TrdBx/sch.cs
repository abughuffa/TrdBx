// public interface IEntity
// {
// }

// public interface IEntity<T> : IEntity
// {
//     T Id { get; set; }
// }



// public abstract class BaseAuditableEntity : BaseEntity, IAuditableEntity
// {
//     public virtual DateTime? CreatedAt { get; set; }

//     public virtual string? CreatedById { get; set; }

//     public virtual DateTime? LastModifiedAt { get; set; }

//     public virtual string? LastModifiedById { get; set; }
// }

// public interface IAuditableEntity
// {
//     DateTime? CreatedAt { get; set; }

//     string? CreatedById { get; set; }

//    DateTime? LastModifiedAt { get; set; }

//     string? LastModifiedById { get; set; }
// }

// public abstract class BaseEntity : IEntity<int>
// {
//     private readonly List<DomainEvent> _domainEvents = new();

//     [NotMapped] public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

//     public virtual int Id { get; set; }

//     public void AddDomainEvent(DomainEvent domainEvent)
//     {
//         _domainEvents.Add(domainEvent);
//     }

//     public void RemoveDomainEvent(DomainEvent domainEvent)
//     {
//         _domainEvents.Remove(domainEvent);
//     }

//     public void ClearDomainEvents()
//     {
//         _domainEvents.Clear();
//     }
// }


// public enum InsMode
// {
//     [Display(Name = "null")] Null = 0,
//     [Display(Name = "Basic")] Basic = 1,
//     [Display(Name = "Advanced")] Advanced = 2,
//     [Display(Name = "Advanced +")] AdvancedPlus = 3,
//     [Display(Name = "Advanced ++")] AdvancedPlusPlus = 4
// }
// public enum WStatus
// {
//     [Display(Name = "Active")] Active = 1,
//     [Display(Name = "Inactive")] Inactive = 2,
// }
// public enum UStatus
// {
//     [Display(Name = "New")] New = 0 ,
//     [Display(Name = "Reserved")] Reserved = 1,
//     [Display(Name = "Installed & Active Gprs")] InstalledActiveGprs = 2,
//     [Display(Name = "Installed & Active Hosting")] InstalledActiveHosting = 3,
//     [Display(Name = "Installed & Active")] InstalledActive = 4,
//     [Display(Name = "Installed & Inactive")] InstalledInactive = 5,
//     [Display(Name = "Recovered")] Recovered = 6,
//     [Display(Name = "Used")] Used = 7,
//     [Display(Name = "Damaged")] Damaged = 8,
//     [Display(Name = "Lost")] Lost = 9,
// }
// public enum SubPackageFees
// {
//     [Display(Name = "Zero Fees")] ZeroFees = 0,
//     [Display(Name = "Gprs Fees")] GprsFees = 1,
//     [Display(Name = "Host Fees")] HostFees = 2,   
//     [Display(Name = "Full Fees")] FullFees = 3
// }

// public enum SStatus
// {
//     [Display(Name = "New")] New = 0,
//     [Display(Name = "Installed")] Installed = 1,
//     [Display(Name = "Recovered")] Recovered = 2,
//     [Display(Name = "Used")] Used = 3,
//     [Display(Name = "Lost")] Lost = 4
// }

// public enum SLStatus
// {
//     [Display(Name = "Active")] Active = 0,
//     [Display(Name = "One-Way Block")] OneWayBlock = 1,
//     [Display(Name = "Two-Way Block")] TwoWayBlock = 2,
//     [Display(Name = "Frozen Block")] Frozen = 3,
//     [Display(Name = "Inactive")] Inactive = 4,
// }
// public enum TicketStatus
// {
//     [Display(Name = "Opened")] Opened = 2,
//     [Display(Name = "Accepted")] Accepted = 3,
//     [Display(Name = "Rejected")] Rejected = 5,
//     [Display(Name = "Closed")] Closed = 7
// }
// public enum ServiceTask
// {
//     [Display(Name = "Check")] Check = 1,
//     [Display(Name = "Install New unit")] Install = 2,
//     [Display(Name = "ReInstall Used unit")] ReInstall = 3,
//     [Display(Name = "Recover Installed unit")] Recover = 4,
//     [Display(Name = "Transfer Installed unit")] Transfer = 5,
//     [Display(Name = "Replace Installed unit")] Replace = 6,
//     [Display(Name = "Install SimCard card")] InstallSimCard = 7,
//     [Display(Name = "Recover SimCard card")] RecoverSimCard = 8,
//     [Display(Name = "Replace SimCard card")] ReplacSimCard = 9,
//     [Display(Name = "Activate unit's Subscription")] ActivateUnit = 10,
//     [Display(Name = "Activate unit's Subscription for GPRS")] ActivateUnitForGprs = 11,
//     [Display(Name = "Activate unit's Subscription FOR Hosting")] ActivateUnitForHosting = 12,
//     [Display(Name = "Deactivate unit's Subscription")] DeactivateUnit = 13,
//     [Display(Name = "Renew unit's Subscription")] RenewUnitSub = 14,
//     [Display(Name = "UploadedData from TrdBx")] TrdbxDataUpload = 15,
//     [Display(Name = "Status Update")] StatusUpdate = 16
// }
// public enum WialonAPIAction
// {
//     [Display(Name = "Check on Wialon")] CheckOnWialon = 0,
//     [Display(Name = "Add To Wialon")] AddToWialon = 1,
//     [Display(Name = "Update On Wialon")] UpdateOnWialon = 2,
//     [Display(Name = "Activate On Wialon")] ActivateOnWialon = 3,
//     [Display(Name = "Deactivate On Wialon")] DeactivateOnWialon = 4,
//     [Display(Name = "Remove From Wialon")] RemoveFromWialon = 5,
// }

// public enum BillingPlan
// {
    
//     [Display(Name = "Unkown")] Unkown = 0,
//     [Display(Name = "Basic")] Basic = 1,
//     [Display(Name = "Advanced")] Advanced = 2
// }
// public enum IStatus
// {
//     [Display(Name = "Draft")] Draft = 1, //just created invoice
//     [Display(Name = "SentToTax")] SentToTax = 2, //invoice in tax process
//     [Display(Name = "Ready")] Ready = 3, //invoice retrived from taxes
//     [Display(Name = "Billed")] Billed = 4, //invoice sent to customer
//     [Display(Name = "Partailly Paid")] PartaillyPaid = 5, //invoice paid
//     [Display(Name = "Paid")] Paid = 6, //invoice paid
//     [Display(Name = "Canceled")] Canceled = 7 //invoice canceled
// }
// public enum InvoiceType
// {
//     [Display(Name = "Check")] Check = 1, //just created invoice
//     [Display(Name = "Support")] Support = 2, //invoice in tax process
//     [Display(Name = "Install")] Install = 3, //invoice retrived from taxes
//     [Display(Name = "Renew")] Renew = 4, //invoice sent to customer
//     [Display(Name = "Subscription")] Subscription = 5, //invoice paid
//     [Display(Name = "Replace")] Replace = 6 //invoice canceled
// }
// public class TrackingUnitModel : BaseEntity
// {
//     public required string WialonName { get; set; } = string.Empty;
//     public required string Name { get; set; } = string.Empty;
//     public required int WhwTypeId { get; set; } = 0;
//     public required decimal DefaultHost { get; set; } = 0.0m;
//     public required decimal DefaultGprs { get; set; } = 0.0m;
//     public required decimal DefaultPrice { get; set; } = 0.0m;
//     public List<TrackingUnit>? TrackingUnits { get; set; } = null;
//     public List<CusPrice>? CusPrices { get; set; } = null;
//     public int PortNo1 { get; set; } = 0;
//     public int PortNo2 { get; set; } = 0;
//     public int? OldId { get; set; } = null; 
// }
// public class TrackingUnit : BaseAuditableEntity
// {
//     public required string SNo { get; set; } = string.Empty;
//     public string? Imei { get; set; } = string.Empty;
//     public string? UnitName { get; set; } = string.Empty;
//     public int TrackingUnitModelId { get; set; }
//     public UStatus UStatus { get; set; } = UStatus.New;
//     public InsMode InsMode { get; set; } = InsMode.Null;
//     public DateOnly? WryDate { get; set; } = null;
//     public int? TrackedAssetId { get; set; } = null;
//     public int? SimCardId { get; set; } = null;
//     public int? CustomerId { get; set; } = null;
//     public bool IsOnWialon { get; set; } = false;
//     public WStatus? WStatus { get; set; }
//     public int? WUnitId { get; set; }
//     public int? OldId { get; set; }
//     public TrackingUnitModel? TrackingUnitModel { get; set; } = null;
//     public SimCard? SimCard { get; set; } = null;
//     public Customer? Customer { get; set; } = null;
//     public TrackedAsset? TrackedAsset { get; set; } = null;
//     public List<Subscription>? Subscriptions { get; set; } = null;
//     public List<WialonTask>? WialonTasks { get; set; } = null;
// }

// public class Customer : BaseAuditableEntity
// {
//     public int? ParentId { get; set; } = null;
//     public string Name { get; set; } = string.Empty;
//     public string Account { get; set; } = string.Empty;
//     public string UserName { get; set; } = string.Empty;
//     public BillingPlan BillingPlan { get; set; }
//     public bool IsTaxable { get; set; } = false;
//     public bool IsRenewable { get; set; } = false;
//     public int? WUserId { get; set; } = null;
//     public int? WUnitGroupId { get; set; } = null;
//     public string? Address { get; set; } = string.Empty;
//     public string? Mobile1 { get; set; } = string.Empty;
//     public string? Mobile2 { get; set; } = string.Empty;
//     public string? Email { get; set; } = string.Empty;
//     public bool IsAvailable { get; set; } = true;
//     public int? OldId { get; set; } = null;
//     public Customer? Parent { get; set; } = null;
//     public List<Customer>? Childs { get; set; } = null;
//     public List<Invoice>? Invoices { get; set; } = null;
// }
// public class CusPrice : BaseAuditableEntity
// {
//     public int CustomerId { get; set; }
//     public int TrackingUnitModelId { get; set; }
//     public decimal Host { get; set; } = 0.0m;
//     public decimal Gprs { get; set; } = 0.0m;
//     public decimal Price { get; set; } = 0.0m;
//     public Customer? Customer { get; set; } = null;
//     public TrackingUnitModel? TrackingUnitModel { get; set; } = null;
// }

// public class SProvider : BaseEntity
// {
//     public required string Name { get; set; } = string.Empty;
//     public List<SPackage>? SPackages { get; set; } = null;
// }
// public class SPackage : BaseEntity
// {
//     public required string Name { get; set; } = string.Empty;
//     public int SProviderId { get; set; }
//     public SProvider? SProvider { get; set; } = null;
//     public int? OldId { get; set; } = null;
//     public List<SimCard>? SimCards { get; set; } = null;
// }
// public class SimCard : BaseAuditableEntity
// {
//     public required string SimCardNo { get; set; } = string.Empty;
//     public string? ICCID { get; set; } = string.Empty;
//     public int SPackageId { get; set; }
//     public SPackage? SPackage { get; set; } = null;
//     public SStatus SStatus { get; set; } = SStatus.New;
//     public bool IsOwned { get; set; } = true;
//     public DateOnly? ExDate { get; set; } = null;
//     public int? OldId { get; set; } = null;
//     public TrackingUnit? TrackingUnits { get; set; } = null;
// }

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

// public class ServiceLog : BaseAuditableEntity
// {
//     public string ServiceNo { get; set; } = string.Empty;
//     public ServiceTask ServiceTask { get; set; }
//     public int CustomerId { get; set; }
//     public string Description { get; set; } = string.Empty;
//     public DateOnly SerDate { get; set; }
//     public bool IsDeserved { get; set; } = true;
//     public bool IsBilled { get; set; } = false;
//     public decimal Amount { get; set; } = 0.0m;
//     public Customer? Customer { get; set; } = null;
//     public InvoiceItemGroup? InvoiceItemGroup { get; set; } = null;
//     public List<Subscription>? Subscriptions { get; set; } = null;
//     public List<WialonTask>? WialonTasks { get; set; } = null;
//     public virtual ApplicationUser? CreatedByUser { get; set; }
// }
// public class ServicePrice : BaseAuditableEntity
// {
//     public ServiceTask ServiceTask { get; set; }
//     public string Description { get; set; } = string.Empty;
//     public decimal Price { get; set; } = 0.0m;
// }
// public class Subscription : BaseEntity
// {
//     public int ServiceLogId { get; set; }
//     public int TrackingUnitId { get; set; }
//     public int CaseCode { get; set; }
//     public SubPackageFees LastPaidFees { get; set; }
//     public string Description { get; set; } = string.Empty;
//     public DateOnly SsDate { get; set; }
//     public DateOnly SeDate { get; set; }
//     public int Days { get; set; }
//     //public int Days => (int)(SeDate.ToDateTime(TimeOnly.MinValue) - SsDate.ToDateTime(TimeOnly.MinValue)).TotalDays;
//     public decimal DailyFees { get; set; } = 0.0m;
//     public decimal Amount { get; set; }
//     //public decimal Amount => Math.Round(Days * DailyFees, 3, MidpointRounding.AwayFromZero);
//     public ServiceLog? ServiceLog { get; set; } = null;
//     public TrackingUnit? TrackingUnit { get; set; } = null;
//     public InvoiceItem? InvoiceItem { get; set; } = null;
// }
// public class WialonTask : BaseEntity
// {
//     public int ServiceLogId { get; set; }
//     public int TrackingUnitId { get; set; }
//     public string Description { get; set; } = string.Empty;
//     public WialonAPIAction? WialonAPIAction { get; set; }
//     public DateOnly ExcDate { get; set; }
//     public bool IsExecuted { get; set; } = false;
//     public ServiceLog? ServiceLog { get; set; } = null;
//     public TrackingUnit? TrackingUnit { get; set; } = null;
// }

// public class Invoice : BaseAuditableEntity
// {
//     public string InvoiceNo { get; set; } = string.Empty;
//     public DateOnly InvoiceDate { get; set; }
//     public DateOnly DueDate { get; set; }
//     public DateOnly? PaymentDate { get; set; }
//     public decimal PaidAmount { get; set; } = 0.0m;
//     public InvoiceType InvoiceType { get; set; }
//     public IStatus IStatus { get; set; }
//     public string DisplayCusName { get; set; } = string.Empty; 
//     public int CustomerId { get; set; }
//     public string Description { get; set; } = string.Empty;
//     public bool IsTaxable { get; set; } = false;
//     public bool IsTaxIgnored { get; set; } = true;
//     public decimal Total { get; set; } = 0.0m;
//     public decimal DiscountRate { get; set; } = 0.0m;
//     public decimal DiscountAmount { get; set; } = 0.0m;
//     public decimal TaxRate { get; set; } = 0.0m;
//     public decimal TaxAmount { get; set; } = 0.0m;
//     public decimal TaxableAmount { get; set; } = 0.0m;
//     public decimal GrandTotal { get; set; } = 0.0m;
//     public Customer? Customer { get; set; }
//     public List<InvoiceItemGroup>? InvoiceItemGroups { get; set; } = null;
// }
// public class InvoiceItemGroup : BaseEntity
// {
//     public int SerialIndex { get; set; }
//     public int InvoiceId { get; set; }
//     public int ServiceLogId { get; set; }
//     public string Description { get; set; } = string.Empty;
//     public decimal Amount { get; set; } = 0.0m;
//     public decimal SubTotal { get; set; } = 0.0m;
//     public Invoice? Invoice { get; set; }
//     public ServiceLog? ServiceLog { get; set; }
//     public List<InvoiceItem>? InvoiceItems { get; set; } = null;
// }
// public class InvoiceItem : BaseEntity
// {
//     public int SubSerialIndex { get; set; }
//     public int InvoiceItemGroupId { get; set; }
//     public int SubscriptionId { get; set; }
//     public string? Description { get; set; }
//     public decimal Amount { get; set; } = 0.0m;
//     public Subscription? Subscription { get; set; }
//     public InvoiceItemGroup? InvoiceItemGroup { get; set; }
// }
// public class Ticket : BaseAuditableEntity
// {
//     public string TicketNo { get; set; } = string.Empty;
//     public ServiceTask ServiceTask { get; set; }
//     public string Description { get; set; } = string.Empty;
//     public TicketStatus TicketStatus { get; set; }
//     public int TrackingUnitId { get; set; }
//     public DateOnly TcDate { get; set; }
//     public DateOnly? TaDate { get; set; }
//     //public string? InstallerId { get; set; }
//     public DateOnly? TeDate { get; set; }
//     public string? Note { get; set; } = string.Empty;
//     public TrackingUnit? TrackingUnit { get; set; }
//     public virtual ApplicationUser? CreatedByUser { get; set; }
//     public virtual ApplicationUser? LastModifiedByUser { get; set; }
// }

// public class LibyanaSimCard : BaseEntity
// {
//     public string? SimCardNo { get; set; } = string.Empty;
//     //public string? SimCardStatus { get; set; } = string.Empty;
//     public SLStatus? SimCardStatus { get; set; }
//     public decimal? Balance { get; set; } = 0.0m;
//     public DateTime? BExDate { get; set; } = null;
//     public DateTime? JoinDate { get; set; } = null;
//     public string? Package { get; set; } = string.Empty;
//     public DateTime? DExDate { get; set; } = null;
//     public string? DataOffer { get; set; } = string.Empty;
//     public DateTime? DOExpired { get; set; } = null;
// }
// public class WialonUnit : BaseEntity
// {
//     public string? UnitName { get; set; } = string.Empty;
//     public string? Account { get; set; } = string.Empty;
//     public string? UnitSNo { get; set; } = string.Empty;
//     public DateTime? Deactivation { get; set; } = null;
//     public string? SimCardNo { get; set; } = string.Empty;
//     public WStatus? StatusOnWialon { get; set; } 
//     public string? Note { get; set; } = string.Empty;
// }
// public class DataMatch : IEntity
// {
//     public string? Account { get; set; }
//     public string? Client { get; set; }
//     public string? Customer { get; set; }
//     public string? WUnitSNo { get; set; }
//     public string? TUnitSNo { get; set; }
//     public string? WSimCardNo { get; set; }
//     public string? TSimCardNo { get; set; }
//     public WStatus? StatusOnWialon { get; set; }
//     public UStatus? StatusOnTrdBx { get; set; }
//     public string? TNote { get; set; }
//     public string? WNote { get; set; }
// }
// public class DataDiagnosis : IEntity
// {
//     [Display(Name = "Account")]
//     public string? Account { get; set; }
//     [Display(Name = "Client")]
//     public string? Client { get; set; }
//     [Display(Name = "Customer")]
//     public string? Customer { get; set; }
//     [Display(Name = "UnitSNo")]
//     public string? UnitSNo { get; set; }
//     [Display(Name = "SimCardNo")]
//     public string? SimCardNo { get; set; }
//     [Display(Name = "StatusOnWialon")]
//     public WStatus? StatusOnWialon { get; set; }
//     [Display(Name = "StatusOnTrdBx")]
//     public UStatus? StatusOnTrdBx { get; set; }
//     [Display(Name = "SimCardStatus")]
//     public SLStatus? SimCardStatus { get; set; }
//     [Display(Name = "LDExDate")]
//     public DateTime? LDExDate { get; set; }
//     [Display(Name = "LDOExpired")]
//     public DateTime? LDOExpired { get; set; }
//     [Display(Name = "WNote")]
//     public string? WNote { get; set; }
//     [Display(Name = "Balance")]
//     public decimal? Balance { get; set; }
// }

// public partial class ApplicationDbContext 
// {
//     #region TrdBx
//     public DbSet<TrackingUnitModel> TrackingUnitModels { get; set; }
//     public DbSet<TrackingUnit> TrackingUnits { get; set; }
//     public DbSet<Customer> Customers { get; set; }
//     public DbSet<CusPrice> CusPrices { get; set; }
//     public DbSet<SProvider> SProviders { get; set; }
//     public DbSet<SPackage> SPackages { get; set; }
//     public DbSet<SimCard> SimCards { get; set; }
//     public DbSet<TrackedAsset> TrackedAssets { get; set; }
//     public DbSet<ServiceLog> ServiceLogs { get; set; }
//     public DbSet<Subscription> Subscriptions { get; set; }
//     public DbSet<WialonTask> WialonTasks { get; set; }
//     public DbSet<ServicePrice> ServicePrices { get; set; }
//     public DbSet<Ticket> Tickets { get; set; }
//     public DbSet<LibyanaSimCard> LibyanaSimCards { get; set; }
//     public DbSet<WialonUnit> WialonUnits { get; set; }
//     public DbSet<Invoice> Invoices { get; set; }
//     public DbSet<InvoiceItemGroup> InvoiceItemGroups { get; set; }
//     public DbSet<InvoiceItem> InvoiceItems { get; set; }
//     #endregion
// }

