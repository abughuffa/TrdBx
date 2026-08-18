using CleanArchitecture.Blazor.Application.Features.Customers.Caching;
using CleanArchitecture.Blazor.Domain.Enums;
using System.ComponentModel.DataAnnotations;
namespace CleanArchitecture.Blazor.Application.Features.Customers.Commands.Create;

public class CreateParentCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Display(Name = "Id")]
    public int Id { get; set; }

    [Display(Name = "Name")]
    public string? Name { get; set; }
    [Display(Name = "Account")]
    public string? Account { get; set; }
    [Display(Name = "UserName")]
    public string? UserName { get; set; }
    [Display(Name = "BillingPlan")]
    public BillingPlan BillingPlan { get; set; } = BillingPlan.Advanced;
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
    //        CreateMap<CreateParentCommand, Customer>(MemberList.None)
    //        .ForMember(dest => dest.ParentId, opt => opt.Ignore());
    //    }

    //}
}

public class CreateParentCommandHandler : IRequestHandler<CreateParentCommand, Result<int>>
{
         private readonly IObjectMapper _objectMapper;
        private readonly IApplicationDbContextFactory _dbContextFactory;
        public CreateParentCommandHandler(
            IObjectMapper objectMapper,
            IApplicationDbContextFactory dbContextFactory)
        {
            _objectMapper = objectMapper;
            _dbContextFactory = dbContextFactory;
        }
    public async ValueTask<Result<int>> Handle(CreateParentCommand request, CancellationToken cancellationToken)
    {
        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);
        //var item = _mapper.Map<Customer>(request);

        var item = _objectMapper.Map<Customer>(request);

        // raise a create domain event
        item.AddDomainEvent(new CustomerCreatedEvent(item));
        context.Customers.Add(item);
        await context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(item.Id);

    }
}

