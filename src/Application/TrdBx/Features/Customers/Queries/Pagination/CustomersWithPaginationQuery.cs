using CleanArchitecture.Blazor.Application.Features.Customers.Caching;
using CleanArchitecture.Blazor.Application.Features.Customers.DTOs;
using CleanArchitecture.Blazor.Application.Features.Customers.Mappers;
using CleanArchitecture.Blazor.Application.Features.Customers.Specifications;
using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.Customers.Queries.Pagination;

public class CustomersWithPaginationQuery : CustomerAdvancedFilter, ICacheableRequest<PaginatedData<CustomerDto>>
{

    public override string ToString()
    {
        return $"Listview:{ListView}: Search:{Keyword}, {OrderBy}, {SortDirection}, {PageNumber}, {PageSize}";
    }

    public string CacheKey => CustomerCacheKey.GetPaginationCacheKey($"{this}");
    public IEnumerable<string>? Tags => CustomerCacheKey.Tags;
    public CustomerAdvancedSpecification Specification => new CustomerAdvancedSpecification(this);

}

public class CustomersWithPaginationQueryHandler :
         IRequestHandler<CustomersWithPaginationQuery, PaginatedData<CustomerDto>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public CustomersWithPaginationQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    public CustomersWithPaginationQueryHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }



    public async Task<PaginatedData<CustomerDto>> Handle(CustomersWithPaginationQuery request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await _context.Customers.OrderBy($"{request.OrderBy} {request.SortDirection}")
        //    .ProjectToPaginatedDataAsync<Customer, CustomerDto>(request.Specification, request.PageNumber, request.PageSize, _mapper.ConfigurationProvider, cancellationToken);
        //return data;


        var data = await _context.Customers.Include(s => s.Parent).OrderBy($"{request.OrderBy} {request.SortDirection}")
                                                   .ProjectToPaginatedDataAsync(request.Specification,
                                                                                request.PageNumber,
                                                                                request.PageSize,
                                                                                Mapper.ToDto,
                                                                                cancellationToken);
        return data;
    }
}