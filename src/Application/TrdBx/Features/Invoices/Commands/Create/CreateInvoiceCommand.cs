using CleanArchitecture.Blazor.Application.Features.Common;
using CleanArchitecture.Blazor.Application.Features.Invoices.Caching;
using CleanArchitecture.Blazor.Application.Features.Invoices.Mappers;
using CleanArchitecture.Blazor.Application.Features.Invoices.Specifications;
using CleanArchitecture.Blazor.Domain.Enums;


namespace CleanArchitecture.Blazor.Application.Features.Invoices.Commands.Create;

public class CreateInvoiceCommand : ICacheInvalidatorRequest<Result<int>>
{

    [Description("InvDate")]
    public DateOnly InvDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    [Description("CustomerId")]
    public int CustomerId { get; set; }

    [Description("InvoiceType")]
    public InvoiceType InvoiceType { get; set; }

    [Description("IgnoreTaxes")]
    public bool IgnoreTaxes { get; set; } = false;



    public string CacheKey => InvoiceCacheKey.GetAllCacheKey;
     public IEnumerable<string> Tags => InvoiceCacheKey.Tags;
    //private class Mapping : Profile
    //{
    //    public Mapping()
    //    {
    //        CreateMap<CreateInvoiceCommand, Invoice>(MemberList.None);
    //    }
    //}
}

