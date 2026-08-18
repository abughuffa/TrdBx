using CleanArchitecture.Blazor.Application.Features.Customers.Caching;
using CleanArchitecture.Blazor.Domain.Enums;
using System.ComponentModel.DataAnnotations;
namespace CleanArchitecture.Blazor.Application.Features.Customers.Commands.Create;

public class CreateChildCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Display(Name = "Id")]
    public int Id { get; set; }
    [Display(Name = "ParentId")]
    public int ParentId { get; set; }
    [Display(Name = "Name")]
    public string? Name { get; set; }

    [Display(Name = "UserName")]
    public string? UserName { get; set; }

    [Display(Name = "IsTaxable")]
    public bool IsTaxable { get; set; } = false;
    [Display(Name = "IsRenewable")]
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

          private readonly IObjectMapper _objectMapper;
        private readonly IApplicationDbContextFactory _dbContextFactory;
        public CreateChildCommandHandler(
            IObjectMapper objectMapper,
            IApplicationDbContextFactory dbContextFactory)
        {
            _objectMapper = objectMapper;
            _dbContextFactory = dbContextFactory;
        }

    public async ValueTask<Result<int>> Handle(CreateChildCommand request, CancellationToken cancellationToken)
    {

        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);
        var parent = await context.Customers.Where(p => p.Id == request.ParentId).FirstAsync();
        //var item = _mapper.Map<Customer>(request);

        var item = _objectMapper.Map<Customer>(request);

        item.Account = parent.Account;
        item.BillingPlan = parent.BillingPlan;

        if (parent.BillingPlan == BillingPlan.Advanced)
        {
            item.IsTaxable = parent.IsTaxable;
            item.IsRenewable = parent.IsRenewable;
        }

        // raise a create domain event
        item.AddDomainEvent(new CustomerCreatedEvent(item));
        context.Customers.Add(item);
        await context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(item.Id);
    
    }
}

