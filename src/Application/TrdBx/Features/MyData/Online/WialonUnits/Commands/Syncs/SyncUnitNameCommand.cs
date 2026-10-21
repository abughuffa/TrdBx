namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.WialonUnits.Commands.Syncs;

public class SyncUnitNameCommand : IRequest<Result>
{
}
public class SyncUnitNameCommandHandler : IRequestHandler<SyncUnitNameCommand, Result>
{
       private readonly IApplicationDbContextFactory _dbContextFactory;
    
    public SyncUnitNameCommandHandler(
       IApplicationDbContextFactory dbContextFactory
        
    )
    {
       _dbContextFactory = dbContextFactory;
        
    }
    public async ValueTask<Result> Handle(SyncUnitNameCommand request, CancellationToken cancellationToken)
    {
        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);

        var wialon = await context.WialonUnits.ToListAsync(cancellationToken);

        if (!wialon.Any()) return await Result.FailureAsync("Thier is no Wialon units imported!");

            var units = await context.TrackingUnits.ToListAsync(cancellationToken);
            foreach (var unit in units)
            {
                var wunit = wialon.Find(w=>w.UnitSNo == unit.SNo);
                unit.UnitName = wunit is not null ? wunit.UnitName != null ? wunit.UnitName : null : null;
                unit.AddDomainEvent(new TrackingUnitUpdatedEvent(unit));
            }
            await context.SaveChangesAsync(cancellationToken);
            return await Result.SuccessAsync();
    }
}
