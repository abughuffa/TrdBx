namespace CleanArchitecture.Blazor.Application.Features.DbForceSyncs.Syncs;

public class SyncSimExpairyCommand : IRequest<Result>
{
}
public class SyncSimExpairyCommandHandler : IRequestHandler<SyncSimExpairyCommand, Result>
{
    //    private readonly IApplicationDbContextFactory _dbContextFactory;
    //    public SyncSimExpairyCommandHandler(
    //        IApplicationDbContextFactory dbContextFactory
    //    )
    //    {
    //        _dbContextFactory = dbContextFactory;
    //    }
    private readonly IApplicationDbContext _context;
    public SyncSimExpairyCommandHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }
    public async Task<Result> Handle(SyncSimExpairyCommand request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var libyana = await _context.LibyanaSimCards.ToListAsync(cancellationToken);

        if (!libyana.Any()) return await Result.FailureAsync("Thier is no Libyana Sim Cards imported!");

            var simcards = await _context.SimCards.ToListAsync(cancellationToken);

            foreach (var sim in simcards)
            {
                var lsim = libyana.Find(LS=>LS.SimCardNo == sim.SimCardNo);
                sim.ExDate = lsim is not null ? lsim.DOExpired is null ? null : DateOnly.FromDateTime((DateTime)lsim.DOExpired) : null;
                sim.AddDomainEvent(new SimCardUpdatedEvent(sim));
            }

            await _context.SaveChangesAsync(cancellationToken);
            return await Result.SuccessAsync();
    }
}
