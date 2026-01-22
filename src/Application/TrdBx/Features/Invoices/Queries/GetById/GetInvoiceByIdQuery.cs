
using CleanArchitecture.Blazor.Application.Features.Invoices.Caching;
using CleanArchitecture.Blazor.Application.Features.Invoices.DTOs;
using CleanArchitecture.Blazor.Application.Features.Invoices.Mappers;
using CleanArchitecture.Blazor.Application.Features.Invoices.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.Invoices.Queries.GetById;

public class GetDetailedInvoiceByIdQuery : ICacheableRequest<Result<InvoiceDto>>
{
   public required int Id { get; set; }
   public string CacheKey => InvoiceCacheKey.GetByIdCacheKey($"{Id}");
    public IEnumerable<string> Tags => InvoiceCacheKey.Tags;
}

public class GetDetailedInvoiceByIdQueryHandler :
     IRequestHandler<GetDetailedInvoiceByIdQuery, Result<InvoiceDto>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public GetInvoiceByIdQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}
    private readonly IApplicationDbContext _context;
    public GetDetailedInvoiceByIdQueryHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }

    public async Task<Result<InvoiceDto>> Handle(GetDetailedInvoiceByIdQuery request, CancellationToken cancellationToken)
    {
        //await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await db.Invoices.ApplySpecification(new InvoiceByIdSpecification(request.Id))
        //                                        .ProjectTo<InvoiceDto>(_mapper.ConfigurationProvider)
        //                                        .FirstAsync(cancellationToken) ?? throw new NotFoundException($"Invoice with id: [{request.Id}] not found.");
        //return await Result<InvoiceDto>.SuccessAsync(data);

        var data = await _context.Invoices
            .ApplySpecification(new InvoiceByIdSpecification(request.Id)).Include(i=>i.InvoiceItemGroups).ThenInclude(ig =>ig.InvoiceItems)
                                       .ProjectTo()
                                       .FirstAsync(cancellationToken) ?? throw new NotFoundException($"Invoice with id: [{request.Id}] not found.");
        return await Result<InvoiceDto>.SuccessAsync(data);

    }
}
