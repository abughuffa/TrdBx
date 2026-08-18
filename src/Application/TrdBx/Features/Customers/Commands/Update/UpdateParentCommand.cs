using CleanArchitecture.Blazor.Application.Features.Customers.Caching;
using System.ComponentModel.DataAnnotations;
namespace CleanArchitecture.Blazor.Application.Features.Customers.Commands.Update;

public class UpdateParentCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Display(Name = "Id")]
    public int Id { get; set; }

    [Display(Name = "Name")]
    public string? Name { get; set; }

    [Display(Name = "Account")]
    public string? Account { get; set; }

    [Display(Name = "UserName")]
    public string? UserName { get; set; }

    [Display(Name = "IsTaxable")]
    public bool IsTaxable { get; set; }
    [Display(Name = "IsRenewable")]
    public bool IsRenewable { get; set; }

    public string CacheKey => CustomerCacheKey.GetAllCacheKey;
    public IEnumerable<string> Tags => CustomerCacheKey.Tags;

    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<UpdateParentCommand, Customer>(MemberList.None);
    //        CreateMap<CustomerDto, UpdateParentCommand>(MemberList.None);
    //    }
    //}

}

public class UpdateParentCommandHandler : IRequestHandler<UpdateParentCommand, Result<int>>
{
        private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly IObjectMapper _objectMapper;
    public UpdateParentCommandHandler(
        IObjectMapper objectMapper,
        IApplicationDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
        _objectMapper = objectMapper;
    }

    public async ValueTask<Result<int>> Handle(UpdateParentCommand request, CancellationToken cancellationToken)
    {

       await using var context = await _dbContextFactory.CreateAsync(cancellationToken);

        var item = await context.Customers.Include(p => p.Childs).FirstAsync(p => p.Id == request.Id, cancellationToken);
        if (item == null) return await Result<int>.FailureAsync("Customer not found");

        foreach (var c in item.Childs)
        {
            c.Account = item.Account;
            c.IsTaxable = item.IsTaxable;
            c.IsRenewable = item.IsRenewable;
        }

        item = _objectMapper.Map(request, item);


        //_mapper.Map(request, item);
        // raise a update domain event
        item.AddDomainEvent(new CustomerUpdatedEvent(item));
        await context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(item.Id);

    }
}

