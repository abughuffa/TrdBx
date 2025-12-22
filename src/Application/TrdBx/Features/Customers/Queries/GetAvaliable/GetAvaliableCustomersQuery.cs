using CleanArchitecture.Blazor.Application.Features.Customers.Caching;
using CleanArchitecture.Blazor.Application.Features.Customers.DTOs;
using CleanArchitecture.Blazor.Application.Features.Customers.Mappers;
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
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public GetAvaliableCustomersQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    public GetAvaliableCustomersQueryHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }

    public async Task<IEnumerable<CustomerDto>> Handle(GetAvaliableCustomersQuery request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await _context.Customers.ApplySpecification(new AvaliableCustomersSpecification())
        //    .ProjectTo<CustomerDto>(_mapper.ConfigurationProvider)
        //    .ToListAsync(cancellationToken);
        //return data;

        var data = await _context.Customers.Include(c=>c.Parent).ApplySpecification(new AvaliableCustomersSpecification(request.WithAdvParents))
                                              .ProjectTo()
                                              .ToListAsync(cancellationToken);
        return data;

    }
}
