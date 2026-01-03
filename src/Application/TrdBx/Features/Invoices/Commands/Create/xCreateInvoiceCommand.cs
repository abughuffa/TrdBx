using CleanArchitecture.Blazor.Application.Features.Common;
using CleanArchitecture.Blazor.Application.Features.Invoices.Caching;
using CleanArchitecture.Blazor.Application.Features.Invoices.Mappers;
using CleanArchitecture.Blazor.Application.Features.Invoices.Specifications;
using CleanArchitecture.Blazor.Domain.Enums;


namespace CleanArchitecture.Blazor.Application.Features.Invoices.Commands.Create;

public class xCreateInvoiceCommand : ICacheInvalidatorRequest<Result<int>>
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
    //        CreateMap<xCreateInvoiceCommand, Invoice>(MemberList.None);
    //    }
    //}
}

public class xCreateInvoiceCommandHandler : SerialForSharedLogic, IRequestHandler<xCreateInvoiceCommand, Result<int>>
{


    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public xCreateInvoiceCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    public xCreateInvoiceCommandHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }
    public async Task<Result<int>> Handle(xCreateInvoiceCommand request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        Result<Invoice>? result = null;

        //var _invoice = _mapper.Map<Invoice>(request);

        var _invoice = Mapper.FromxCreateCommand(request);

        _invoice.InvoiceType = request.InvoiceType;
        _invoice.InvDate = request.InvDate;
        _invoice.DueDate = request.InvDate.AddDays(30);
        _invoice.InvNo = GenSerialNo(_context, "Invoice", request.InvDate).Result;
        _invoice.IStatus = IStatus.Draft;
        _invoice.CustomerId = request.CustomerId;


        _invoice.InvoiceItems = [];
        //_invoice.ItemGruops = [];



        switch (request.InvoiceType)
        {
            case InvoiceType.Check:
                {
                    _invoice.InvDesc = "فاتورة فحص فني";
                    result = await CreateCheckInvoice(_context, _invoice, cancellationToken);
                    //result = await Mediator.Send(new CreateCheckInvoiceCommand() { invoice = _invoice });
                    break;
                }
            case InvoiceType.Install:
                {
                    _invoice.InvDesc = "فاتورة سداد وحدات تتبع";
                    result = await CreatePaymentInvoice(_context, _invoice, cancellationToken);
                    //result = await Mediator.Send(new CreatePaymentInvoiceCommand() { invoice = _invoice });
                    break;
                }
            case InvoiceType.Replace:
                {
                    _invoice.InvDesc = "فاتورة استبدال وحدات";
                    result = await CreateReplaceInvoice(_context, _invoice, cancellationToken);
                    //result = await Mediator.Send(new CreateReplaceInvoiceCommand() { invoice = _invoice });
                    break;
                }
            case InvoiceType.Renew:
                {
                    _invoice.InvDesc = "فاتورة تجديد اشتراك";
                    result = await CreateRenewInvoice(_context, _invoice, cancellationToken);
                    //result = await Mediator.Send(new CreateRenewInvoiceCommand() { invoice = _invoice });
                    break;
                }
            case InvoiceType.Subscription:
                {
                    _invoice.InvDesc = "فاتورة تفعيل / إلغاء تفعيل الاشتراك";
                    result = await CreateSubscriptionInvoice(_context, _invoice, cancellationToken);
                    //result = await Mediator.Send(new CreateSubscriptionInvoiceCommand() { invoice = _invoice });
                    break;
                }
            case InvoiceType.Support:
                {
                    _invoice.InvDesc = "فاتورة اعمال دعم فني";
                    result = await CreateSupportInvoice(_context, _invoice, cancellationToken);
                    //result = await Mediator.Send(new CreateSupportInvoiceCommand() { invoice = _invoice });
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
            var total = _invoice.InvoiceItems.Sum(ii => ii.Amount);
            //var total = _invoice.ItemGroups.Sum(ii => ii.Amount);

            _invoice.Total = total;

            if (request.IgnoreTaxes)
            {
                _invoice.Taxes = 0.0m;
                _invoice.GrangTotal = total;
            }

            else

            {
                _invoice.Taxes = Math.Round(total / 100, 3, MidpointRounding.AwayFromZero);
                _invoice.GrangTotal = Math.Round(total + total / 100, 3, MidpointRounding.AwayFromZero);
            }

            _invoice.AddDomainEvent(new InvoiceCreatedEvent(_invoice));
            _context.Invoices.Add(_invoice);
            await _context.SaveChangesAsync(cancellationToken);

            return await Result<int>.SuccessAsync(_invoice.Id);
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



