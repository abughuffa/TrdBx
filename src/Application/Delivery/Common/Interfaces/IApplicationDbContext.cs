// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Common.Interfaces;

public partial interface IApplicationDbContext
{
    DbSet<POI> POIs { get; set; }
    DbSet<WayPoint> WayPoints { get; set; }
    DbSet<BidRecord> BidRecords { get; set; }
    DbSet<Vehicle> Vehicles { get; set; }
    DbSet<VehicleType> VehicleTypes { get; set; }
    DbSet<Shipment> Shipments { get; set; }
}