namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.WialonUnits.Commands.Syncs;

public class SyncUnitNameCommand : IRequest<Result>
{
}
public class SyncUnitNameCommandHandler : IRequestHandler<SyncUnitNameCommand, Result>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //public SyncUnitNameCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //}

    private readonly IApplicationDbContext _context;
    public SyncUnitNameCommandHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }
    public async Task<Result> Handle(SyncUnitNameCommand request, CancellationToken cancellationToken)
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
