using CleanArchitecture.Blazor.Application.Features.Customers.Caching;
using CleanArchitecture.Blazor.Application.Features.Customers.Mappers;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Enums;
using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.Customers.Commands.Create;

public class CreateParentCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")]
    public int Id { get; set; }

    [Description("Name")]
    public string? Name { get; set; }
    [Description("Account")]
    public string? Account { get; set; }
    [Description("UserName")]
    public string? UserName { get; set; }
    [Description("BillingPlan")]
    public BillingPlan BillingPlan { get; set; } = BillingPlan.Advanced;
    [Description("IsTaxable")]
    public bool IsTaxable { get; set; } = false;
    [Description("IsRenewable")]
    public bool IsRenewable { get; set; } = false;

    public string CacheKey => CustomerCacheKey.GetAllCacheKey;
     public IEnumerable<string> Tags => CustomerCacheKey.Tags;

    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<CreateParentCommand, Customer>(MemberList.None)
    //        .ForMember(dest => dest.ParentId, opt => opt.Ignore());
    //    }

    //}
}

public class CreateParentCommandHandler : IRequestHandler<CreateParentCommand, Result<int>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public CreateParentCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    public CreateParentCommandHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }
    public async Task<Result<int>> Handle(CreateParentCommand request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        //var item = _mapper.Map<Customer>(request);

        var item = Mapper.FromCreateParentCommand(request);

        // raise a create domain event
        item.AddDomainEvent(new CustomerCreatedEvent(item));
        _context.Customers.Add(item);
        await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(item.Id);

    }
}

