using CleanArchitecture.Blazor.Application.Features.Invoices.Caching;
using CleanArchitecture.Blazor.Domain.Enums;
using CleanArchitecture.Blazor.Domain.Enums;
using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.Invoices.Commands.SetAsPaid;

public class SetAsPaidInvoiceCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")]
    public int Id { get; }
    public string CacheKey => InvoiceCacheKey.GetAllCacheKey;
     public IEnumerable<string> Tags => InvoiceCacheKey.Tags;
    public SetAsPaidInvoiceCommand(int id)
    {
        Id = id;
    }

}

public class SetAsPaidInvoiceCommandHandler : IRequestHandler<SetAsPaidInvoiceCommand, Result<int>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //public SetAsPaidInvoiceCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //}
    private readonly IApplicationDbContext _context;
    public SetAsPaidInvoiceCommandHandler(
      IApplicationDbContext context
  )
    {
        _context = context;
    }
    public async Task<Result<int>> Handle(SetAsPaidInvoiceCommand request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        var item = await _context.Invoices.FindAsync(request.Id, cancellationToken);
        if (item == null) return await Result<int>.FailureAsync("Invoice not found");

        if (!(item.IStatus != IStatus.Billed || item.IStatus != IStatus.Canceled))
        {
            return await Result<int>.FailureAsync($"Faild to set Invoice with id: [{request.Id}] as Paid.");
        }

        item.IStatus = IStatus.Paid;

        // raise a update domain event
        item.AddDomainEvent(new InvoiceUpdatedEvent(item));
        await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(item.Id);

    }
}

