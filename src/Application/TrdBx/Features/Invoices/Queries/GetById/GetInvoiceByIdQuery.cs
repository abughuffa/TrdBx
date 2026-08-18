
using CleanArchitecture.Blazor.Application.Features.Invoices.Caching;
using CleanArchitecture.Blazor.Application.Features.Invoices.DTOs;
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
        private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly TypeAdapterConfig _typeAdapterConfig;
    public GetDetailedInvoiceByIdQueryHandler(
        TypeAdapterConfig typeAdapterConfig,
        IApplicationDbContextFactory dbContextFactory)
    {
        _typeAdapterConfig = typeAdapterConfig;
        _dbContextFactory = dbContextFactory;
    }

    public async ValueTask<Result<InvoiceDto>> Handle(GetDetailedInvoiceByIdQuery request, CancellationToken cancellationToken)
    {
        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);
        var data = await context.Invoices.ApplySpecification(new InvoiceByIdSpecification(request.Id)).Include(i=>i.InvoiceItemGroups).ThenInclude(ig =>ig.InvoiceItems)
                                      .ProjectToType<InvoiceDto>(_typeAdapterConfig)
                                       .FirstAsync(cancellationToken) ?? throw new NotFoundException($"Invoice with id: [{request.Id}] not found.");
        return await Result<InvoiceDto>.SuccessAsync(data);

    }
}
