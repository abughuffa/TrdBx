using CleanArchitecture.Blazor.Application.Features.Summaries.SimCards.Caching;
using CleanArchitecture.Blazor.Application.Features.Summaries.SimCards.DTOs;
using CleanArchitecture.Blazor.Application.Features.Summaries.TrackingUnits.DTOs;
using CleanArchitecture.Blazor.Domain.Enums;
using DocumentFormat.OpenXml.InkML;


namespace CleanArchitecture.Blazor.Application.Features.Summaries.SimCards.Queries.GetSummary;

public class GetSimCardSummaryQuery : ICacheableRequest<Result<SimCardSummaryDto>>
{
    public string CacheKey => SimCardSummaryCacheKey.GetCacheKey;
     public IEnumerable<string> Tags => SimCardSummaryCacheKey.Tags;
}

public class GetSimCardSummaryQueryHandler :
     IRequestHandler<GetSimCardSummaryQuery, Result<SimCardSummaryDto>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public GetSimCardSummaryQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}
    private readonly IApplicationDbContext _context;
    public GetSimCardSummaryQueryHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }

    public async Task<Result<SimCardSummaryDto>> Handle(GetSimCardSummaryQuery request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var data = await _context.SimCards.GroupBy(x => 1) // Group all records together
                    .Select(g => new SimCardSummaryDto
                    {
                        News = g.Count(x => x.SStatus == SStatus.New),
                        Installeds = g.Count(x => x.SStatus == SStatus.Installed),
                        Recovereds = g.Count(x => x.SStatus == SStatus.Recovered),
                        Useds = g.Count(x => x.SStatus == SStatus.Used),
                        Losts = g.Count(x => x.SStatus == SStatus.Lost),
                        Counts = g.Count()
                    }).FirstOrDefaultAsync(cancellationToken);

        if (data != null) 
        return await Result<SimCardSummaryDto>.SuccessAsync(data);
        else return await Result<SimCardSummaryDto>.SuccessAsync(new SimCardSummaryDto());
    }
}
