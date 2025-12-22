//using EagleEye.IsMs.Application.Features.Customers.Specifications;
using CleanArchitecture.Blazor.Application.Features.Contacts.DTOs;
using CleanArchitecture.Blazor.Application.Features.Contacts.Specifications;
using CleanArchitecture.Blazor.Application.Features.Customers.Caching;
using CleanArchitecture.Blazor.Application.Features.Customers.DTOs;
using CleanArchitecture.Blazor.Application.Features.Customers.Mappers;
using CleanArchitecture.Blazor.Application.Features.Customers.Specifications;


namespace CleanArchitecture.Blazor.Application.Features.Customers.Queries.GetAvaliable;

public class GetAvaliableChildsByParentIdQuery : ICacheableRequest<IEnumerable<CustomerDto>>
{
    public int? Id { get; set; }
    public string CacheKey => CustomerCacheKey.GetAvaliableChildsByParentId($"{Id}");
     public IEnumerable<string> Tags => CustomerCacheKey.Tags;
}

public class GetAvaliableChildsByParentIdQueryHandler :
     IRequestHandler<GetAvaliableChildsByParentIdQuery, IEnumerable<CustomerDto>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public GetAvaliableChildsByParentIdQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    public GetAvaliableChildsByParentIdQueryHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }

    public async Task<IEnumerable<CustomerDto>> Handle(GetAvaliableChildsByParentIdQuery request, CancellationToken cancellationToken)
    {
        

        //await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        //var customer = await _context.Customers.Where(c => c.Id == request.Id).FirstOrDefaultAsync();

        //if (customer is null)
        //{
        //    var data = await _context.Customers.ApplySpecification(new AvaliableChildsByParentIdSpecification(null))
        //                                   .ProjectTo<CustomerDto>(_mapper.ConfigurationProvider)
        //                                   .ToListAsync(cancellationToken);
        //    return data;
        //}
        //else
        //{
        //    var data = await _context.Customers.ApplySpecification(new AvaliableChildsByParentIdSpecification(customer.ParentId))
        //                                  .ProjectTo<CustomerDto>(_mapper.ConfigurationProvider)
        //                                  .ToListAsync(cancellationToken);
        //    return data;
        //}


        var customer = await _context.Customers.Where(c => c.Id == request.Id).FirstOrDefaultAsync();

        if (customer is null)
        {
            var data = await _context.Customers.Include(c => c.Parent).ApplySpecification(new AvaliableChildsByParentIdSpecification(null))
                                              .ProjectTo()
                                              .ToListAsync(cancellationToken);
            return data;
        }
        else
        {
            var data = await _context.Customers.ApplySpecification(new AvaliableChildsByParentIdSpecification(customer.ParentId))
                                               .ProjectTo()
                                               .ToListAsync(cancellationToken);
            return data;
        }

        
    }
}
