////using CleanArchitecture.Blazor.Application.Features.Invoices.Specifications;

//using CleanArchitecture.Blazor.Application.Features.Invoices.Specifications;
//using CleanArchitecture.Blazor.Domain.Enums;

//namespace CleanArchitecture.Blazor.Application.Features.Invoices.Commands.Create;

//public class CreateCheckInvoiceCommand : IRequest<Result<Invoice>>
//{
//    [Description("Invoice")]
//    public required Invoice invoice { get; set; }
//}

//public class CreateCheckInvoiceCommandHandler : IRequestHandler<CreateCheckInvoiceCommand, Result<Invoice>>
//{
//    private readonly IApplicationDbContext _context;
//    public CreateCheckInvoiceCommandHandler(
//        IApplicationDbContext context)
//    {
//        _context = context;
//    }
//    public async Task<Result<Invoice>> Handle(CreateCheckInvoiceCommand request, CancellationToken cancellationToken)
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
//                               .ApplySpecification(new UnBilledCheckInvoiceByCustomerParentSpecification(request.invoice.CustomerId))
//                               .ToListAsync(cancellationToken);
//        }
//        else
//        {
//            //get invoice for customer
//            serviceLogs = await _context.ServiceLogs
//                               .Include(s => s.Subscriptions)
//                               .Include(s => s.Customer).Where(s => s.SerDate <= request.invoice.InvDate)
//                               .ApplySpecification(new UnBilledCheckInvoiceByCustomerChildSpecification(request.invoice.CustomerId))
//                               .ToListAsync(cancellationToken);
//        }



//        foreach (var sl in serviceLogs)
//        {

//            request.invoice.InvoiceItems?.Add(new InvoiceItem
//            {

//                ServiceLogId = sl.Id,
//                Amount = sl.Amount
//            });
//            sl.IsBilled = true;
//            sl.AddDomainEvent(new ServiceLogUpdatedEvent(sl));

//        }

//        return await Result<Invoice>.SuccessAsync(request.invoice);
//    }
//}






