using CleanArchitecture.Blazor.Application.Features.Summaries.TrackingUnits.Caching;
using CleanArchitecture.Blazor.Application.Features.Summaries.TrackingUnits.DTOs;
using CleanArchitecture.Blazor.Domain.Enums;
using DocumentFormat.OpenXml.InkML;


namespace CleanArchitecture.Blazor.Application.Features.Summaries.TrackingUnits.Queries.GetSummary;

public class GetTrackingUnitSummaryQuery : ICacheableRequest<Result<TrackingUnitSummaryDto>>
{
    public string CacheKey => TrackingUnitSummaryCacheKey.GetCacheKey;
     public IEnumerable<string> Tags => TrackingUnitSummaryCacheKey.Tags;
}

public class GetTrackingUnitSummaryQueryHandler :
     IRequestHandler<GetTrackingUnitSummaryQuery, Result<TrackingUnitSummaryDto>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public GetTrackingUnitSummaryQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    public GetTrackingUnitSummaryQueryHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }

    public async Task<Result<TrackingUnitSummaryDto>> Handle(GetTrackingUnitSummaryQuery request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var data = await _context.TrackingUnits
                .GroupBy(x => 1) // Group all records together
                .Select(g => new TrackingUnitSummaryDto
                {
                    News = g.Count(x => x.UStatus == UStatus.New),
                    Reserveds = g.Count(x => x.UStatus == UStatus.Reserved),
                    InstalledActiveGprss = g.Count(x => x.UStatus == UStatus.InstalledActiveGprs),
                    InstalledActiveHostings = g.Count(x => x.UStatus == UStatus.InstalledActiveHosting),
                    InstalledActives = g.Count(x => x.UStatus == UStatus.InstalledActive),
                    InstalledInactives = g.Count(x => x.UStatus == UStatus.InstalledInactive),
                    Recovereds = g.Count(x => x.UStatus == UStatus.Used),
                    Useds = g.Count(x => x.UStatus == UStatus.Used),
                    Damageds = g.Count(x => x.UStatus == UStatus.Damaged),
                    Losts = g.Count(x => x.UStatus == UStatus.Lost),
                    Counts = g.Count()
                }).FirstOrDefaultAsync(cancellationToken);



        if (data != null)
            return await Result<TrackingUnitSummaryDto>.SuccessAsync(data);
        else return await Result<TrackingUnitSummaryDto>.SuccessAsync(new TrackingUnitSummaryDto());
    }
}
