
using CleanArchitecture.Blazor.Application.Features.Customers.Caching;
using CleanArchitecture.Blazor.Application.Features.Customers.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.Customers.Queries.GetAll;

public class GetAllCustomersQuery : ICacheableRequest<IEnumerable<CustomerDto>>
{
    public string CacheKey => CustomerCacheKey.GetAllCacheKey;
     public IEnumerable<string> Tags => CustomerCacheKey.Tags;
}

public class GetAllCustomersQueryHandler :
     IRequestHandler<GetAllCustomersQuery, IEnumerable<CustomerDto>>
{
    private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly TypeAdapterConfig _typeAdapterConfig;
    public GetAllCustomersQueryHandler(
        TypeAdapterConfig typeAdapterConfig,
        IApplicationDbContextFactory dbContextFactory)
    {
        _typeAdapterConfig = typeAdapterConfig;
        _dbContextFactory = dbContextFactory;
    }
    public async ValueTask<IEnumerable<CustomerDto>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
    {

        await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await _context.Customers
        //    .ProjectTo<CustomerDto>(_mapper.ConfigurationProvider)
        //    .ToListAsync(cancellationToken);
        //return data;

        var data = await _context.Customers.ProjectToType<CustomerDto>(_typeAdapterConfig)
                                               .AsNoTracking()
                                               .ToListAsync(cancellationToken);
        return data;


    }
}


