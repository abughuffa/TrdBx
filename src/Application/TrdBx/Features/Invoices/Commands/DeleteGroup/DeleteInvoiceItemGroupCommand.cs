using CleanArchitecture.Blazor.Application.Features.Invoices.Caching;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Enums;


namespace CleanArchitecture.Blazor.Application.Features.Invoices.Commands.Delete;

public class DeleteInvoiceItemGroupCommand : ICacheInvalidatorRequest<Result<int>>
{
    public int Id { get; }
    public IEnumerable<string> Tags => InvoiceCacheKey.Tags;
    public DeleteInvoiceItemGroupCommand(int id)
    {
        Id = id;
    }
}

public class DeleteInvoiceItemGroupCommandHandler :
             IRequestHandler<DeleteInvoiceItemGroupCommand, Result<int>>

{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //public DeleteInvoiceCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //}

    private readonly IApplicationDbContext _context;
    public DeleteInvoiceItemGroupCommandHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }
    public async Task<Result<int>> Handle(DeleteInvoiceItemGroupCommand request, CancellationToken cancellationToken)
    {
        


        try
        {
            // Load item group with all related data
            var itemGroup = await _context.InvoiceItemGroups
                .Include(ig => ig.Invoice) // Load parent invoice
                .Include(ig => ig.ServiceLog) // Load related service log
                .Include(ig => ig.InvoiceItems) // Load all items in group
                .FirstOrDefaultAsync(ig => ig.Id == request.Id);

            if (itemGroup == null) await Result<int>.FailureAsync($"Item group with ID {request.Id} not found.");


            if (itemGroup.Invoice.IStatus == IStatus.SentToTax
               || itemGroup.Invoice.IsTaxable && (itemGroup.Invoice.IStatus == IStatus.Ready
                                   || itemGroup.Invoice.IStatus == IStatus.Billed
                                   || itemGroup.Invoice.IStatus == IStatus.Canceled)
                          || itemGroup.Invoice.IStatus == IStatus.PartaillyPaid
                          || itemGroup.Invoice.IStatus == IStatus.Paid)
            {
                return await Result<int>.FailureAsync($"Faild to delete Item group with id: [{request.Id}].");
            }

            var invoice = itemGroup.Invoice;

            // Mark service log as unbilled since group is being removed
            if (itemGroup.ServiceLog != null)
            {
                itemGroup.ServiceLog.IsBilled = false; // Reset billing status
            }

            // Remove all items in the group (child entities first)
            _context.InvoiceItems.RemoveRange(itemGroup.InvoiceItems);


            itemGroup.AddDomainEvent(new InvoiceItemGroupDeletedEvent(itemGroup));

            _context.InvoiceItemGroups.Remove(itemGroup); // Remove the group itself

            // Get remaining groups in the invoice
            var remainingGroups = await _context.InvoiceItemGroups
                .Where(ig => ig.InvoiceId == invoice.Id && ig.Id != request.Id)
                .OrderBy(ig => ig.SerialIndex) // Order by current index
                .ToListAsync();

            // Re-index remaining groups to maintain sequential order
            for (int i = 0; i < remainingGroups.Count; i++)
            {
                remainingGroups[i].SerialIndex = i + 1; // Reset to 1, 2, 3...
            }

            // Recalculate total invoice amount after group removal
            invoice.Total = remainingGroups.Sum(ig => ig.SubTotal);

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



            var result = await _context.SaveChangesAsync(cancellationToken); // Save all deletions
            return await Result<int>.SuccessAsync(result);
        }
        catch (Exception ex)
        {

            //var result = await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.FailureAsync($"Error deleting item group {request.Id}");

        }



    }


}

