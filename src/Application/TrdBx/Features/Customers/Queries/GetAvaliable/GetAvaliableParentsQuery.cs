using CleanArchitecture.Blazor.Application.Features.Customers.Caching;
using CleanArchitecture.Blazor.Application.Features.Customers.DTOs;
using CleanArchitecture.Blazor.Application.Features.Customers.Specifications;


namespace CleanArchitecture.Blazor.Application.Features.Customers.Queries.GetAvaliable;

public class GetAvaliableParentsQuery : ICacheableRequest<IEnumerable<CustomerDto>>
{
    public string CacheKey => CustomerCacheKey.GetAvaliableCustomersCacheKey;
     public IEnumerable<string> Tags => CustomerCacheKey.Tags;
}

public class GetAvaliableParentsQueryHandler :
     IRequestHandler<GetAvaliableParentsQuery, IEnumerable<CustomerDto>>
{
    private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly IMapper _mapper;
    public GetAvaliableParentsQueryHandler(
        IApplicationDbContextFactory dbContextFactory,
        IMapper mapper
    )
    {
        _dbContextFactory = dbContextFactory;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CustomerDto>> Handle(GetAvaliableParentsQuery request, CancellationToken cancellationToken)
    {
        //var data = await _context.Customers.ApplySpecification(new AvaliableParentsSpecification())
        //                                       .ProjectTo<CustomerDto>(_mapper.ConfigurationProvider)
        //                                        .ToListAsync(cancellationToken);
        //return data;

        await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        var data = await db.Customers.ApplySpecification(new AvaliableParentsSpecification())
            .ProjectTo<CustomerDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        return data;

    }
}
