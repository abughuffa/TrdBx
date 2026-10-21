using CleanArchitecture.Blazor.Application.Features.Customers.Caching;
using System.ComponentModel.DataAnnotations;
namespace CleanArchitecture.Blazor.Application.Features.Customers.Commands.SwitchAvailability;

public class SwitchCustomerAvailabilityCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Display(Name = "Id")]
    public int Id { get; set; }

    public string CacheKey => CustomerCacheKey.GetAllCacheKey;
    public IEnumerable<string> Tags => CustomerCacheKey.Tags;

}

public class SwitchCustomerAvailabilityCommandHandler : IRequestHandler<SwitchCustomerAvailabilityCommand, Result<int>>
{

       private readonly IApplicationDbContextFactory _dbContextFactory;
    public SwitchCustomerAvailabilityCommandHandler(
        IApplicationDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }
    public async ValueTask<Result<int>> Handle(SwitchCustomerAvailabilityCommand request, CancellationToken cancellationToken)
    {

      await using var context = await _dbContextFactory.CreateAsync(cancellationToken);

        var item = await context.Customers.Include(c=>c.Children).Where(x => x.Id == request.Id).FirstAsync() ?? throw new NotFoundException($"Customer with id: [{request.Id}] not found.");

        if (item.Children.Any(c=>c.IsAvailable))
        {
           return await Result<int>.FailureAsync("Can not Switch Customer Availability while some it's children is Available!");
        }

        item.IsAvailable = !item.IsAvailable;

        item.AddDomainEvent(new CustomerUpdatedEvent(item));

        var result = await context.SaveChangesAsync(cancellationToken);

        if (result > 0)
            return await Result<int>.SuccessAsync(item.Id);
        else
            return await Result<int>.FailureAsync("Set Customer as Available  Faild!");

    }
}

