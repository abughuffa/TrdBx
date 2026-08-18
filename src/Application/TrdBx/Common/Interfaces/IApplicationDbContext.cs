
using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Common.Interfaces;

public partial interface IApplicationDbContext
{
    DbSet<TrackingUnitModel> TrackingUnitModels { get; set; }
    DbSet<TrackingUnit> TrackingUnits { get; set; }
    DbSet<Customer> Customers { get; set; }
    DbSet<CusPrice> CusPrices { get; set; }
    DbSet<SProvider> SProviders { get; set; }
    DbSet<SPackage> SPackages { get; set; }
    DbSet<SimCard> SimCards { get; set; }
    DbSet<TrackedAsset> TrackedAssets { get; set; }
    DbSet<ServiceLog> ServiceLogs { get; set; }
    DbSet<Subscription> Subscriptions { get; set; }
    DbSet<WialonTask> WialonTasks { get; set; }
    DbSet<ServicePrice> ServicePrices { get; set; }
    DbSet<Ticket> Tickets { get; set; }
    DbSet<LibyanaSimCard> LibyanaSimCards { get; set; }
    DbSet<WialonUnit> WialonUnits { get; set; }



    DbSet<Invoice> Invoices { get; set; }
    DbSet<InvoiceItemGroup> InvoiceItemGroups { get; set; }
    DbSet<InvoiceItem> InvoiceItems { get; set; }
}