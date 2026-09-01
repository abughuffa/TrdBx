
using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.ImpulseCharts.Caching;
using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.ImpulseCharts.DTOs;
using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.ImpulseCharts.Specifications;


namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.ImpulseCharts.Queries.GetImpulseCharts;

public class GetImpulseChartsQuery : ImpulseChartAdvancedFilter, ICacheableRequest<IEnumerable<Impulse>>
{


    public IEnumerable<string>? Tags => ImpulseChartCacheKey.Tags;
    public ImpulseChartAdvancedSpecification Specification => new(this);
    public string CacheKey => ImpulseChartCacheKey.GetPaginationCacheKey($"{this}");


    public override string ToString()
    {
        return $"Listview:{ListView}, Customer:{CustomerId} StartDate:{FromDate}, EndDate:{ToDate}";
    }


}



// Handler Implementation
public class GetImpulseChartsQueryHandler : IRequestHandler<GetImpulseChartsQuery, IEnumerable<Impulse>>
{
    private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly ILogger<GetImpulseChartsQueryHandler> _logger;

    public GetImpulseChartsQueryHandler(
        IApplicationDbContextFactory dbContextFactory,
        ILogger<GetImpulseChartsQueryHandler> logger)
    {
        _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async ValueTask<IEnumerable<Impulse>> Handle(GetImpulseChartsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            //request.Validate();

            await using var context = await _dbContextFactory.CreateAsync(cancellationToken);

            return request.ListView switch
            {
                ImpulseChartListView.SimCardsExpiryDate => 
                    await GetSimCardExpiryDataAsync(context, request, cancellationToken),
                
                ImpulseChartListView.UnitSubExpiryDate => 
                    await GetUnitSubscriptionExpiryDataAsync(context, request, cancellationToken),
                
                _ => new List<Impulse>()
            };
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Validation error in GetImpulseCharts query");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing GetImpulseCharts query for ListView: {ListView}", request.ListView);
            throw new ApplicationException("Error generating impulse chart data", ex);
        }
    }

    #region Private Helper Methods

    private async Task<List<Impulse>> GetSimCardExpiryDataAsync(
        IApplicationDbContext context,
        GetImpulseChartsQuery request,
        CancellationToken cancellationToken)
    {
        var currentDate = DateOnly.FromDateTime(DateTime.Today);

        // Optimized query - load only necessary data
        var query = context.TrackingUnits
            .Include(t=>t.SimCard)
            .Include(t=>t.Customer).ThenInclude(c=>c.Parent) 
            .ApplySpecification(request.Specification)  
            .Where(t => t.SimCard != null)
            .AsNoTracking()
            .Select(t => new ExpiryObject
            {
                ObjectId = t.SimCardId! ?? 0,
                ExDate = t.SimCard!.ExDate ?? currentDate,

                CustomerName = t.Customer != null && t.Customer.Parent != null 
                    ? string.Format("{0} - {1}",t.Customer.Parent.Name,t.Customer.Name)
                    : t.Customer!.Name ?? string.Empty,
                SNo = t.SNo ?? string.Empty,
                SimNo = t.SimCard!.SimCardNo ?? string.Empty,
                Status = t.UStatus.ToString(),
                DaysRemaining = 0, // (int?)(t.SimCard!.ExDate!.ToDateTime(TimeOnly.MinValue) - DateTime.Today).TotalDays,
                ObjectStatus = t.SimCard.ExDate < DateOnly.FromDateTime(DateTime.Today)
                    ? "Expired"
                    : "Active"
            });

        var projectionList = await query.ToListAsync(cancellationToken);

        if (!projectionList.Any() && !request.FromDate.HasValue && !request.ToDate.HasValue)
        {
            return new List<Impulse>();
        }

        return BuildDateRangeResult(
            projectionList,
            request,
            currentDate,
            p => p.ExDate,
            p => new ExpiryObject
            {
                

                ObjectId = p.ObjectId,
                CustomerName = p.CustomerName,
                SNo = p.SNo,
                SimNo = p.SimNo,
                Status = p.Status,
                DaysRemaining = p.DaysRemaining,
                ObjectStatus = p.ObjectStatus
                
            });
    }

    private async Task<List<Impulse>> GetUnitSubscriptionExpiryDataAsync(
        IApplicationDbContext context,
        GetImpulseChartsQuery request,
        CancellationToken cancellationToken)
    {
        var currentDate = DateOnly.FromDateTime(DateTime.Today);

        // Optimized query using subquery for latest subscription
        var query = context.TrackingUnits
            .Include(t=>t.SimCard)
            .Include(t=>t.Customer).ThenInclude(c=>c.Parent)
            .Include(t=>t.Subscriptions)
            .ApplySpecification(request.Specification)
            .Where(t => t.Subscriptions.Any())
            .AsNoTracking()
            .Select(t => new
            {
                //Id = t.Id,
                TrackingUnit = t,
                LatestSubscription = t.Subscriptions
                    .OrderByDescending(s => s.SeDate)
                    .FirstOrDefault()
            })
            .Where(x => x.LatestSubscription != null && x.LatestSubscription.SeDate != default)
            .Select(x => new ExpiryObject
            {
                ObjectId = x.TrackingUnit.Id,
                ExDate = x.LatestSubscription!.SeDate,

                CustomerName = x.TrackingUnit.Customer != null && x.TrackingUnit.Customer.Parent != null 
                    ? string.Format("{0} - {1}",x.TrackingUnit.Customer.Parent.Name,x.TrackingUnit.Customer.Name)
                    : x.TrackingUnit.Customer!.Name ?? string.Empty,

                SNo = x.TrackingUnit.SNo ?? string.Empty,
                SimNo = x.TrackingUnit.SimCard != null
                    ? x.TrackingUnit.SimCard.SimCardNo ?? string.Empty
                    : string.Empty,
                Status = x.TrackingUnit.UStatus.ToString(),
                DaysRemaining = (int?)(x.LatestSubscription.SeDate.ToDateTime(TimeOnly.MinValue) - DateTime.Today).TotalDays,
                ObjectStatus = x.LatestSubscription.SeDate < DateOnly.FromDateTime(DateTime.Today)
                    ? "Expired"
                    : "Active"
            });

        var projectionList = await query.ToListAsync(cancellationToken);

        if (!projectionList.Any() && !request.FromDate.HasValue && !request.ToDate.HasValue)
        {
            return new List<Impulse>();
        }

        return BuildDateRangeResult(
            projectionList,
            request,
            currentDate,
            p => p.ExDate,
            p => new ExpiryObject
            {
                

                ObjectId = p.ObjectId,
                CustomerName = p.CustomerName,
                SNo = p.SNo,
                SimNo = p.SimNo,
                Status = p.Status,
                DaysRemaining = p.DaysRemaining,
                ObjectStatus = p.ObjectStatus
                
            });
            // p => p.SeDate,
            // p => new ItemDto
            // {
            //     Id = 0,
            //     ParentName = p.ParentName,
            //     ChildName = p.ChildName,
            //     SNo = p.SNo,
            //     SimNo = p.SimNo,
            //     Status = p.Status,
            //     DaysRemaining = p.DaysRemaining,
            //     SubscriptionStatus = p.SubscriptionStatus
            // });
    }

    private List<Impulse> BuildDateRangeResult<T>(
        List<T> items,
        GetImpulseChartsQuery request,
        DateOnly currentDate,
        Func<T, DateOnly> dateSelector,
        Func<T, ExpiryObject> itemMapper)
    {
        if (!items.Any())
        {
            return new List<Impulse>();
        }

        var groupedData = items
            .GroupBy(dateSelector)
            .ToDictionary(
                g => g.Key,
                g => g.Select(itemMapper).ToList()
            );

        var startDate = request.FromDate ?? groupedData.Keys.Min();
        var endDate = request.ToDate ?? groupedData.Keys.Max();

        // Ensure startDate <= endDate
        if (startDate > endDate)
        {
            (startDate, endDate) = (endDate, startDate);
        }

        var dayRange = (endDate.DayNumber - startDate.DayNumber) + 1;

        // Check for excessive date range
        // if (dayRange > request.MaxDayRange)
        // {
        //     _logger.LogWarning(
        //         "Date range of {DayRange} days exceeds maximum of {MaxDays}",
        //         dayRange,
        //         request.MaxDayRange);
        // }

        var result = new List<Impulse>();



        for (int offset = 0; offset < dayRange; offset++)
        {
            var date = startDate.AddDays(offset);
            
            if (groupedData.TryGetValue(date, out var dateItems))
            {
                result.Add(new Impulse
                {
                    Date = date,
                    ExpiryObjects = dateItems,
                    Summary = $"Count: {dateItems.Count}"
                });
            }
            else
            {
                result.Add(new Impulse
                {
                    Date = date,
                    ExpiryObjects = new List<ExpiryObject>(),
                    Summary = "No items"
                });
            }
        }

        return result;
    }

    #endregion

    // #region Projection Classes



    // // private class SubscriptionExpiryProjection
    // // {
    // //     public int ObjectId { get; set; }
    // //     public DateOnly ExDate { get; set; }
    // //     public string CustomerName { get; set; } = string.Empty;
    // //     public string SNo { get; set; } = string.Empty;
    // //     public string SimNo { get; set; } = string.Empty;
    // //     public string Status { get; set; } = string.Empty;
    // //     public int? DaysRemaining { get; set; }
    // //     public string? ObjectStatus { get; set; }
    // // }

    // #endregion
}

// // Extensions for better query performance


// // Validation Exception
// public class ValidationException : Exception
// {
//     public ValidationException(string message) : base(message) { }
//     public ValidationException(string message, Exception innerException) : base(message, innerException) { }
// }

// // Cache Key Helper (Updated)
