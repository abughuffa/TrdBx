using CleanArchitecture.Blazor.Application.Features.Invoices.Caching;
using System.ComponentModel.DataAnnotations;
namespace CleanArchitecture.Blazor.Application.Features.Invoices.Commands.Update;

public class UpdateInvoiceCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Display(Name = "Id")]
    public int Id { get; set; }
    public string DisplayCusName { get; set; } = string.Empty;



    public string CacheKey => InvoiceCacheKey.GetAllCacheKey;
    public IEnumerable<string> Tags => InvoiceCacheKey.Tags;

    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<UpdateInvoiceCommand, Invoice>(MemberList.None);
    //        CreateMap<InvoiceDto, UpdateInvoiceCommand>(MemberList.None);
    //    }
    //}

}

public class UpdateInvoiceCommandHandler : IRequestHandler<UpdateInvoiceCommand, Result<int>>
{
         private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly IObjectMapper _objectMapper;
    public UpdateInvoiceCommandHandler(
        IObjectMapper objectMapper,
        IApplicationDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
        _objectMapper = objectMapper;
    }
    public async ValueTask<Result<int>> Handle(UpdateInvoiceCommand request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);


        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);

        var item = await context.Invoices.FindAsync(request.Id, cancellationToken);
        if (item == null) return await Result<int>.FailureAsync("Invoice not found");


        item = _objectMapper.Map(request, item);


        // raise a update domain event
        item.AddDomainEvent(new InvoiceUpdatedEvent(item));
        await context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(item.Id);
    }
}

