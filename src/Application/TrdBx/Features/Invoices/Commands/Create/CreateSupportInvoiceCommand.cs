
//using CleanArchitecture.Blazor.Application.Features.Invoices.Specifications;
//using CleanArchitecture.Blazor.Domain.Enums;

//namespace CleanArchitecture.Blazor.Application.Features.Invoices.Commands.Create;

//public class CreateSupportInvoiceCommand : IRequest<Result<Invoice>>
//{
//    [Description("Invoice")]
//    public required Invoice invoice { get; set; }
//}

//public class CreateSupportInvoiceCommandHandler : IRequestHandler<CreateSupportInvoiceCommand, Result<Invoice>>
//{
//    private readonly IApplicationDbContext _context;
//    public CreateSupportInvoiceCommandHandler(
//        IApplicationDbContext context)
//    {
//        _context = context;
//    }
//    public async Task<Result<Invoice>> Handle(CreateSupportInvoiceCommand request, CancellationToken cancellationToken)
//    {

//        List<ServiceLog> serviceLogs;

//        var customer = await _context.Customers.Where(c => c.Id == request.invoice.CustomerId).FirstAsync(cancellationToken);

//        request.invoice.IsTaxable = customer.IsTaxable;

//        if (customer.BillingPlan == BillingPlan.Advanced && customer.ParentId is null)
//        {
//            //get invoice for all customers of a client
//            serviceLogs = await _context.ServiceLogs
//                               .Include(s => s.Subscriptions)
//                               .Include(s => s.Customer).Where(s => s.SerDate <= request.invoice.InvDate)
//                               .ApplySpecification(new UnBilledSupportInvoiceByCustomerParentSpecification(request.invoice.CustomerId))
//                               .ToListAsync(cancellationToken);
//        }
//        else
//        {
//            //get invoice for customer
//            serviceLogs = await _context.ServiceLogs
//                               .Include(s => s.Subscriptions)
//                               .Include(s => s.Customer).Where(s => s.SerDate <= request.invoice.InvDate)
//                               .ApplySpecification(new UnBilledSupportInvoiceByCustomerChildSpecification(request.invoice.CustomerId))
//                               .ToListAsync(cancellationToken);
//        }

//        //if (serviceLogs == null) return await Result<null>.FailureAsync("There is no records for this invoice!");

//        request.invoice.IsTaxable = customer.IsTaxable;

//        foreach (var sl in serviceLogs)
//        {

//            request.invoice.InvoiceItems?.Add(new InvoiceItem
//            {

//                ServiceLogId = sl.Id,
//                Amount = sl.Subscriptions is null ? sl.Amount : sl.Amount + sl.Subscriptions.Sum(s => s.Amount)
//            });
//            sl.IsBilled = true;
//            sl.AddDomainEvent(new ServiceLogUpdatedEvent(sl));
//        }

//        return await Result<Invoice>.SuccessAsync(request.invoice);
//    }
//}






