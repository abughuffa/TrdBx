// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Infrastructure.Persistence;

#nullable disable
public partial class ApplicationDbContext 
{
    #region TrdBx
    public DbSet<TrackingUnitModel> TrackingUnitModels { get; set; }
    public DbSet<TrackingUnit> TrackingUnits { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<CusPrice> CusPrices { get; set; }
    public DbSet<SProvider> SProviders { get; set; }
    public DbSet<SPackage> SPackages { get; set; }
    public DbSet<SimCard> SimCards { get; set; }
    public DbSet<TrackedAsset> TrackedAssets { get; set; }
    public DbSet<ServiceLog> ServiceLogs { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<WialonTask> WialonTasks { get; set; }
    public DbSet<ServicePrice> ServicePrices { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<InvoiceItem> InvoiceItems { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<DeactivateTestCase> DeactivateTestCases { get; set; }
    public DbSet<ActivateTestCase> ActivateTestCases { get; set; }
    public DbSet<ActivateGprsTestCase> ActivateGprsTestCases { get; set; }
    public DbSet<ActivateHostingTestCase> ActivateHostingTestCases { get; set; }
    public DbSet<LibyanaSimCard> LibyanaSimCards { get; set; }
    public DbSet<WialonUnit> WialonUnits { get; set; }
    #endregion








    





}