using CleanArchitecture.Blazor.Application.Features.Customers.Caching;
using CleanArchitecture.Blazor.Domain.Enums;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Enums;
using CleanArchitecture.Blazor.Domain.Events;
using CleanArchitecture.Blazor.Application.Features.Customers.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.Customers.Commands.Create;

public class CreateChildCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")]
    public int Id { get; set; }
    [Description("ParentId")]
    public int ParentId { get; set; }
    [Description("Name")]
    public string? Name { get; set; }

    [Description("UserName")]
    public string? UserName { get; set; }

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
    //        CreateMap<CreateChildCommand, Customer>(MemberList.None)
    //            .ForMember(dest => dest.Account, opt => opt.Ignore())
    //            .ForMember(dest => dest.BillingPlan, opt => opt.Ignore());

    //    }
    //}
}

public class CreateChildCommandHandler : IRequestHandler<CreateChildCommand, Result<int>>
{

    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public CreateChildCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}


    private readonly IApplicationDbContext _context;
    public CreateChildCommandHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }

    public async Task<Result<int>> Handle(CreateChildCommand request, CancellationToken cancellationToken)
    {

        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        var parent = await _context.Customers.Where(p => p.Id == request.ParentId).FirstAsync();
        //var item = _mapper.Map<Customer>(request);

        var item = Mapper.FromCreateChildCommand(request);

        item.Account = parent.Account;
        item.BillingPlan = parent.BillingPlan;

        if (parent.BillingPlan == BillingPlan.Advanced)
        {
            item.IsTaxable = parent.IsTaxable;
            item.IsRenewable = parent.IsRenewable;
        }

        // raise a create domain event
        item.AddDomainEvent(new CustomerCreatedEvent(item));
        _context.Customers.Add(item);
        await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(item.Id);
    
    }
}

