using CleanArchitecture.Blazor.Application.Features.Customers.Caching;
using CleanArchitecture.Blazor.Application.Features.Customers.DTOs;
using CleanArchitecture.Blazor.Application.Features.Customers.Specifications;


namespace CleanArchitecture.Blazor.Application.Features.Customers.Queries.GetAvaliable;

public class GetAvaliableParentsQuery : ICacheableRequest<IEnumerable<CustomerDto>>
{
    public string CacheKey => CustomerCacheKey.GetAvaliableParentsCacheKey;
     public IEnumerable<string> Tags => CustomerCacheKey.Tags;
}

public class GetAvaliableParentsQueryHandler :
     IRequestHandler<GetAvaliableParentsQuery, IEnumerable<CustomerDto>>
{
                private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly TypeAdapterConfig _typeAdapterConfig;
    public GetAvaliableParentsQueryHandler(
        TypeAdapterConfig typeAdapterConfig,
        IApplicationDbContextFactory dbContextFactory)
    {
        _typeAdapterConfig = typeAdapterConfig;
        _dbContextFactory = dbContextFactory;
    }

    public async ValueTask<IEnumerable<CustomerDto>> Handle(GetAvaliableParentsQuery request, CancellationToken cancellationToken)
    {
        

          await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var data = await _context.Customers.ApplySpecification(new AvaliableParentsSpecification())
                                            .ProjectToType<CustomerDto>(_typeAdapterConfig)
                                            .ToListAsync(cancellationToken);
        return data;
    }
}
