using CleanArchitecture.Blazor.Application.Features.Contacts.Mappers;
using CleanArchitecture.Blazor.Application.Features.Customers.Caching;
using CleanArchitecture.Blazor.Application.Features.Customers.DTOs;
using CleanArchitecture.Blazor.Application.Features.Customers.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.Customers.Queries.GetAll;

public class GetAllCustomersQuery : ICacheableRequest<IEnumerable<CustomerDto>>
{
    public string CacheKey => CustomerCacheKey.GetAllCacheKey;
     public IEnumerable<string> Tags => CustomerCacheKey.Tags;
}

public class GetAllCustomersQueryHandler :
     IRequestHandler<GetAllCustomersQuery, IEnumerable<CustomerDto>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public GetAllCustomersQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    public GetAllCustomersQueryHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }
    public async Task<IEnumerable<CustomerDto>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
    {

        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await _context.Customers
        //    .ProjectTo<CustomerDto>(_mapper.ConfigurationProvider)
        //    .ToListAsync(cancellationToken);
        //return data;

        var data = await _context.Customers.ProjectTo()
                                               .AsNoTracking()
                                               .ToListAsync(cancellationToken);
        return data;


    }
}


