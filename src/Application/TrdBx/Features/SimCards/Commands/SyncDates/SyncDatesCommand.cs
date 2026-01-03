namespace CleanArchitecture.Blazor.Application.Features.SimCards.Commands.SyncDates;

public class SyncDatesCommand : IRequest<Result>
{
}
public class SyncDatesCommandHandler : IRequestHandler<SyncDatesCommand, Result>
{
    //    private readonly IApplicationDbContextFactory _dbContextFactory;
    //    public SyncDatesCommandHandler(
    //        IApplicationDbContextFactory dbContextFactory
    //    )
    //    {
    //        _dbContextFactory = dbContextFactory;
    //    }
    private readonly IApplicationDbContext _context;
    public SyncDatesCommandHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }
    public async Task<Result> Handle(SyncDatesCommand request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var libyana = await _context.LibyanaSimCards.ToListAsync(cancellationToken);

        if (!libyana.Any()) return await Result.FailureAsync("Thier is no Libyana Sim Cards imported!");

            var simcards = await _context.SimCards.Where(s=>s.IsOwen==true).ToListAsync(cancellationToken);

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
