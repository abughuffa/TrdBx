namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.LibyanaSimCards.Commands.Syncs;

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
    private readonly IApplicationDbContextFactory _dbContextFactory;

    public SyncSimExpairyCommandHandler(
       IApplicationDbContextFactory dbContextFactory

    )
    {
       _dbContextFactory = dbContextFactory;

    }
    public async ValueTask<Result> Handle(SyncSimExpairyCommand request, CancellationToken cancellationToken)
    {
        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);

        var libyana = await context.LibyanaSimCards.ToListAsync(cancellationToken);

        if (!libyana.Any()) return await Result.FailureAsync("Thier is no Libyana Sim Cards imported!");

            var simcards = await context.SimCards.Where(s => s.IsOwen == true).ToListAsync(cancellationToken);

            foreach (var sim in simcards)
            {
                var lsim = libyana.Find(LS=>LS.SimCardNo == sim.SimCardNo);
                sim.ExDate = lsim is not null ? lsim.DOExpired is null ? null : DateOnly.FromDateTime((DateTime)lsim.DOExpired) : null;
                sim.AddDomainEvent(new SimCardUpdatedEvent(sim));
            }

            await context.SaveChangesAsync(cancellationToken);
            return await Result.SuccessAsync();
    }
}
