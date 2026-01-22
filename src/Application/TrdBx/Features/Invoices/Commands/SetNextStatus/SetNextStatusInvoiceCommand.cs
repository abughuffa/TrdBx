using CleanArchitecture.Blazor.Application.Features.Invoices.Caching;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.Invoices.Commands.SetNextStatus;

public class SetNextStatusInvoiceCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")]
    public int Id { get; }
    public string CacheKey => InvoiceCacheKey.GetAllCacheKey;
     public IEnumerable<string> Tags => InvoiceCacheKey.Tags;
    public SetNextStatusInvoiceCommand(int id)
    {
        Id = id;
    }



}

public class SetNextStatusInvoiceCommandHandler : IRequestHandler<SetNextStatusInvoiceCommand, Result<int>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //public SetNextStatusInvoiceCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //}

    private readonly IApplicationDbContext _context;
    public SetNextStatusInvoiceCommandHandler(
      IApplicationDbContext context
  )
    {
        _context = context;
    }
    public async Task<Result<int>> Handle(SetNextStatusInvoiceCommand request, CancellationToken cancellationToken)
    {

        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        var item = await _context.Invoices.FindAsync(request.Id, cancellationToken);
        if (item == null) return await Result<int>.FailureAsync("Invoice not found");

        if (!(item.IStatus != IStatus.Draft || item.IStatus != IStatus.SentToTax || item.IStatus != IStatus.Ready || item.IStatus != IStatus.PartaillyPaid || item.IStatus != IStatus.Paid))
        {
            return await Result<int>.FailureAsync($"Faild to set Invoice with id: [{request.Id}] to next status.");
        }

        switch (item.IStatus)
        {
            case IStatus.Draft:
                {
                    if (item.IsTaxable)
                    {
                        item.IStatus = IStatus.SentToTax;
                    }
                    else
                    {
                        item.IStatus = IStatus.Ready;
                    }
                    break;
                }
            case IStatus.SentToTax:
                {
                    item.IStatus = IStatus.Ready;
                    break;
                }
            case IStatus.Ready:
                {
                    item.IStatus = IStatus.Billed;
                    break;
                }
            default:
                {
                    return null;
                }
        }

        // raise a update domain event
        item.AddDomainEvent(new InvoiceUpdatedEvent(item));
        await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(item.Id);
    }
}

