using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.TrdBxData.Commands.Delete;


public class DeleteDataCommand : IRequest<Result<bool>>
{
    public DeleteDataCommand() { }

}

public class DeleteDataCommandHandler : IRequestHandler<DeleteDataCommand, Result<bool>>
{
    private readonly IApplicationDbContextFactory _dbContextFactory;
    public DeleteDataCommandHandler(
       IApplicationDbContextFactory dbContextFactory
    )
    {
       _dbContextFactory = dbContextFactory;
       //_mapper = mapper;
    }

    // private readonly IApplicationDbContext context;
    // public DeleteDataCommandHandler(
    //     IApplicationDbContext context
    // )
    // {
    //     context = context;
    // }
    public async ValueTask<Result<bool>> Handle(DeleteDataCommand request, CancellationToken cancellationToken)
    {

        try
        {

            await using var context = await _dbContextFactory.CreateAsync(cancellationToken);

            var InvoiceItems = context.InvoiceItems.ToList();
            if (InvoiceItems.Any())
            {
                foreach (var item in InvoiceItems)
                {
                    // raise a delete domain event
                    item.AddDomainEvent(new InvoiceItemDeletedEvent(item));
                    context.InvoiceItems.Remove(item);
                }
            }

            var InvoiceItemGroups = context.InvoiceItemGroups.ToList();
            if (InvoiceItemGroups.Any())
            {
                foreach (var item in InvoiceItemGroups)
                {
                    // raise a delete domain event
                    item.AddDomainEvent(new InvoiceItemGroupDeletedEvent(item));
                    context.InvoiceItemGroups.Remove(item);
                }
            }


            var Invoices = context.Invoices.ToList();
            if (Invoices.Any())
            {
                foreach (var item in Invoices)
                {
                    // raise a delete domain event
                    item.AddDomainEvent(new InvoiceDeletedEvent(item));
                    context.Invoices.Remove(item);
                }
            }

            var CusPrices = context.CusPrices.ToList();
            if (CusPrices.Any())
            {
                foreach (var item in CusPrices)
                {
                    // raise a delete domain event
                    item.AddDomainEvent(new CusPriceDeletedEvent(item));
                    context.CusPrices.Remove(item);
                }
            }

            // var ActivateTestCases = context.ActivateTestCases.ToList();
            // if (ActivateTestCases.Any())
            // {
            //     foreach (var item in ActivateTestCases)
            //     {
            //         // raise a delete domain event
            //         item.AddDomainEvent(new ActivateTestCaseDeletedEvent(item));
            //         context.ActivateTestCases.Remove(item);
            //     }
            // }

            // var ActivateGprsTestCases = context.ActivateGprsTestCases.ToList();
            // if (ActivateGprsTestCases.Any())
            // {
            //     foreach (var item in ActivateGprsTestCases)
            //     {
            //         // raise a delete domain event
            //         item.AddDomainEvent(new ActivateGprsTestCaseDeletedEvent(item));
            //         context.ActivateGprsTestCases.Remove(item);
            //     }
            // }

            // var ActivateHostingTestCases = context.ActivateHostingTestCases.ToList();
            // if (ActivateHostingTestCases.Any())
            // {
            //     foreach (var item in ActivateHostingTestCases)
            //     {
            //         // raise a delete domain event
            //         item.AddDomainEvent(new ActivateHostingTestCaseDeletedEvent(item));
            //         context.ActivateHostingTestCases.Remove(item);
            //     }
            // }

            // var DeactivateTestCases = context.DeactivateTestCases.ToList();
            // if (DeactivateTestCases.Any())
            // {
            //     foreach (var item in DeactivateTestCases)
            //     {
            //         // raise a delete domain event
            //         item.AddDomainEvent(new DeactivateTestCaseDeletedEvent(item));
            //         context.DeactivateTestCases.Remove(item);
            //     }
            // }

            var Subscriptions = context.Subscriptions.ToList();
            if (Subscriptions.Any())
            {
                foreach (var item in Subscriptions)
                {
                    // raise a delete domain event
                    item.AddDomainEvent(new SubscriptionDeletedEvent(item));
                    context.Subscriptions.Remove(item);
                }
            }
            var WialonTasks = context.WialonTasks.ToList();
            if (WialonTasks.Any())
            {
                foreach (var item in WialonTasks)
                {
                    // raise a delete domain event
                    item.AddDomainEvent(new WialonTaskDeletedEvent(item));
                    context.WialonTasks.Remove(item);
                }
            }
            var ServiceLogs = context.ServiceLogs.ToList();
            if (ServiceLogs.Any())
            {

                foreach (var item in ServiceLogs)
                {
                    // raise a delete domain event
                    item.AddDomainEvent(new ServiceLogDeletedEvent(item));
                    context.ServiceLogs.Remove(item);
                }
            }


            var TrackingUnits = context.TrackingUnits.ToList();
            if (TrackingUnits.Any())
            {
                foreach (var item in TrackingUnits)
                {
                    // raise a delete domain event
                    item.AddDomainEvent(new TrackingUnitDeletedEvent(item));
                    context.TrackingUnits.Remove(item);
                }
            }

            var TrackingUnitModels = context.TrackingUnitModels.ToList();
            if (TrackingUnitModels.Any())
            {
                foreach (var item in TrackingUnitModels)
                {
                    // raise a delete domain event
                    item.AddDomainEvent(new TrackingUnitModelDeletedEvent(item));
                    context.TrackingUnitModels.Remove(item);
                }
            }

            var Sims = context.SimCards.ToList();
            if (Sims.Any())
            {
                foreach (var item in Sims)
                {
                    // raise a delete domain event
                    item.AddDomainEvent(new SimCardDeletedEvent(item));
                    context.SimCards.Remove(item);
                }
            }
            var SPackages = context.SPackages.ToList();
            if (SPackages.Any())
            {
                foreach (var item in SPackages)
                {
                    // raise a delete domain event
                    item.AddDomainEvent(new SPackageDeletedEvent(item));
                    context.SPackages.Remove(item);
                }
            }
            var TrackedAssets = context.TrackedAssets.ToList();
            if (TrackedAssets.Any())
            {
                foreach (var item in TrackedAssets)
                {
                    // raise a delete domain event
                    item.AddDomainEvent(new TrackedAssetDeletedEvent(item));
                    context.TrackedAssets.Remove(item);
                }

            }

            var CCustomers = context.Customers.Where(c => c.ParentId != null).ToList();
            if (CCustomers.Any())
            {
                foreach (var item in CCustomers)
                {
                    // raise a delete domain event
                    item.AddDomainEvent(new CustomerDeletedEvent(item));
                    context.Customers.Remove(item);
                }
            }

            var CClients = context.Customers.Where(c => c.ParentId == null).ToList();
            if (CClients.Any())
            {
                foreach (var item in CClients)
                {
                    // raise a delete domain event
                    item.AddDomainEvent(new CustomerDeletedEvent(item));
                    context.Customers.Remove(item);
                }
            }

            var SProviders = context.SProviders.ToList();
            if (SProviders.Any())
            {
                foreach (var item in SProviders)
                {
                    // raise a delete domain event
                    item.AddDomainEvent(new SProviderDeletedEvent(item));
                    context.SProviders.Remove(item);
                }
            }


            //********************************************

            var i = await context.SaveChangesAsync(cancellationToken);

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