public class CreateInvoiceCommandHandler : SerialForSharedLogic, IRequestHandler<CreateInvoiceCommand, Result<int>>
{


    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public CreateInvoiceCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    public CreateInvoiceCommandHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }
    public async Task<Result<int>> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        Result<Invoice>? result = null;

        //var item = _mapper.Map<Invoice>(request);

        var item = Mapper.FromCreateCommand(request);

        item.InvoiceType = request.InvoiceType;
        item.InvDate = request.InvDate;
        item.DueDate = request.InvDate.AddDays(30);
        item.InvNo = GenSerialNo(_context, "Invoice", request.InvDate).Result;
        item.IStatus = IStatus.Draft;
        item.CustomerId = request.CustomerId;


        item.InvoiceItems = [];

        

        switch (request.InvoiceType)
        {
            case InvoiceType.Check:
                {
                    item.InvDesc = "فاتورة فحص فني";
                    result = await CreateCheckInvoice(_context, item, cancellationToken);
                    //result = await Mediator.Send(new CreateCheckInvoiceCommand() { invoice = item });
                    break;
                }
            case InvoiceType.Install:
                {
                    item.InvDesc = "فاتورة سداد وحدات تتبع";
                    result = await CreatePaymentInvoice(_context, item, cancellationToken);
                    //result = await Mediator.Send(new CreatePaymentInvoiceCommand() { invoice = item });
                    break;
                }
            case InvoiceType.Replace:
                {
                    item.InvDesc = "فاتورة استبدال وحدات";
                    result = await CreateReplaceInvoice(_context, item, cancellationToken);
                    //result = await Mediator.Send(new CreateReplaceInvoiceCommand() { invoice = item });
                    break;
                }
            case InvoiceType.Renew:
                {
                    item.InvDesc = "فاتورة تجديد اشتراك";
                    result = await CreateRenewInvoice(_context, item, cancellationToken);
                    //result = await Mediator.Send(new CreateRenewInvoiceCommand() { invoice = item });
                    break;
                }
            case InvoiceType.Subscription:
                {
                    item.InvDesc = "فاتورة تفعيل / إلغاء تفعيل الاشتراك";
                    result = await CreateSubscriptionInvoice(_context, item, cancellationToken);
                    //result = await Mediator.Send(new CreateSubscriptionInvoiceCommand() { invoice = item });
                    break;
                }
            case InvoiceType.Support:
                {
                    item.InvDesc = "فاتورة اعمال دعم فني";
                    result = await CreateSupportInvoice(_context, item, cancellationToken);
                    //result = await Mediator.Send(new CreateSupportInvoiceCommand() { invoice = item });
                    break;
                }

        }

        //CC:  IsTaxable     =>    Invoice:   IgnoreTaxes

        //       False                          False           ==> To = x, Tx = 0, GT = To             Invoice: IsTaxable = False           
        //       False                           True           ==> To = x, Tx = 0, GT = To             Invoice: IsTaxable = False   
        //       True                           False           ==> To = x, Tx = x/100, GT = To + Tx    Invoice: IsTaxable = true
        //       True                            True           ==> To = x, Tx = 0, GT = To             Invoice: IsTaxable = true    


        if (result.Succeeded && result.Data is not null)
        {
            var total = item.InvoiceItems.Sum(ii => ii.Amount);

            item.Total = total;

            if (request.IgnoreTaxes)
            {
                item.Taxes = 0.0m;
                item.GrangTotal = total;
            }

            else

            {
                item.Taxes = Math.Round(total / 100, 3, MidpointRounding.AwayFromZero);
                item.GrangTotal = Math.Round(total + total / 100, 3, MidpointRounding.AwayFromZero);
            }

            item.AddDomainEvent(new InvoiceCreatedEvent(item));
            _context.Invoices.Add(item);
            await _context.SaveChangesAsync(cancellationToken);

            return await Result<int>.SuccessAsync(item.Id);
        }
        else
        {
            return await Result<int>.FailureAsync(result.Errors);
        }

    }



    private static async Task<Result<Invoice>> CreateCheckInvoice(IApplicationDbContext cnx, Invoice invoice,CancellationToken cancellationToken)
    {

        List<ServiceLog> serviceLogs;

        var customer = await cnx.Customers.Where(c => c.Id == invoice.CustomerId).FirstAsync(cancellationToken);

        invoice.IsTaxable = customer.IsTaxable;

        if (customer.BillingPlan == BillingPlan.Advanced && customer.ParentId is null)
        {
            //get invoice for all customers of a client
            serviceLogs = await cnx.ServiceLogs
                               .Include(s => s.Subscriptions)
                               .Include(s => s.Customer).Where(s => s.SerDate <= invoice.InvDate)
                               .ApplySpecification(new UnBilledCheckInvoiceByCustomerParentSpecification(invoice.CustomerId))
                               .ToListAsync(cancellationToken);
        }
        else
        {
            //get invoice for customer
            serviceLogs = await cnx.ServiceLogs
                               .Include(s => s.Subscriptions)
                               .Include(s => s.Customer).Where(s => s.SerDate <= invoice.InvDate)
                               .ApplySpecification(new UnBilledCheckInvoiceByCustomerChildSpecification(invoice.CustomerId))
                               .ToListAsync(cancellationToken);
        }



        foreach (var sl in serviceLogs)
        {

            invoice.InvoiceItems?.Add(new InvoiceItem
            {

                ServiceLogId = sl.Id,
                Amount = sl.Amount
            });
            sl.IsBilled = true;
            sl.AddDomainEvent(new ServiceLogUpdatedEvent(sl));

        }

        return await Result<Invoice>.SuccessAsync(invoice);
    }
    private static async Task<Result<Invoice>> CreatePaymentInvoice(IApplicationDbContext cnx, Invoice invoice, CancellationToken cancellationToken)
    {
        List<ServiceLog> serviceLogs;

        var customer = await cnx.Customers.Where(c => c.Id == invoice.CustomerId).FirstAsync(cancellationToken);

        invoice.IsTaxable = customer.IsTaxable;

        if (customer.BillingPlan == BillingPlan.Advanced && customer.ParentId is null)
        {
            //get invoice for all customers of a client
            serviceLogs = await cnx.ServiceLogs
                               .Include(s => s.Subscriptions)
                               .Include(s => s.Customer).Where(s => s.SerDate <= invoice.InvDate)
                               .ApplySpecification(new UnBilledPaymentInvoiceByCustomerParentSpecification(invoice.CustomerId))
                               .ToListAsync(cancellationToken);
        }
        else
        {
            //get invoice for customer
            serviceLogs = await cnx.ServiceLogs
                               .Include(s => s.Subscriptions)
                               .Include(s => s.Customer).Where(s => s.SerDate <= invoice.InvDate)
                               .ApplySpecification(new UnBilledPaymentInvoiceByCustomerChildSpecification(invoice.CustomerId))
                               .ToListAsync(cancellationToken);
        }


        foreach (var sl in serviceLogs)
        {
            invoice.InvoiceItems?.Add(new InvoiceItem
            {

                ServiceLogId = sl.Id,
                Amount = sl.Subscriptions is null ? sl.Amount : sl.Amount + sl.Subscriptions.Sum(s => s.Amount)
            });
            sl.IsBilled = true;
            sl.AddDomainEvent(new ServiceLogUpdatedEvent(sl));
        }

        return await Result<Invoice>.SuccessAsync(invoice);
    }
    private static async Task<Result<Invoice>> CreateReplaceInvoice(IApplicationDbContext cnx, Invoice invoice, CancellationToken cancellationToken)
    {
        List<ServiceLog> serviceLogs;

        var customer = await cnx.Customers.Where(c => c.Id == invoice.CustomerId).FirstAsync(cancellationToken);

        invoice.IsTaxable = customer.IsTaxable;

        if (customer.BillingPlan == BillingPlan.Advanced && customer.ParentId is null)
        {
            //get invoice for all customers of a client
            serviceLogs = await cnx.ServiceLogs
                               .Include(s => s.Subscriptions)
                               .Include(s => s.Customer).Where(s => s.SerDate <= invoice.InvDate)
                               .ApplySpecification(new UnBilledReplaceInvoiceByCustomerParentSpecification(invoice.CustomerId))
                               .ToListAsync(cancellationToken);
        }
        else
        {
            //get invoice for customer
            serviceLogs = await cnx.ServiceLogs
                               .Include(s => s.Subscriptions)
                               .Include(s => s.Customer).Where(s => s.SerDate <= invoice.InvDate)
                               .ApplySpecification(new UnBilledReplaceInvoiceByCustomerChildSpecification(invoice.CustomerId))
                               .ToListAsync(cancellationToken);
        }

        foreach (var sl in serviceLogs)
        {

            invoice.InvoiceItems?.Add(new InvoiceItem
            {

                ServiceLogId = sl.Id,
                //ServiceLog = sl,
                Amount = sl.Subscriptions is null ? sl.Amount : sl.Amount + sl.Subscriptions.Sum(s => s.Amount)
            });
            sl.IsBilled = true;
            sl.AddDomainEvent(new ServiceLogUpdatedEvent(sl));

        }
        return await Result<Invoice>.SuccessAsync(invoice);
    }
    private static async Task<Result<Invoice>> CreateRenewInvoice(IApplicationDbContext cnx, Invoice invoice, CancellationToken cancellationToken)
    {
        List<ServiceLog> serviceLogs;

        var customer = await cnx.Customers.Where(c => c.Id == invoice.CustomerId).FirstAsync(cancellationToken);

        invoice.IsTaxable = customer.IsTaxable;

        if (customer.BillingPlan == BillingPlan.Advanced && customer.ParentId is null)
        {
            //get invoice for all customers of a client
            serviceLogs = await cnx.ServiceLogs
                               .Include(s => s.Subscriptions)
                               .Include(s => s.Customer).Where(s => s.SerDate <= invoice.InvDate)
                               .ApplySpecification(new UnBilledRenewInvoiceByCustomerParentSpecification(invoice.CustomerId))
                               .ToListAsync(cancellationToken);
        }
        else
        {
            //get invoice for customer
            serviceLogs = await cnx.ServiceLogs
                               .Include(s => s.Subscriptions)
                               .Include(s => s.Customer).Where(s => s.SerDate <= invoice.InvDate)
                               .ApplySpecification(new UnBilledRenewInvoiceByCustomerChildSpecification(invoice.CustomerId))
                               .ToListAsync(cancellationToken);
        }

        foreach (var sl in serviceLogs)
        {

            invoice.InvoiceItems?.Add(new InvoiceItem
            {

                ServiceLogId = sl.Id,
                //ServiceLog = sl,
                Amount = sl.Subscriptions is null ? sl.Amount : sl.Amount + sl.Subscriptions.Sum(s => s.Amount)
            });
            sl.IsBilled = true;
            sl.AddDomainEvent(new ServiceLogUpdatedEvent(sl));

        }

        return await Result<Invoice>.SuccessAsync(invoice);
    }
    private static async Task<Result<Invoice>> CreateSubscriptionInvoice(IApplicationDbContext cnx, Invoice invoice, CancellationToken cancellationToken)
    {
        List<ServiceLog> serviceLogs;

        var customer = await cnx.Customers.Where(c => c.Id == invoice.CustomerId).FirstAsync(cancellationToken);

        invoice.IsTaxable = customer.IsTaxable;

        if (customer.BillingPlan == BillingPlan.Advanced && customer.ParentId is null)
        {
            //get invoice for all customers of a client
            serviceLogs = await cnx.ServiceLogs
                               .Include(s => s.Subscriptions)
                               .Include(s => s.Customer).Where(s => s.SerDate <= invoice.InvDate)
                               .ApplySpecification(new UnBilledSubscriptionInvoiceByCustomerParentSpecification(invoice.CustomerId))
                               .ToListAsync(cancellationToken);
        }
        else
        {
            //get invoice for customer
            serviceLogs = await cnx.ServiceLogs
                               .Include(s => s.Subscriptions)
                               .Include(s => s.Customer).Where(s => s.SerDate <= invoice.InvDate)
                               .ApplySpecification(new UnBilledSubscriptionInvoiceByCustomerChildSpecification(invoice.CustomerId))
                               .ToListAsync(cancellationToken);
        }


        foreach (var sl in serviceLogs)
        {
            invoice.InvoiceItems?.Add(new InvoiceItem
            {
                ServiceLogId = sl.Id,
                Amount = sl.Subscriptions is null ? sl.Amount : sl.Amount + sl.Subscriptions.Sum(s => s.Amount)
            });

            sl.IsBilled = true;
            sl.AddDomainEvent(new ServiceLogUpdatedEvent(sl));
        }

        return await Result<Invoice>.SuccessAsync(invoice);
    }
    private static async Task<Result<Invoice>> CreateSupportInvoice(IApplicationDbContext cnx, Invoice invoice, CancellationToken cancellationToken)
    {
        List<ServiceLog> serviceLogs;

        var customer = await cnx.Customers.Where(c => c.Id == invoice.CustomerId).FirstAsync(cancellationToken);

        invoice.IsTaxable = customer.IsTaxable;

        if (customer.BillingPlan == BillingPlan.Advanced && customer.ParentId is null)
        {
            //get invoice for all customers of a client
            serviceLogs = await cnx.ServiceLogs
                               .Include(s => s.Subscriptions)
                               .Include(s => s.Customer).Where(s => s.SerDate <= invoice.InvDate)
                               .ApplySpecification(new UnBilledSupportInvoiceByCustomerParentSpecification(invoice.CustomerId))
                               .ToListAsync(cancellationToken);
        }
        else
        {
            //get invoice for customer
            serviceLogs = await cnx.ServiceLogs
                               .Include(s => s.Subscriptions)
                               .Include(s => s.Customer).Where(s => s.SerDate <= invoice.InvDate)
                               .ApplySpecification(new UnBilledSupportInvoiceByCustomerChildSpecification(invoice.CustomerId))
                               .ToListAsync(cancellationToken);
        }

        invoice.IsTaxable = customer.IsTaxable;

        foreach (var sl in serviceLogs)
        {

            invoice.InvoiceItems?.Add(new InvoiceItem
            {

                ServiceLogId = sl.Id,
                Amount = sl.Subscriptions is null ? sl.Amount : sl.Amount + sl.Subscriptions.Sum(s => s.Amount)
            });
            sl.IsBilled = true;
            sl.AddDomainEvent(new ServiceLogUpdatedEvent(sl));
        }

        return await Result<Invoice>.SuccessAsync(invoice);
    }
}



