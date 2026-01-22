using CleanArchitecture.Blazor.Application.Features.Invoices.Caching;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Enums;


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
        
        try
        {
            // Load invoice with all related data for cleanup
            var invoice = await _context.Invoices
                .Include(i => i.InvoiceItemGroups) // Load item groups
                    .ThenInclude(ig => ig.ServiceLog) // Load service log for each group
                .Include(i => i.InvoiceItemGroups)
                    .ThenInclude(ig => ig.InvoiceItems) // Load items in each group
                .FirstOrDefaultAsync(i => i.Id == request.Id);

            if (invoice == null) return await Result<int>.FailureAsync($"Invoice with ID {request.Id} not found.");

       

            if (invoice.IStatus == IStatus.SentToTax
                          || invoice.IsTaxable && (invoice.IStatus == IStatus.Ready
                                              || invoice.IStatus == IStatus.Billed
                                              || invoice.IStatus == IStatus.Canceled)
                          || invoice.IStatus == IStatus.PartaillyPaid
                          || invoice.IStatus == IStatus.Paid)
            {
                return await Result<int>.FailureAsync($"Faild to delete Invoice with id: [{request.Id}].");
            }

            // Before deleting invoice, mark all service logs as unbilled
            // This allows them to be included in future invoices
            foreach (var itemGroup in invoice.InvoiceItemGroups)
            {
                if (itemGroup.ServiceLog != null)
                {
                    itemGroup.ServiceLog.IsBilled = false; // Reset billing status
                }
            }

            // Remove all related entities in correct order (child-first)
            foreach (var itemGroup in invoice.InvoiceItemGroups)
            {
                _context.InvoiceItems.RemoveRange(itemGroup.InvoiceItems); // Delete items first
            }
            _context.InvoiceItemGroups.RemoveRange(invoice.InvoiceItemGroups); // Delete item groups


            invoice.AddDomainEvent(new InvoiceDeletedEvent(invoice));

            _context.Invoices.Remove(invoice); // Delete invoice



            //await _context.SaveChangesAsync(); 
            //await transaction.CommitAsync(); // Commit transaction

            //_logger.LogInformation($"Invoice {invoice.InvoiceNo} deleted successfully");
            var result = await _context.SaveChangesAsync(cancellationToken); // Save all deletions
            return await Result<int>.SuccessAsync(result);
        }
        catch (Exception ex)
        {
            //await transaction.RollbackAsync(); // Rollback on error

            return await Result<int>.FailureAsync($"Error deleting invoice {request.Id}");
            //_logger.LogError(ex, $"Error deleting invoice {invoiceId}");
            //throw;
        }
    }


}

