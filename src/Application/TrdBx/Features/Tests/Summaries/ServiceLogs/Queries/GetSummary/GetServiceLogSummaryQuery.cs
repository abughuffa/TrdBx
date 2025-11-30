using CleanArchitecture.Blazor.Application.Features.Summaries.ServiceLogs.Caching;
using CleanArchitecture.Blazor.Application.Features.Summaries.ServiceLogs.DTOs;
using CleanArchitecture.Blazor.Domain.Enums;
using DocumentFormat.OpenXml.InkML;


namespace CleanArchitecture.Blazor.Application.Features.Summaries.ServiceLogs.Queries.GetSummary;

public class GetServiceLogSummaryQuery : ICacheableRequest<Result<ServiceLogSummaryDto>>
{
    public string CacheKey => ServiceLogSummaryCacheKey.GetCacheKey;
     public IEnumerable<string> Tags => ServiceLogSummaryCacheKey.Tags;
}

public class GetServiceLogSummaryQueryHandler :
     IRequestHandler<GetServiceLogSummaryQuery, Result<ServiceLogSummaryDto>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public GetServiceLogSummaryQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    public GetServiceLogSummaryQueryHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }

    public async Task<Result<ServiceLogSummaryDto>> Handle(GetServiceLogSummaryQuery request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var data = await _context.ServiceLogs
                        .GroupBy(x => 1) // Group all records together
                        .Select(g => new ServiceLogSummaryDto
                        {
                            Checks = g.Count(x => x.ServiceTask == ServiceTask.Check && x.IsDeserved && !x.IsBilled),
                            Installs = g.Count(x => x.ServiceTask == ServiceTask.Install && x.IsDeserved && !x.IsBilled),
                            Replaces = g.Count(x => x.ServiceTask == ServiceTask.Replace && x.IsDeserved && !x.IsBilled),
                            Supports = g.Count(x => (x.ServiceTask == ServiceTask.Recover || x.ServiceTask == ServiceTask.Transfer || x.ServiceTask == ServiceTask.InstallSimCard || x.ServiceTask == ServiceTask.ReplacSimCard)
                                                  && x.IsDeserved && !x.IsBilled),
                            Subscriptions = g.Count(x => (x.ServiceTask == ServiceTask.ActivateUnit || x.ServiceTask == ServiceTask.ActivateUnitForGprs || x.ServiceTask == ServiceTask.ActivateUnitForHosting || x.ServiceTask == ServiceTask.DeactivateUnit)
                                                       && x.IsDeserved && !x.IsBilled),
                            Renews = g.Count(x => x.ServiceTask == ServiceTask.RenewUnitSub && x.IsDeserved && !x.IsBilled),
                            Counts = g.Count(x => x.IsDeserved && !x.IsBilled)
                        }).FirstOrDefaultAsync(cancellationToken);

        if (data != null)
            return await Result<ServiceLogSummaryDto>.SuccessAsync(data);
        else return await Result<ServiceLogSummaryDto>.SuccessAsync(new ServiceLogSummaryDto());
    }
}
