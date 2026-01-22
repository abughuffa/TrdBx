using CleanArchitecture.Blazor.Application.Features.Invoices.Caching;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Enums;


namespace CleanArchitecture.Blazor.Application.Features.Invoices.Commands.Delete;

public class DeleteInvoiceItemCommand : ICacheInvalidatorRequest<Result<int>>
{
    public int Id { get; }
    public IEnumerable<string> Tags => InvoiceCacheKey.Tags;
    public DeleteInvoiceItemCommand(int id)
    {
        Id = id;
    }
}

public class DeleteInvoiceItemCommandHandler :
             IRequestHandler<DeleteInvoiceItemCommand, Result<int>>

{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //public DeleteInvoiceCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //}

    private readonly IApplicationDbContext _context;
    public DeleteInvoiceItemCommandHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }
    public async Task<Result<int>> Handle(DeleteInvoiceItemCommand request, CancellationToken cancellationToken)
    {


        try
        {
            // Load invoice item with its parent relationships
            var invoiceItem = await _context.InvoiceItems
                .Include(ii => ii.InvoiceItemGroup) // Load parent item group
                    .ThenInclude(ig => ig.Invoice) // Load parent invoice
                .Include(ii => ii.Subscription) // Load related Sub
                .FirstOrDefaultAsync(ii => ii.Id == request.Id);

            if (invoiceItem == null) await Result<int>.FailureAsync($"Faild to delete Item with id: [{request.Id}].");


            if (invoiceItem.InvoiceItemGroup.Invoice.IStatus == IStatus.SentToTax
                  || invoiceItem.InvoiceItemGroup.Invoice.IsTaxable && (invoiceItem.InvoiceItemGroup.Invoice.IStatus == IStatus.Ready
                                      || invoiceItem.InvoiceItemGroup.Invoice.IStatus == IStatus.Billed
                                      || invoiceItem.InvoiceItemGroup.Invoice.IStatus == IStatus.Canceled)
                  || invoiceItem.InvoiceItemGroup.Invoice.IStatus == IStatus.PartaillyPaid
                  || invoiceItem.InvoiceItemGroup.Invoice.IStatus == IStatus.Paid)
            {
                return await Result<int>.FailureAsync($"Faild to delete Item with id: [{request.Id}].");
            }


            var itemGroup = invoiceItem.InvoiceItemGroup;
            var invoice = itemGroup.Invoice;

            invoiceItem.AddDomainEvent(new InvoiceItemDeletedEvent(invoiceItem));
            // Remove the specific invoice item
            _context.InvoiceItems.Remove(invoiceItem);

            // Get remaining items in the same group
            var remainingItems = await _context.InvoiceItems
                .Where(ii => ii.InvoiceItemGroupId == itemGroup.Id && ii.Id != request.Id)
                .OrderBy(ii => ii.SubSerialIndex) // Order by current index
                .ToListAsync();

            // Re-index remaining items to maintain sequential order
            for (int i = 0; i < remainingItems.Count; i++)
            {
                remainingItems[i].SubSerialIndex = i + 1; // Reset to 1, 2, 3...
            }

            // Recalculate group subtotal after item removal
            // Sum of remaining items + service log amount (if exists)
            itemGroup.SubTotal = remainingItems.Sum(ii => ii.Amount) +
                               (itemGroup.ServiceLog != null ? (decimal)itemGroup.ServiceLog.Amount : 0);

            // Recalculate total invoice amount
            invoice.Total = await _context.InvoiceItemGroups
                .Where(ig => ig.InvoiceId == invoice.Id)
                .SumAsync(ig => ig.SubTotal);


            // Recalculate total invoice amount after group removal
            invoice.DiscountAmount = invoice.Total * (invoice.DiscountRate / 100);

            invoice.TaxableAmount = invoice.Total - invoice.DiscountAmount;

            if (invoice.IsTaxIgnored)
            {
                invoice.TaxAmount = 0.0m;
                invoice.GrandTotal = invoice.TaxableAmount;
            }

            else

            {
                var taxAmount = Math.Round((invoice.TaxableAmount * (invoice.TaxRate / 100)), 3, MidpointRounding.AwayFromZero);
                invoice.TaxAmount = taxAmount;
                invoice.GrandTotal = Math.Round((invoice.TaxableAmount + taxAmount), 3, MidpointRounding.AwayFromZero);
            }


            var result = await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(result);

            //await _context.SaveChangesAsync();
            //await transaction.CommitAsync();

            //_logger.LogInformation($"Invoice item {invoiceItemId} deleted successfully");
            //return true;
        }
        catch (Exception ex)
        {
            return await Result<int>.FailureAsync($"Error deleting invoice {request.Id}");
        }
    }


}

