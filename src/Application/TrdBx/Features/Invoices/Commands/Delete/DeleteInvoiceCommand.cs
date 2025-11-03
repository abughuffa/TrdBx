using CleanArchitecture.Blazor.Application.Features.Invoices.Caching;
using CleanArchitecture.Blazor.Domain.Enums;
using CleanArchitecture.Blazor.Domain.Enums;
using CleanArchitecture.Blazor.Domain.Events;


namespace CleanArchitecture.Blazor.Application.Features.Invoices.Commands.Delete;

public class DeleteInvoiceCommand : ICacheInvalidatorRequest<Result>
{
    public int Id { get; }
     public IEnumerable<string> Tags => InvoiceCacheKey.Tags;
    public DeleteInvoiceCommand(int id)
    {
        Id = id;
    }
}

public class DeleteInvoiceCommandHandler :
             IRequestHandler<DeleteInvoiceCommand, Result>

{
    private readonly IApplicationDbContextFactory _dbContextFactory;
    public DeleteInvoiceCommandHandler(
        IApplicationDbContextFactory dbContextFactory
    )
    {
        _dbContextFactory = dbContextFactory;
    }
    public async Task<Result> Handle(DeleteInvoiceCommand request, CancellationToken cancellationToken)
    {
        await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        var item = await db.Invoices.Where(x => x.Id == request.Id).FirstAsync(cancellationToken);


        if (item == null)
        {
            return await Result.FailureAsync($"Invoice with id: [{request.Id}] not found.");
        }

        if (item.IStatus == IStatus.SentToTax
                          || item.IsTaxable && (item.IStatus == IStatus.Ready
                                              || item.IStatus == IStatus.Billed
                                              || item.IStatus == IStatus.Canceled))
        {
            return await Result.FailureAsync($"Faild to delete Invoice with id: [{request.Id}].");
        }

        var InvoiceItems = db.InvoiceItems.Include(ii => ii.ServiceLog).Where(ii => ii.InvoiceId == request.Id).ToList();

        if (InvoiceItems.Count != 0)
        {
            foreach (var i in InvoiceItems)
            {
                i.ServiceLog.IsBilled = false;
                i.ServiceLog.AddDomainEvent(new ServiceLogUpdatedEvent(i.ServiceLog));
                // raise a delete domain event
                i.AddDomainEvent(new InvoiceItemDeletedEvent(i));
                db.InvoiceItems.Remove(i);
            }
        }

        item.AddDomainEvent(new InvoiceDeletedEvent(item));
        db.Invoices.Remove(item);

        await db.SaveChangesAsync(cancellationToken);
        return await Result.SuccessAsync();
    }


}

