
using CleanArchitecture.Blazor.Application.Features.Customers.Caching;
using CleanArchitecture.Blazor.Application.Features.Customers.DTOs;
using CleanArchitecture.Blazor.Application.Features.Customers.Specifications;


namespace CleanArchitecture.Blazor.Application.Features.Customers.Queries.GetAvaliable;

public class GetAvaliableChildsQuery : ICacheableRequest<IEnumerable<CustomerDto>>
{
    public string CacheKey => CustomerCacheKey.GetAvaliableChildsCacheKey;
     public IEnumerable<string> Tags => CustomerCacheKey.Tags;
}

public class GetAvaliableChildsQueryHandler :
     IRequestHandler<GetAvaliableChildsQuery, IEnumerable<CustomerDto>>
{
        private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly TypeAdapterConfig _typeAdapterConfig;
    public GetAvaliableChildsQueryHandler(
        TypeAdapterConfig typeAdapterConfig,
        IApplicationDbContextFactory dbContextFactory)
    {
        _typeAdapterConfig = typeAdapterConfig;
        _dbContextFactory = dbContextFactory;
    }

    public async ValueTask<IEnumerable<CustomerDto>> Handle(GetAvaliableChildsQuery request, CancellationToken cancellationToken)
    {

          await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var data = await _context.Customers.ApplySpecification(new AvaliableChildsSpecification())
                                               .ProjectToType<CustomerDto>(_typeAdapterConfig)
                                               .ToListAsync(cancellationToken);
        return data;



    }
}
