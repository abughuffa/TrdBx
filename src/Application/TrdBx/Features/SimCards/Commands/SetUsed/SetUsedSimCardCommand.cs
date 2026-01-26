using CleanArchitecture.Blazor.Application.Features.SimCards.Caching;

namespace CleanArchitecture.Blazor.Application.Features.SimCards.Commands.SetUsed;

public class SetUsedSimCardCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")]
    public int Id { get; set; }


    public string CacheKey => SimCardCacheKey.GetAllCacheKey;
    public IEnumerable<string> Tags => SimCardCacheKey.Tags;


    //public SetUsedSimCardCommand(int id)
    //{
    //    Id = id;
    //}
    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<SetUsedSimCardCommand, SimCard>(MemberList.None);
    //        CreateMap<SimCardDto, SetUsedSimCardCommand>(MemberList.None);
    //    }
    //}

}

public class SetUsedSimCardCommandHandler : IRequestHandler<SetUsedSimCardCommand, Result<int>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public SetUsedSimCardCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    public SetUsedSimCardCommandHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }

    public async Task<Result<int>> Handle(SetUsedSimCardCommand request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        var item = await _context.SimCards.FindAsync(request.Id, cancellationToken);
        if (item == null) return await Result<int>.FailureAsync("SimCard not found");

        if (!((item.SStatus == Domain.Enums.SStatus.Recovered)|| (item.SStatus == Domain.Enums.SStatus.Lost))) return await Result<int>.FailureAsync("Can not set this SimCard as used!");

        item.SStatus = Domain.Enums.SStatus.Used;
        // raise a update domain event
        item.AddDomainEvent(new SimCardUpdatedEvent(item));
        await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(item.Id);
    }
}


