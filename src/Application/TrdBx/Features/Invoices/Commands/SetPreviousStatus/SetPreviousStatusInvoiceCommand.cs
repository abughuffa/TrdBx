using CleanArchitecture.Blazor.Application.Features.Invoices.Caching;
using CleanArchitecture.Blazor.Domain.Enums;
using CleanArchitecture.Blazor.Domain.Enums;
using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.Invoices.Commands.SetPreviousStatus;

public class SetPreviousStatusInvoiceCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")]
    public int Id { get; }
    public string CacheKey => InvoiceCacheKey.GetAllCacheKey;
     public IEnumerable<string> Tags => InvoiceCacheKey.Tags;
    public SetPreviousStatusInvoiceCommand(int id)
    {
        Id = id;
    }

}

public class SetPreviousStatusInvoiceCommandHandler : IRequestHandler<SetPreviousStatusInvoiceCommand, Result<int>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //public SetPreviousStatusInvoiceCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //}

    private readonly IApplicationDbContext _context;
    public SetPreviousStatusInvoiceCommandHandler(
      IApplicationDbContext context
  )
    {
        _context = context;
    }
    public async Task<Result<int>> Handle(SetPreviousStatusInvoiceCommand request, CancellationToken cancellationToken)
    {

        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        var item = await _context.Invoices.FindAsync(request.Id, cancellationToken);
        if (item == null) return await Result<int>.FailureAsync("Invoice not found");

        if (!(item.IStatus != IStatus.SentToTax || item.IStatus != IStatus.Ready
            || item.IStatus != IStatus.Billed || item.IStatus != IStatus.Paid || item.IStatus != IStatus.Canceled))
        {
            return await Result<int>.FailureAsync($"Faild to set Invoice with id: [{request.Id}] to previous status.");
        }

        switch (item.IStatus)
        {
            case IStatus.SentToTax:
                {
                    item.IStatus = IStatus.Draft;
                    break;
                }
            case IStatus.Ready:
                {
                    if (item.IsTaxable)
                    {
                        return await Result<int>.FailureAsync($"Faild to set Invoice with id: [{request.Id}] to previous status.");
                    }
                    else
                    {
                        item.IStatus = IStatus.Draft;
                    }
                    break;
                }
            case IStatus.Billed:
                {
                    item.IStatus = IStatus.Ready;
                    break;
                }
            case IStatus.Paid:
                {
                    item.IStatus = IStatus.Billed;
                    break;
                }
            case IStatus.Canceled:
                {
                    item.IStatus = IStatus.Billed;
                    break;
                }
        }

        // raise a update domain event
        item.AddDomainEvent(new InvoiceUpdatedEvent(item));
        await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(item.Id);

    }
}

