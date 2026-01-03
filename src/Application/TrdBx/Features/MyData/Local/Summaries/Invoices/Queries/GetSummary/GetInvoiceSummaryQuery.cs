using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.Summaries.Invoices.Caching;
using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.Summaries.Invoices.DTOs;
using CleanArchitecture.Blazor.Domain.Enums;


namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.Summaries.Invoices.Queries.GetSummary;

public class GetInvoiceSummaryQuery : ICacheableRequest<Result<InvoiceSummaryDto>>
{
    public string CacheKey => InvoiceSummaryCacheKey.GetCacheKey;
     public IEnumerable<string> Tags => InvoiceSummaryCacheKey.Tags;
}

public class GetInvoiceSummaryQueryHandler :
     IRequestHandler<GetInvoiceSummaryQuery, Result<InvoiceSummaryDto>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public GetInvoiceSummaryQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    public GetInvoiceSummaryQueryHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }

    public async Task<Result<InvoiceSummaryDto>> Handle(GetInvoiceSummaryQuery request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var data = await _context.Invoices
                    .GroupBy(x => 1) // Group all records together
                    .Select(g => new InvoiceSummaryDto
                    {
                        Drafts = g.Count(x => x.IStatus == IStatus.Draft),
                        SentToTaxs = g.Count(x => x.IStatus == IStatus.SentToTax),
                        Readys = g.Count(x => x.IStatus == IStatus.Ready),
                        Billeds = g.Count(x => x.IStatus == IStatus.Billed),
                        Paids = g.Count(x => x.IStatus == IStatus.Paid),
                        Canceleds = g.Count(x => x.IStatus == IStatus.Canceled),
                        Counts = g.Count()
                    }).FirstOrDefaultAsync(cancellationToken);
        if (data != null)
            return await Result<InvoiceSummaryDto>.SuccessAsync(data);
        else return await Result<InvoiceSummaryDto>.SuccessAsync(new InvoiceSummaryDto());

    }
}
