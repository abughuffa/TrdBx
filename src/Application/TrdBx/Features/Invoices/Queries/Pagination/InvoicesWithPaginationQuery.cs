using CleanArchitecture.Blazor.Application.Features.Invoices.Caching;
using CleanArchitecture.Blazor.Application.Features.Invoices.DTOs;
using CleanArchitecture.Blazor.Application.Features.Invoices.Mappers;
using CleanArchitecture.Blazor.Application.Features.Invoices.Specifications;
using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.Invoices.Queries.Pagination;

public class InvoicesWithPaginationQuery : InvoiceAdvancedFilter, ICacheableRequest<PaginatedData<InvoiceDto>>
{
    public override string ToString()
    {

        return $"Listview:{ListView}, Search:{Keyword},Client/Customer:{CustomerId},InvoiceType:{InvoiceType},IStatus:{IStatus}, SortDirection:{SortDirection}, OrderBy:{OrderBy}, {PageNumber}, {PageSize}";
    }
    public string CacheKey => InvoiceCacheKey.GetPaginationCacheKey($"{this}");
    public IEnumerable<string> Tags => InvoiceCacheKey.Tags;
    public InvoiceAdvancedSpecification Specification => new InvoiceAdvancedSpecification(this);
}
    
public class InvoicesWithPaginationQueryHandler :
         IRequestHandler<InvoicesWithPaginationQuery, PaginatedData<InvoiceDto>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public InvoicesWithPaginationQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}
    private readonly IApplicationDbContext _context;
    public InvoicesWithPaginationQueryHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }
    public async Task<PaginatedData<InvoiceDto>> Handle(InvoicesWithPaginationQuery request, CancellationToken cancellationToken)
        {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await _context.Invoices.OrderBy($"{request.OrderBy} {request.SortDirection}")
        //                                            .ProjectToPaginatedDataAsync<Invoice, InvoiceDto>(request.Specification,
        //                                            request.PageNumber,
        //                                            request.PageSize,
        //                                            _mapper.ConfigurationProvider,
        //                                            cancellationToken);
        //return data;

        var data = await _context.Invoices.OrderBy($"{request.OrderBy} {request.SortDirection}")
                                                  .ProjectToPaginatedDataAsync(request.Specification,
                                                                               request.PageNumber,
                                                                               request.PageSize,
                                                                               Mapper.ToDto,
                                                                               cancellationToken);
        return data;
    }
}