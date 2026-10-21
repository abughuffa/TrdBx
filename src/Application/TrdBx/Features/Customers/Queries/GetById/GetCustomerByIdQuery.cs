using CleanArchitecture.Blazor.Application.Features.Contacts.DTOs;
using CleanArchitecture.Blazor.Application.Features.Contacts.Specifications;
using CleanArchitecture.Blazor.Application.Features.Customers.Caching;
using CleanArchitecture.Blazor.Application.Features.Customers.DTOs;
// using CleanArchitecture.Blazor.Application.Features.Customers.Mappers;
using CleanArchitecture.Blazor.Application.Features.Customers.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.Customers.Queries.GetById;

public class GetCustomerByIdQuery : ICacheableRequest<Result<CustomerDto>>
{
    public int Id { get; set; }
    public string CacheKey => CustomerCacheKey.GetByIdCacheKey($"{Id}");
     public IEnumerable<string> Tags => CustomerCacheKey.Tags;
}

public class GetCustomerByIdQueryHandler :
     IRequestHandler<GetCustomerByIdQuery, Result<CustomerDto>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public GetCustomerByIdQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}

        private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly TypeAdapterConfig _typeAdapterConfig;
    public GetCustomerByIdQueryHandler(
        TypeAdapterConfig typeAdapterConfig,
        IApplicationDbContextFactory dbContextFactory)
    {
        _typeAdapterConfig = typeAdapterConfig;
        _dbContextFactory = dbContextFactory;
    }
    public async ValueTask<Result<CustomerDto>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
       await using var context = await _dbContextFactory.CreateAsync(cancellationToken);
        var data = await context.Customers.ApplySpecification(new CustomerByIdSpecification(request.Id))
                                              .ProjectToType<CustomerDto>(_typeAdapterConfig)
                                              .FirstAsync(cancellationToken) ?? throw new NotFoundException($"Customer with id: [{request.Id}] not found.");
        return await Result<CustomerDto>.SuccessAsync(data);
    }
}
