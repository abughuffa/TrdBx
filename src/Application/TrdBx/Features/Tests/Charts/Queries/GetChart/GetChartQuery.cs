
using CleanArchitecture.Blazor.Application.Features.Charts.Caching;
using CleanArchitecture.Blazor.Application.Features.Charts.Dto;
using CleanArchitecture.Blazor.Application.Features.Charts.Specifications;


namespace CleanArchitecture.Blazor.Application.Features.Charts.Queries.GetCharts;


public class GetChartsQuery : ChartAdvancedFilter, ICacheableRequest<List<ChartDto>>
{

    public IEnumerable<string>? Tags => ChartCacheKey.Tags;
    public ChartAdvancedSpecification Specification => new(this);
    public string CacheKey => ChartCacheKey.GetPaginationCacheKey($"{this}");
    public override string ToString()
    {
        return $"Listview:{ListView}, Customer:{CustomerId} StartDate:{FromDate}, EndDate:{ToDate}";
    }

}

public class GetChartsQueryHandler : IRequestHandler<GetChartsQuery, List<ChartDto>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public GetChartsQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    public GetChartsQueryHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }

    public async Task<List<ChartDto>> Handle(GetChartsQuery request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var result = new List<ChartDto>();

        switch (request.ListView)
        {
            case ChartListView.SimCardsExpiryDate:
                {
                    try
                    {
                        var currentDate = DateOnly.FromDateTime(DateTime.Today);

                        var dailyData = await _context.TrackingUnits
                            .Include(t => t.SimCard)
                            .ApplySpecification(request.Specification)
                            .Where(t => t.SimCard != null) // Removed the ExDate check since we'll handle nulls
                            .GroupBy(t => t.SimCard!.ExDate.HasValue ? t.SimCard.ExDate.Value : currentDate)
                            .Select(g => new {
                                Date = g.Key,
                                Count = (double)g.Count(),
                                Objects = g.Select(t => t.SimCard.SimCardNo.ToString()).ToList()
                            })
                            .ToDictionaryAsync(x => x.Date, x => x, cancellationToken);

                        if (!dailyData.Any() && !request.FromDate.HasValue && !request.ToDate.HasValue)
                        {
                            return new List<ChartDto>();
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
                                if (dailyData.TryGetValue(date, out var data))
                                {
                                    return new ChartDto
                                    {
                                        Date = date,
                                        Count = data.Count,
                                        Objects = data.Objects
                                    };
                                }
                                else
                                {
                                    return new ChartDto
                                    {
                                        Date = date,
                                        Count = 0.0,
                                        Objects = new List<string>()
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
                }
            case ChartListView.UnitSubExpiryDate:
                {

                    try
                    {
                        var dailyData = await _context.TrackingUnits
                            .Include(t => t.Subscriptions)
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
                                Count = (double)g.Count(),
                                Objects = g.Select(x => x.TrackingUnit.SNo.ToString()).ToList() // Adjust based on what you want in Objects
                            })
                            .ToDictionaryAsync(x => x.Date, x => x, cancellationToken);

                        // Handle empty case
                        if (!dailyData.Any() && !request.FromDate.HasValue && !request.ToDate.HasValue)
                        {
                            return new List<ChartDto>();
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
                                    return new ChartDto
                                    {
                                        Date = date,
                                        Count = data.Count,
                                        Objects = data.Objects
                                    };
                                }
                                else
                                {
                                    return new ChartDto
                                    {
                                        Date = date,
                                        Count = 0.0,
                                        Objects = new List<string>()
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
                }
        }

        return result;

    }
}





