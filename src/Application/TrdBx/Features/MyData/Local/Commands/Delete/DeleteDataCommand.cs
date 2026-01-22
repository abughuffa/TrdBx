using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.Commands.Delete;

public class DeleteDataCommand : IRequest<Result<bool>>
{
    public DeleteDataCommand() { }

}

public class DeleteDataCommandHandler : IRequestHandler<DeleteDataCommand, Result<bool>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public DeleteDataCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    //_mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    public DeleteDataCommandHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }
    public async Task<Result<bool>> Handle(DeleteDataCommand request, CancellationToken cancellationToken)
    {

        try
        {

            //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

            var InvoiceItems = _context.InvoiceItems.ToList();
            if (InvoiceItems.Any())
            {
                foreach (var item in InvoiceItems)
                {
                    // raise a delete domain event
                    item.AddDomainEvent(new InvoiceItemDeletedEvent(item));
                    _context.InvoiceItems.Remove(item);
                }
            }

            var InvoiceItemGroups = _context.InvoiceItemGroups.ToList();
            if (InvoiceItemGroups.Any())
            {
                foreach (var item in InvoiceItemGroups)
                {
                    // raise a delete domain event
                    item.AddDomainEvent(new InvoiceItemGroupDeletedEvent(item));
                    _context.InvoiceItemGroups.Remove(item);
                }
            }


            var Invoices = _context.Invoices.ToList();
            if (Invoices.Any())
            {
                foreach (var item in Invoices)
                {
                    // raise a delete domain event
                    item.AddDomainEvent(new InvoiceDeletedEvent(item));
                    _context.Invoices.Remove(item);
                }
            }

            var CusPrices = _context.CusPrices.ToList();
            if (CusPrices.Any())
            {
                foreach (var item in CusPrices)
                {
                    // raise a delete domain event
                    item.AddDomainEvent(new CusPriceDeletedEvent(item));
                    _context.CusPrices.Remove(item);
                }
            }

            var ActivateTestCases = _context.ActivateTestCases.ToList();
            if (ActivateTestCases.Any())
            {
                foreach (var item in ActivateTestCases)
                {
                    // raise a delete domain event
                    item.AddDomainEvent(new ActivateTestCaseDeletedEvent(item));
                    _context.ActivateTestCases.Remove(item);
                }
            }

            var ActivateGprsTestCases = _context.ActivateGprsTestCases.ToList();
            if (ActivateGprsTestCases.Any())
            {
                foreach (var item in ActivateGprsTestCases)
                {
                    // raise a delete domain event
                    item.AddDomainEvent(new ActivateGprsTestCaseDeletedEvent(item));
                    _context.ActivateGprsTestCases.Remove(item);
                }
            }

            var ActivateHostingTestCases = _context.ActivateHostingTestCases.ToList();
            if (ActivateHostingTestCases.Any())
            {
                foreach (var item in ActivateHostingTestCases)
                {
                    // raise a delete domain event
                    item.AddDomainEvent(new ActivateHostingTestCaseDeletedEvent(item));
                    _context.ActivateHostingTestCases.Remove(item);
                }
            }

            var DeactivateTestCases = _context.DeactivateTestCases.ToList();
            if (DeactivateTestCases.Any())
            {
                foreach (var item in DeactivateTestCases)
                {
                    // raise a delete domain event
                    item.AddDomainEvent(new DeactivateTestCaseDeletedEvent(item));
                    _context.DeactivateTestCases.Remove(item);
                }
            }

            var Subscriptions = _context.Subscriptions.ToList();
            if (Subscriptions.Any())
            {
                foreach (var item in Subscriptions)
                {
                    // raise a delete domain event
                    item.AddDomainEvent(new SubscriptionDeletedEvent(item));
                    _context.Subscriptions.Remove(item);
                }
            }
            var WialonTasks = _context.WialonTasks.ToList();
            if (WialonTasks.Any())
            {
                foreach (var item in WialonTasks)
                {
                    // raise a delete domain event
                    item.AddDomainEvent(new WialonTaskDeletedEvent(item));
                    _context.WialonTasks.Remove(item);
                }
            }
            var ServiceLogs = _context.ServiceLogs.ToList();
            if (ServiceLogs.Any())
            {

                foreach (var item in ServiceLogs)
                {
                    // raise a delete domain event
                    item.AddDomainEvent(new ServiceLogDeletedEvent(item));
                    _context.ServiceLogs.Remove(item);
                }
            }


            var TrackingUnits = _context.TrackingUnits.ToList();
            if (TrackingUnits.Any())
            {
                foreach (var item in TrackingUnits)
                {
                    // raise a delete domain event
                    item.AddDomainEvent(new TrackingUnitDeletedEvent(item));
                    _context.TrackingUnits.Remove(item);
                }
            }

            var TrackingUnitModels = _context.TrackingUnitModels.ToList();
            if (TrackingUnitModels.Any())
            {
                foreach (var item in TrackingUnitModels)
                {
                    // raise a delete domain event
                    item.AddDomainEvent(new TrackingUnitModelDeletedEvent(item));
                    _context.TrackingUnitModels.Remove(item);
                }
            }

            var Sims = _context.SimCards.ToList();
            if (Sims.Any())
            {
                foreach (var item in Sims)
                {
                    // raise a delete domain event
                    item.AddDomainEvent(new SimCardDeletedEvent(item));
                    _context.SimCards.Remove(item);
                }
            }
            var SPackages = _context.SPackages.ToList();
            if (SPackages.Any())
            {
                foreach (var item in SPackages)
                {
                    // raise a delete domain event
                    item.AddDomainEvent(new SPackageDeletedEvent(item));
                    _context.SPackages.Remove(item);
                }
            }
            var TrackedAssets = _context.TrackedAssets.ToList();
            if (TrackedAssets.Any())
            {
                foreach (var item in TrackedAssets)
                {
                    // raise a delete domain event
                    item.AddDomainEvent(new TrackedAssetDeletedEvent(item));
                    _context.TrackedAssets.Remove(item);
                }

            }

            var CCustomers = _context.Customers.Where(c => c.ParentId != null).ToList();
            if (CCustomers.Any())
            {
                foreach (var item in CCustomers)
                {
                    // raise a delete domain event
                    item.AddDomainEvent(new CustomerDeletedEvent(item));
                    _context.Customers.Remove(item);
                }
            }

            var CClients = _context.Customers.Where(c => c.ParentId == null).ToList();
            if (CClients.Any())
            {
                foreach (var item in CClients)
                {
                    // raise a delete domain event
                    item.AddDomainEvent(new CustomerDeletedEvent(item));
                    _context.Customers.Remove(item);
                }
            }

            var SProviders = _context.SProviders.ToList();
            if (SProviders.Any())
            {
                foreach (var item in SProviders)
                {
                    // raise a delete domain event
                    item.AddDomainEvent(new SProviderDeletedEvent(item));
                    _context.SProviders.Remove(item);
                }
            }


            //********************************************

            var i = await _context.SaveChangesAsync(cancellationToken);

            if (i >= 0)
                return await Result<bool>.SuccessAsync(true);
            else
                return await Result<bool>.FailureAsync("Faild");
        }
        catch
        {

            return await Result<bool>.FailureAsync("Faild");
        }

    }
}

