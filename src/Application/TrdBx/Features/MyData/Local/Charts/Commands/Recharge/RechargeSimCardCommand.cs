using CleanArchitecture.Blazor.Application.Features.SimCards.Caching;


namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.Charts.Commands.Recharge;

public class RechargeSimCardCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")]
    public int Id { get; set; }

    public string CacheKey => SimCardCacheKey.GetAllCacheKey;
    public IEnumerable<string> Tags => SimCardCacheKey.Tags;

}

public class RechargeSimCardCommandHandler : IRequestHandler<RechargeSimCardCommand, Result<int>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public RechargeSimCardCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    public RechargeSimCardCommandHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }

    public async Task<Result<int>> Handle(RechargeSimCardCommand request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        var item = await _context.SimCards.FindAsync(request.Id, cancellationToken);
        if (item == null) return await Result<int>.FailureAsync("SimCard not found");
        if (!item.IsOwen) return await Result<int>.FailureAsync("Could not recharge Sim card which not beleong to Eagele eye.");

        item.ExDate = (DateOnly.FromDateTime(DateTime.Now)).AddDays(360);
        // raise a update domain event
        item.AddDomainEvent(new SimCardUpdatedEvent(item));
        await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(item.Id);
    }
}


