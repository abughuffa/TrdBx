using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.ImpulseCharts.Caching;
using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.ImpulseCharts.DTOs;
using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.ImpulseCharts.Specifications;


namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.ImpulseCharts.Queries.GetImpulseCharts;


public class GetImpulseChartsQuery : ImpulseChartAdvancedFilter, ICacheableRequest<List<ImpulseChartDto>>
{

    public IEnumerable<string>? Tags => ImpulseChartCacheKey.Tags;
    public ImpulseChartAdvancedSpecification Specification => new(this);
    public string CacheKey => ImpulseChartCacheKey.GetPaginationCacheKey($"{this}");
    public override string ToString()
    {
        return $"Listview:{ListView}, Customer:{CustomerId} StartDate:{FromDate}, EndDate:{ToDate}";
    }

}

public class GetImpulseChartsQueryHandler : IRequestHandler<GetImpulseChartsQuery, List<ImpulseChartDto>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public GetImpulseChartsQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    public GetImpulseChartsQueryHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }

    public async Task<List<ImpulseChartDto>> Handle(GetImpulseChartsQuery request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var result = new List<ImpulseChartDto>();

        switch (request.ListView)
        {
            case ImpulseChartListView.SimCardsExpiryDate:
                {
                    try
                    {
                        var currentDate = DateOnly.FromDateTime(DateTime.Today);

                        var dailyData = await _context.TrackingUnits
                            .Include(t => t.Customer).ThenInclude(c=>c.Parent)
                            .Include(t => t.SimCard)
                            .ApplySpecification(request.Specification)
                            .Where(t => t.SimCard != null)
                            .GroupBy(t => t.SimCard!.ExDate.HasValue ? t.SimCard.ExDate.Value : currentDate)
                            .Select(g => new {
                                Date = g.Key,
                                Items = g.Select(t => new ItemDto
                                {
                                    Id = t.SimCard != null ? t.SimCard.Id : 0,
                                    ParentName = t.Customer.Parent != null ? t.Customer.Parent.Name : string.Empty,
                                    ChildName = t.Customer != null ? t.Customer.Name : string.Empty,
                                    SNo = t.SNo ?? string.Empty,
                                    SimNo = t.SimCard!.SimCardNo.ToString(),
                                    Status = t.UStatus.ToString() ?? string.Empty
                                }).ToList()
                            })
                            .ToDictionaryAsync(x => x.Date, x => x, cancellationToken);

                        if (!dailyData.Any() && !request.FromDate.HasValue && !request.ToDate.HasValue)
                        {
                            return new List<ImpulseChartDto>();
                        }

                        var startDate = request.FromDate ??
                            (dailyData.Any() ? dailyData.Keys.Min() : currentDate);
                        var endDate = request.ToDate ??
                            (dailyData.Any() ? dailyData.Keys.Max() : currentDate);

                        // Ensure startDate <= endDate
                        if (startDate > endDate)
                        {
                            (startDate, endDate) = (endDate, startDate);
                        }

                        return [.. Enumerable.Range(0, (endDate.DayNumber - startDate.DayNumber) + 1)
        .Select(offset => startDate.AddDays(offset))
        .Select(date =>
        {
            if (dailyData.TryGetValue(date, value: out var data))
            {
                return new ImpulseChartDto
                {
                    Date = date,
                    Items = data.Items
                };
            }
            else
            {
                return new ImpulseChartDto
                {
                    Date = date,
                    Items = new List<ItemDto>()
                };
            }
        })];
                    }
                    catch (Exception ex)
                    {
                        // Log exception
                        // _logger.LogError(ex, "Error generating expiry date counts");
                        throw new ApplicationException("Error generating expiry date counts", ex);
                    }
                    //try
                    //{
                    //    var currentDate = DateOnly.FromDateTime(DateTime.Today);

                    //    var dailyData = await _context.TrackingUnits
                    //        .Include(t => t.SimCard)
                    //        .ApplySpecification(request.Specification)
                    //        .Where(t => t.SimCard != null) // Removed the ExDate check since we'll handle nulls
                    //        .GroupBy(t => t.SimCard!.ExDate.HasValue ? t.SimCard.ExDate.Value : currentDate)
                    //        .Select(g => new {
                    //            Date = g.Key,
                    //            Items = g.Select(t => t.SimCard.SimCardNo.ToString()).ToList()
                    //        })
                    //        .ToDictionaryAsync(x => x.Date, x => x, cancellationToken);

                    //    if (!dailyData.Any() && !request.FromDate.HasValue && !request.ToDate.HasValue)
                    //    {
                    //        return new List<ImpulseChartDto>();
                    //    }

                    //    var startDate = request.FromDate ??
                    //        (dailyData.Any() ? dailyData.Keys.Min() : currentDate);
                    //    var endDate = request.ToDate ??
                    //        (dailyData.Any() ? dailyData.Keys.Max() : currentDate);

                    //    // Ensure startDate <= endDate
                    //    if (startDate > endDate)
                    //    {
                    //        (startDate, endDate) = (endDate, startDate);
                    //    }

                    //    return [.. Enumerable.Range(0, (endDate.DayNumber - startDate.DayNumber) + 1)
                    //        .Select(offset => startDate.AddDays(offset))
                    //        .Select(date =>
                    //        {
                    //            if (dailyData.TryGetValue(date, out var data))
                    //            {
                    //                return new ImpulseChartDto
                    //                {
                    //                    Date = date,
                    //                    Items = data.Items
                    //                };
                    //            }
                    //            else
                    //            {
                    //                return new ImpulseChartDto
                    //                {
                    //                    Date = date,
                    //                    Items = new List<string>()
                    //                };
                    //            }
                    //        })];
                    //}
                    //catch (Exception ex)
                    //{
                    //    // Log exception
                    //    // _logger.LogError(ex, "Error generating expiry date counts");
                    //    throw new ApplicationException("Error generating expiry date counts", ex);
                    //}
                }
            case ImpulseChartListView.UnitSubExpiryDate:
                {

                    try
                    {
                        var dailyData = await _context.TrackingUnits
                            .Include(t => t.Customer).ThenInclude(c => c.Parent)
                            .Include(t => t.Subscriptions)
                            .Include(t => t.SimCard) // Added to get SimCard information
                            .ApplySpecification(request.Specification)
                            .Where(t => t.Subscriptions.Any())
                            .Select(t => new
                            {
                                TrackingUnit = t,
                                LatestSubscription = t.Subscriptions
                                    .OrderByDescending(s => s.SeDate)
                                    .FirstOrDefault()
                            })
                            .Where(x => x.LatestSubscription != null && x.LatestSubscription.SeDate != default)
                            .GroupBy(x => x.LatestSubscription!.SeDate)
                            .Select(g => new
                            {
                                Date = g.Key,
                                Items = g.Select(x => new ItemDto
                                {
                                    Id = x.TrackingUnit != null ? x.TrackingUnit.Id : 0,
                                    ParentName = x.TrackingUnit.Customer.Parent != null ? x.TrackingUnit.Customer.Parent.Name : string.Empty,
                                    ChildName = x.TrackingUnit.Customer != null ? x.TrackingUnit.Customer.Name : string.Empty,
                                    SNo = x.TrackingUnit.SNo ?? string.Empty,
                                    SimNo = x.TrackingUnit.SimCard != null ? x.TrackingUnit.SimCard.SimCardNo.ToString() : string.Empty,
                                    Status = x.TrackingUnit.UStatus.ToString() ?? string.Empty // Using TrackingUnit status
                                }).ToList()
                            })
                            .ToDictionaryAsync(x => x.Date, x => x, cancellationToken);

                        // Handle empty case
                        if (!dailyData.Any() && !request.FromDate.HasValue && !request.ToDate.HasValue)
                        {
                            return new List<ImpulseChartDto>();
                        }

                        var startDate = request.FromDate ??
                            (dailyData.Any() ? dailyData.Keys.Min() : DateOnly.FromDateTime(DateTime.Today));
                        var endDate = request.ToDate ??
                            (dailyData.Any() ? dailyData.Keys.Max() : DateOnly.FromDateTime(DateTime.Today));

                        // Ensure startDate <= endDate
                        if (startDate > endDate)
                        {
                            (startDate, endDate) = (endDate, startDate);
                        }

                        // Validate date range isn't too large (optional safety check)
                        //const int maxDayRange = 365 * 5; // 5 years max
                        var dayRange = (endDate.DayNumber - startDate.DayNumber) + 1;

                        //if (dayRange > maxDayRange)
                        //{
                        //    throw new InvalidOperationException($"Date range too large: {dayRange} days. Maximum allowed: {maxDayRange} days.");
                        //}

                        return [.. Enumerable.Range(0, dayRange)
        .Select(offset => startDate.AddDays(offset))
        .Select(date =>
        {
            if (dailyData.TryGetValue(date, out var data))
            {
                return new ImpulseChartDto
                {
                    Date = date,
                    Items = data.Items
                };
            }
            else
            {
                return new ImpulseChartDto
                {
                    Date = date,
                    Items = new List<ItemDto>()
                };
            }
        })];
                    }
                    catch (Exception ex) when (ex is not ApplicationException)
                    {
                        // Log exception details here
                        //_logger?.LogError(ex, "Error generating expiry date counts for tracking units");
                        throw new ApplicationException("Error generating subscription expiry date counts", ex);
                    }

                    //try
                    //{
                    //    var dailyData = await _context.TrackingUnits
                    //        .Include(t => t.Subscriptions)
                    //        .ApplySpecification(request.Specification)
                    //        .Where(t => t.Subscriptions.Any())
                    //        .Select(t => new
                    //        {
                    //            TrackingUnit = t,
                    //            LatestSubscription = t.Subscriptions
                    //                .OrderByDescending(s => s.SeDate)
                    //                .FirstOrDefault()
                    //        })
                    //        .Where(x => x.LatestSubscription != null && x.LatestSubscription.SeDate != default)
                    //        .GroupBy(x => x.LatestSubscription!.SeDate)
                    //        .Select(g => new
                    //        {
                    //            Date = g.Key,
                    //            //Count = (double)g.Count(),
                    //            Items = g.Select(x => x.TrackingUnit.SNo.ToString()).ToList() // Adjust based on what you want in Objects
                    //        })
                    //        .ToDictionaryAsync(x => x.Date, x => x, cancellationToken);

                    //    // Handle empty case
                    //    if (!dailyData.Any() && !request.FromDate.HasValue && !request.ToDate.HasValue)
                    //    {
                    //        return new List<ImpulseChartDto>();
                    //    }

                    //    var startDate = request.FromDate ??
                    //        (dailyData.Any() ? dailyData.Keys.Min() : DateOnly.FromDateTime(DateTime.Today));
                    //    var endDate = request.ToDate ??
                    //        (dailyData.Any() ? dailyData.Keys.Max() : DateOnly.FromDateTime(DateTime.Today));

                    //    // Ensure startDate <= endDate
                    //    if (startDate > endDate)
                    //    {
                    //        (startDate, endDate) = (endDate, startDate);
                    //    }

                    //    // Validate date range isn't too large (optional safety check)
                    //    //const int maxDayRange = 365 * 5; // 5 years max
                    //    var dayRange = (endDate.DayNumber - startDate.DayNumber) + 1;

                    //    //if (dayRange > maxDayRange)
                    //    //{
                    //    //    throw new InvalidOperationException($"Date range too large: {dayRange} days. Maximum allowed: {maxDayRange} days.");
                    //    //}

                    //    return [.. Enumerable.Range(0, dayRange)
                    //        .Select(offset => startDate.AddDays(offset))
                    //        .Select(date =>
                    //        {
                    //            if (dailyData.TryGetValue(date, out var data))
                    //            {
                    //                return new ImpulseChartDto
                    //                {
                    //                    Date = date,
                    //                    //Count = data.Count,
                    //                    Items = data.Items
                    //                };
                    //            }
                    //            else
                    //            {
                    //                return new ImpulseChartDto
                    //                {
                    //                    Date = date,
                    //                    //Count = 0.0,
                    //                    Items = new List<string>()
                    //                };
                    //            }
                    //        })];
                    //}
                    //catch (Exception ex) when (ex is not ApplicationException)
                    //{
                    //    // Log exception details here
                    //    //_logger?.LogError(ex, "Error generating expiry date counts for tracking units");
                    //    throw new ApplicationException("Error generating subscription expiry date counts", ex);
                    //}
                }
        }

        return result;

    }
}





