namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.SyncNames;

public class SyncNamesCommand : IRequest<Result>
{
}
public class SyncNamesCommandHandler : IRequestHandler<SyncNamesCommand, Result>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //public SyncNamesCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //}

    private readonly IApplicationDbContext _context;
    public SyncNamesCommandHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }
    public async Task<Result> Handle(SyncNamesCommand request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var wialon = await _context.WialonUnits.ToListAsync(cancellationToken);

        if (!wialon.Any()) return await Result.FailureAsync("Thier is no Wialon units imported!");

            var units = await _context.TrackingUnits.ToListAsync(cancellationToken);
            foreach (var unit in units)
            {
                var wunit = wialon.Find(w=>w.UnitSNo == unit.SNo);
                unit.UnitName = wunit is not null ? wunit.UnitName != null ? wunit.UnitName : null : null;
                unit.AddDomainEvent(new TrackingUnitUpdatedEvent(unit));
            }
            await _context.SaveChangesAsync(cancellationToken);
            return await Result.SuccessAsync();
    }
}
