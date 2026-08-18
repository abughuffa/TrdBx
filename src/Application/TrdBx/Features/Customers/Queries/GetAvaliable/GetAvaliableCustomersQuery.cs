using CleanArchitecture.Blazor.Application.Features.Customers.Caching;
using CleanArchitecture.Blazor.Application.Features.Customers.DTOs;
using CleanArchitecture.Blazor.Application.Features.Customers.Specifications;


namespace CleanArchitecture.Blazor.Application.Features.Customers.Queries.GetAvaliable;

public class GetAvaliableCustomersQuery : ICacheableRequest<IEnumerable<CustomerDto>>
{
     public bool WithAdvParents { get; set; }
     public string CacheKey => CustomerCacheKey.GetAvaliableCustomersCacheKey;
     public IEnumerable<string> Tags => CustomerCacheKey.Tags;
}

public class GetAvaliableCustomersQueryHandler :
     IRequestHandler<GetAvaliableCustomersQuery, IEnumerable<CustomerDto>>
{
            private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly TypeAdapterConfig _typeAdapterConfig;
    public GetAvaliableCustomersQueryHandler(
        TypeAdapterConfig typeAdapterConfig,
        IApplicationDbContextFactory dbContextFactory)
    {
        _typeAdapterConfig = typeAdapterConfig;
        _dbContextFactory = dbContextFactory;
    }

    public async ValueTask<IEnumerable<CustomerDto>> Handle(GetAvaliableCustomersQuery request, CancellationToken cancellationToken)
    {
          await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var data = await _context.Customers.Include(c=>c.Parent).ApplySpecification(new AvaliableCustomersSpecification(request.WithAdvParents))
                                              .ProjectToType<CustomerDto>(_typeAdapterConfig)
                                              .ToListAsync(cancellationToken);
        return data;

    }
}
