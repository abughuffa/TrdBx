using CleanArchitecture.Blazor.Application.Features.Invoices.Caching;
using CleanArchitecture.Blazor.Domain.Enums;
using CleanArchitecture.Blazor.Domain.Enums;
using CleanArchitecture.Blazor.Domain.Events;


namespace CleanArchitecture.Blazor.Application.Features.Invoices.Commands.Delete;

public class DeleteInvoiceCommand : ICacheInvalidatorRequest<Result<int>>
{
    public int Id { get; }
     public IEnumerable<string> Tags => InvoiceCacheKey.Tags;
    public DeleteInvoiceCommand(int id)
    {
        Id = id;
    }
}

public class DeleteInvoiceCommandHandler :
             IRequestHandler<DeleteInvoiceCommand, Result<int>>

{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //public DeleteInvoiceCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //}

    private readonly IApplicationDbContext _context;
    public DeleteInvoiceCommandHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }
    public async Task<Result<int>> Handle(DeleteInvoiceCommand request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        //var item = await _context.Invoices.Where(x => x.Id == request.Id).FirstAsync(cancellationToken);


        //if (item == null)
        //{
        //    return await Result.FailureAsync($"Invoice with id: [{request.Id}] not found.");
        //}

        //if (item.IStatus == IStatus.SentToTax
        //                  || item.IsTaxable && (item.IStatus == IStatus.Ready
        //                                      || item.IStatus == IStatus.Billed
        //                                      || item.IStatus == IStatus.Canceled))
        //{
        //    return await Result.FailureAsync($"Faild to delete Invoice with id: [{request.Id}].");
        //}

        //var InvoiceItems = _context.InvoiceItems.Include(ii => ii.ServiceLog).Where(ii => ii.InvoiceId == request.Id).ToList();

        //if (InvoiceItems.Count != 0)
        //{
        //    foreach (var i in InvoiceItems)
        //    {
        //        i.ServiceLog.IsBilled = false;
        //        i.ServiceLog.AddDomainEvent(new ServiceLogUpdatedEvent(i.ServiceLog));
        //        // raise a delete domain event
        //        i.AddDomainEvent(new InvoiceItemDeletedEvent(i));
        //        _context.InvoiceItems.Remove(i);
        //    }
        //}

        //item.AddDomainEvent(new InvoiceDeletedEvent(item));
        //_context.Invoices.Remove(item);

        //await _context.SaveChangesAsync(cancellationToken);
        //return await Result.SuccessAsync();


        var item = await _context.Invoices.Where(x => x.Id == request.Id).FirstAsync(cancellationToken);


        if (item == null)
        {
            return await Result<int>.FailureAsync($"Invoice with id: [{request.Id}] not found.");
        }

        if (item.IStatus == IStatus.SentToTax
                          || item.IsTaxable && (item.IStatus == IStatus.Ready
                                              || item.IStatus == IStatus.Billed
                                              || item.IStatus == IStatus.Canceled))
        {
            return await Result<int>.FailureAsync($"Faild to delete Invoice with id: [{request.Id}].");
        }

        var InvoiceItems = _context.InvoiceItems.Include(ii => ii.ServiceLog).Where(ii => ii.InvoiceId == request.Id).ToList();

        if (InvoiceItems.Count != 0)
        {
            foreach (var i in InvoiceItems)
            {
                i.ServiceLog.IsBilled = false;
                i.ServiceLog.AddDomainEvent(new ServiceLogUpdatedEvent(i.ServiceLog));
                // raise a delete domain event
                i.AddDomainEvent(new InvoiceItemDeletedEvent(i));
                _context.InvoiceItems.Remove(i);
            }
        }

        item.AddDomainEvent(new InvoiceDeletedEvent(item));
        _context.Invoices.Remove(item);

        var result = await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(result);
    }


}

