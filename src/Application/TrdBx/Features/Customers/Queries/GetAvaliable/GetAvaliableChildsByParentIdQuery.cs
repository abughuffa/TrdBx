//using EagleEye.IsMs.Application.Features.Customers.Specifications;
using CleanArchitecture.Blazor.Application.Features.Contacts.DTOs;
using CleanArchitecture.Blazor.Application.Features.Contacts.Specifications;
using CleanArchitecture.Blazor.Application.Features.Customers.Caching;
using CleanArchitecture.Blazor.Application.Features.Customers.DTOs;
using CleanArchitecture.Blazor.Application.Features.Customers.Mappers;
using CleanArchitecture.Blazor.Application.Features.Customers.Specifications;
using CleanArchitecture.Blazor.Domain.Enums;

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
    // Case 1: Id has value
    if (request.Id.HasValue)
    {
        var customer = await _context.Customers
            .Where(c => c.Id == request.Id.Value)
            .Select(c => new { c.BillingPlan, c.ParentId, c.IsAvaliable })
            .FirstOrDefaultAsync(cancellationToken);

        if (customer is null)
        
        throw new NotFoundException($"Customer with id: [{request.Id}] not found.");

        // Case 1a: Basic plan - return just this customer (regardless of IsAvailable)
        if (customer.BillingPlan == BillingPlan.Basic)
        {
            var dataX = await _context.Customers
                .Where(c => c.Id == request.Id.Value)
                .ProjectTo()
                .ToListAsync(cancellationToken);
            
            return dataX;
        }

        // Case 1b: Advanced plan - return available customers from same parent
        // Include the original customer even if IsAvailable = false
        int parentId = customer.ParentId ?? 0;
        int customerId = request.Id.Value;
        
        var data = await _context.Customers
            .Include(c => c.Parent)
            .Where(c => (c.ParentId == parentId && c.BillingPlan == BillingPlan.Advanced && c.IsAvaliable)
                        || c.Id == customerId) // Always include the original customer
            .ProjectTo()
            .ToListAsync(cancellationToken);
        
        return data;
    }

    // Case 2: Id is null - return all available customers
    // Including the customer record even if IsAvailable = false doesn't apply here
    // because we don't have a specific customer to include
    var allData = await _context.Customers
        .Include(c => c.Parent)
        .Where(c => c.IsAvaliable 
                    && ((c.ParentId == null && c.BillingPlan == BillingPlan.Basic)
                        || (c.ParentId != null && c.BillingPlan == BillingPlan.Advanced)))
        .ProjectTo()
        .ToListAsync(cancellationToken);
    
    return allData;





        
    }
}
