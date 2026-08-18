

using CleanArchitecture.Blazor.Application.Features.Invoices.Caching;

using CleanArchitecture.Blazor.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Blazor.Application.Features.Invoices.Commands.AddPayment;

public class AddPaymentCommand : ICacheInvalidatorRequest<Result<int>>
{
    [Display(Name = "Id")]
    public int Id { get; set; }
    public decimal Amount { get; set; }
    [Display(Name = "PaymentDate")]
    public DateOnly PaymentDate { get; set; }


    public string CacheKey => InvoiceCacheKey.GetAllCacheKey;
    public IEnumerable<string> Tags => InvoiceCacheKey.Tags;

    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<AddPaymentCommand, Invoice>(MemberList.None);
    //        CreateMap<InvoiceDto, AddPaymentCommand>(MemberList.None);
    //    }
    //}

}

public class AddPaymentCommandHandler : IRequestHandler<AddPaymentCommand, Result<int>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public AddPaymentCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}

         private readonly IApplicationDbContextFactory _dbContextFactory;

    public AddPaymentCommandHandler(
        IApplicationDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;

    }
    public async ValueTask<Result<int>> Handle(AddPaymentCommand request, CancellationToken cancellationToken)
    {
        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);
        var item = await context.Invoices.FindAsync(request.Id, cancellationToken);

        if (item == null) return await Result<int>.FailureAsync("Invoice not found");


        if (item.IStatus != IStatus.Billed || item.IStatus != IStatus.PartaillyPaid)
        {
            return await Result<int>.FailureAsync($"Faild to add payment to Invoice with id: [{request.Id}].");
        }

        var OldBalanceDue = item.GrandTotal - item.PaidAmount;

        if (request.Amount > OldBalanceDue) return await Result<int>.FailureAsync("Payment exceeds balance due");


        item.PaidAmount += request.Amount;

        var CurrentBalanceDue = item.GrandTotal - item.PaidAmount;

        if (Math.Abs(CurrentBalanceDue) < 0.01m)
        {
            item.IStatus = Domain.Enums.IStatus.Paid;
            item.PaymentDate = request.PaymentDate;
        }
        else
            item.IStatus = Domain.Enums.IStatus.PartaillyPaid;


        // raise a update domain event
        item.AddDomainEvent(new InvoiceUpdatedEvent(item));
        await context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(item.Id);
    }
}

