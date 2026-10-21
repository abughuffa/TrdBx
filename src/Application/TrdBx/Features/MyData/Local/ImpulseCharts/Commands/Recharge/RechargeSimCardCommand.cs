using CleanArchitecture.Blazor.Application.Features.SimCards.Caching;

using System.ComponentModel.DataAnnotations;
namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.ImpulseCharts.Commands.Recharge;

public class RechargeSimCardCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Display(Name = "Id")]
    public int Id { get; set; }

    public string CacheKey => SimCardCacheKey.GetAllCacheKey;
    public IEnumerable<string> Tags => SimCardCacheKey.Tags;

}

public class RechargeSimCardCommandHandler : IRequestHandler<RechargeSimCardCommand, Result<int>>
{
      private readonly IApplicationDbContextFactory _dbContextFactory;
    public RechargeSimCardCommandHandler(
        IApplicationDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async ValueTask<Result<int>> Handle(RechargeSimCardCommand request, CancellationToken cancellationToken)
    {
        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);
        var item = await context.SimCards.FindAsync(request.Id, cancellationToken);
        if (item == null) return await Result<int>.FailureAsync("SimCard not found");
        if (!item.IsOwned) return await Result<int>.FailureAsync("Could not recharge Sim card which not beleong to Eagele eye.");

        item.ExDate = (DateOnly.FromDateTime(DateTime.Now)).AddDays(360);
        // raise a update domain event
        item.AddDomainEvent(new SimCardUpdatedEvent(item));
        await context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(item.Id);
    }
}


