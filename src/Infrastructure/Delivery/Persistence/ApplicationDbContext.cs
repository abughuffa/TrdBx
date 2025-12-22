// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Infrastructure.Persistence;

#nullable disable
public partial class ApplicationDbContext 
{
    #region Delivery
    public DbSet<WayPoint> WayPoints { get; set; }
    public DbSet<POI> POIs { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<VehicleType> VehicleTypes { get; set; }
    public DbSet<Shipment> Shipments { get; set; }
    public DbSet<BidRecord> BidRecords { get; set; }
    #endregion














}