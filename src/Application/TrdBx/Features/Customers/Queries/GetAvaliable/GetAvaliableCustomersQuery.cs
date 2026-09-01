using CleanArchitecture.Blazor.Application.Features.Customers.Caching;
using CleanArchitecture.Blazor.Application.Features.Customers.DTOs;
using CleanArchitecture.Blazor.Application.Features.Customers.Specifications;
using CleanArchitecture.Blazor.Domain.Enums;


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
        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);



        //********

        var data = await context.Customers
                        .Include(c=>c.Parent)
                        .Where(c => c.IsAvailable)
                        .ApplySpecification(new AvaliableCustomersSpecification(request.WithAdvParents))
                        .ProjectToType<CustomerDto>(_typeAdapterConfig)
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
        return data;

    }
}
