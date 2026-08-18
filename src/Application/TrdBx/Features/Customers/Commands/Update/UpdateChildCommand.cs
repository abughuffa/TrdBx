using CleanArchitecture.Blazor.Application.Features.Customers.Caching;
using System.ComponentModel.DataAnnotations;
namespace CleanArchitecture.Blazor.Application.Features.Customers.Commands.Update;

public class UpdateChildCommand : ICacheInvalidatorRequest<Result<int>>
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
    public bool IsTaxable { get; set; }
    [Display(Name = "IsRenewable")]
    public bool IsRenewable { get; set; }

    public string CacheKey => CustomerCacheKey.GetAllCacheKey;
    public IEnumerable<string> Tags => CustomerCacheKey.Tags;

    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<UpdateChildCommand, Customer>(MemberList.None);
    //        CreateMap<CustomerDto, UpdateChildCommand>(MemberList.None);
    //    }
    //}

}

public class UpdateChildCommandHandler : IRequestHandler<UpdateChildCommand, Result<int>>
{

       private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly IObjectMapper _objectMapper;
    public UpdateChildCommandHandler(
        IObjectMapper objectMapper,
        IApplicationDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
        _objectMapper = objectMapper;
    }
    public async ValueTask<Result<int>> Handle(UpdateChildCommand request, CancellationToken cancellationToken)
    {

       await using var context = await _dbContextFactory.CreateAsync(cancellationToken);

        var item = await context.Customers.FindAsync(request.Id, cancellationToken);
        if (item == null) return await Result<int>.FailureAsync("Customer not found");

        item = _objectMapper.Map(request, item);

        // raise a update domain event
        item.AddDomainEvent(new CustomerUpdatedEvent(item));
        await context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(item.Id);

    }
}

