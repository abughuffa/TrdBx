
//using CleanArchitecture.Blazor.Application.Features.Invoices.Specifications;
//using CleanArchitecture.Blazor.Domain.Enums;

//namespace CleanArchitecture.Blazor.Application.Features.Invoices.Commands.Create;

//public class CreateRenewInvoiceCommand : IRequest<Result<Invoice>>
//{
//    [Description("Invoice")]
//    public required Invoice invoice { get; set; }
//}

//public class CreateRenewInvoiceCommandHandler : IRequestHandler<CreateRenewInvoiceCommand, Result<Invoice>>
//{
//    private readonly IApplicationDbContext _context;
//    public CreateRenewInvoiceCommandHandler(
//        IApplicationDbContext context)
//    {
//        _context = context;
//    }
//    public async Task<Result<Invoice>> Handle(CreateRenewInvoiceCommand request, CancellationToken cancellationToken)
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
//                               .ApplySpecification(new UnBilledRenewInvoiceByCustomerParentSpecification(request.invoice.CustomerId))
//                               .ToListAsync(cancellationToken);
//        }
//        else
//        {
//            //get invoice for customer
//            serviceLogs = await _context.ServiceLogs
//                               .Include(s => s.Subscriptions)
//                               .Include(s => s.Customer).Where(s => s.SerDate <= request.invoice.InvDate)
//                               .ApplySpecification(new UnBilledRenewInvoiceByCustomerChildSpecification(request.invoice.CustomerId))
//                               .ToListAsync(cancellationToken);
//        }

//        foreach (var sl in serviceLogs)
//        {

//            request.invoice.InvoiceItems?.Add(new InvoiceItem
//            {

//                ServiceLogId = sl.Id,
//                //ServiceLog = sl,
//                Amount = sl.Subscriptions is null ? sl.Amount : sl.Amount + sl.Subscriptions.Sum(s => s.Amount)
//            });
//            sl.IsBilled = true;
//            sl.AddDomainEvent(new ServiceLogUpdatedEvent(sl));

//        }

//        return await Result<Invoice>.SuccessAsync(request.invoice);
//    }
//}






