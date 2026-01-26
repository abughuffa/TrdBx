using CleanArchitecture.Blazor.Application.Features.Common;
using CleanArchitecture.Blazor.Application.Features.ServiceLogs.Specifications;
using CleanArchitecture.Blazor.Application.Features.Invoices.Caching;
using CleanArchitecture.Blazor.Application.Features.Invoices.DTOs;
using CleanArchitecture.Blazor.Application.Features.Invoices.Helper;
using CleanArchitecture.Blazor.Application.Features.Invoices.Mappers;
using CleanArchitecture.Blazor.Domain.Entities;
//using CleanArchitecture.Blazor.Application.Features.Invoices.Specifications;
using CleanArchitecture.Blazor.Domain.Enums;


namespace CleanArchitecture.Blazor.Application.Features.Invoices.Commands.Create;

public class CreateInvoiceCommand : ICacheInvalidatorRequest<Result<int>>
{

    [Description("InvoiceDate")]
    public DateOnly InvoiceDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    [Description("CustomerId")]
    public int CustomerId { get; set; }
    [Description("InvoiceType")]
    public InvoiceType InvoiceType { get; set; }
    [Description("Discount Rate")]
    public decimal DiscountRate { get; set; } = 0.0m;
    [Description("Tax Rate")]
    public decimal TaxRate { get; set; } = 1.0m;
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

        var customer = await _context.Customers.Where(c => c.Id == request.CustomerId).Include(c => c.Parent).FirstAsync(cancellationToken);

        // Validate customer exists in database
        if (customer == null) throw new ArgumentException($"Customer with ID {request.CustomerId} not found.");

        List<ServiceLog> serviceLogs = [];

        var invoiceNo = await GenSerialNo(_context, "Invoice", request.InvoiceDate); // Generate unique invoice number

        // Create new invoice instance with basic information
        var invoice = new InvoiceDto()
        {
            DisplayCusName = customer.Parent is null ? customer.Name : $"{customer.Parent.Name}-{customer.Name}",
            CustomerId = request.CustomerId,
            InvoiceType = request.InvoiceType,
            InvoiceDate = request.InvoiceDate,
            DiscountRate = request.DiscountRate,
            TaxRate = request.TaxRate,
            IsTaxIgnored = request.IgnoreTaxes,
            DueDate = request.InvoiceDate.AddDays(30),
            InvoiceNo = invoiceNo, // Generate unique invoice number
            IStatus = IStatus.Draft, // Default status for new invoices
            InvoiceItemGroups = []
        };

        switch (request.InvoiceType)
        {
            case InvoiceType.Check:
                {
                   
                    //var customer = await _context.Customers.Where(c => c.Id == request.CustomerId).FirstAsync(cancellationToken);

                    if (customer.BillingPlan == BillingPlan.Advanced && customer.ParentId is null)
                    {
                        // Get all unbilled servicelogs for for all child customers of a parent
                        serviceLogs = await _context.ServiceLogs.Include(sl => sl.Subscriptions) // Eager load Subscriptions collection
                                           .Where(s => s.SerDate <= request.InvoiceDate)
                                           .ApplySpecification(new UnBilledCheckInvoiceByCustomerParentSpecification(request.CustomerId))
                                           .OrderBy(sl => sl.Id)
                                           .ToListAsync(cancellationToken);
                    }
                    else
                    {
                        //get all unbilled servicelogs for a child customer
                        serviceLogs = await _context.ServiceLogs.Include(sl => sl.Subscriptions) // Eager load Subscriptions collection
                                           .Where(s => s.SerDate <= request.InvoiceDate)
                                           .ApplySpecification(new UnBilledCheckInvoiceByCustomerChildSpecification(request.CustomerId))
                                           .OrderBy(sl => sl.Id)
                                           .ToListAsync(cancellationToken);
                    }

                    // Validate serviceLogs exists in database
                    if (!serviceLogs.Any()) return await Result<int>.FailureAsync("No unbilled service logs found for the specified criteria.");



                    // Add Invoice Desc as basic information
                    invoice.IsTaxable = customer.IsTaxable;
                    invoice.Description = "فاتورة فحص فني";
                    var xresult =  await GenerateInvoiceAsync(_context, invoice, serviceLogs,request.IgnoreTaxes, cancellationToken);
                    return await Result<int>.SuccessAsync(xresult);
                }
            case InvoiceType.Install:
                {
                    //var customer = await _context.Customers.Where(c => c.Id == request.CustomerId).FirstAsync(cancellationToken);

                    if (customer.BillingPlan == BillingPlan.Advanced && customer.ParentId is null)
                    {
                        // Get all unbilled servicelogs for for all child customers of a parent
                        serviceLogs = await _context.ServiceLogs
                                           .Include(s => s.Subscriptions)
                                           .Include(s => s.Customer).Where(s => s.SerDate <= request.InvoiceDate)
                                           .ApplySpecification(new UnBilledPaymentInvoiceByCustomerParentSpecification(request.CustomerId))
                                           .ToListAsync(cancellationToken);
                    }
                    else
                    {
                        //get all unbilled servicelogs for a child customer
                        serviceLogs = await _context.ServiceLogs
                                           .Include(s => s.Subscriptions)
                                           .Include(s => s.Customer).Where(s => s.SerDate <= request.InvoiceDate)
                                           .ApplySpecification(new UnBilledPaymentInvoiceByCustomerChildSpecification(request.CustomerId))
                                           .ToListAsync(cancellationToken);
                    }
                    
                    // Validate serviceLogs exists in database
                    if (!serviceLogs.Any()) return await Result<int>.FailureAsync("No unbilled service logs found for the specified criteria.");

                 

                    // Add Invoice Desc as basic information
                    invoice.IsTaxable = customer.IsTaxable;
                    invoice.Description = "فاتورة سداد وحدات تتبع";
                    var xresult = await GenerateInvoiceAsync(_context, invoice, serviceLogs, request.IgnoreTaxes, cancellationToken);
                    return await Result<int>.SuccessAsync(xresult);
                }
            case InvoiceType.Replace:
                {
                    //var customer = await _context.Customers.Where(c => c.Id == request.CustomerId).FirstAsync(cancellationToken);

                    if (customer.BillingPlan == BillingPlan.Advanced && customer.ParentId is null)
                    {
                        // Get all unbilled servicelogs for for all child customers of a parent
                        serviceLogs = await _context.ServiceLogs
                                           .Include(s => s.Subscriptions)
                                           .Include(s => s.Customer).Where(s => s.SerDate <= request.InvoiceDate)
                                           .ApplySpecification(new UnBilledReplaceInvoiceByCustomerParentSpecification(request.CustomerId))
                                           .ToListAsync(cancellationToken);
                    }
                    else
                    {
                        //get all unbilled servicelogs for a child customer
                        serviceLogs = await _context.ServiceLogs
                                           .Include(s => s.Subscriptions)
                                           .Include(s => s.Customer).Where(s => s.SerDate <= request.InvoiceDate)
                                           .ApplySpecification(new UnBilledReplaceInvoiceByCustomerChildSpecification(request.CustomerId))
                                           .ToListAsync(cancellationToken);
                    }

                    // Validate serviceLogs exists in database
                    if (!serviceLogs.Any()) return await Result<int>.FailureAsync("No unbilled service logs found for the specified criteria.");

                    // Add Invoice Desc as basic information
                    invoice.IsTaxable = customer.IsTaxable;
                    invoice.Description = "فاتورة استبدال وحدات";
                    var xresult = await GenerateInvoiceAsync(_context, invoice, serviceLogs, request.IgnoreTaxes, cancellationToken);
                    return await Result<int>.SuccessAsync(xresult);
                }
            case InvoiceType.Renew:
                {
                    //var customer = await _context.Customers.Where(c => c.Id == request.CustomerId).FirstAsync(cancellationToken);

                    if (customer.BillingPlan == BillingPlan.Advanced && customer.ParentId is null)
                    {
                        // Get all unbilled servicelogs for for all child customers of a parent
                        serviceLogs = await _context.ServiceLogs
                                           .Include(s => s.Subscriptions)
                                           .Include(s => s.Customer).Where(s => s.SerDate <= request.InvoiceDate)
                                           .ApplySpecification(new UnBilledRenewInvoiceByCustomerParentSpecification(request.CustomerId))
                                           .ToListAsync(cancellationToken);
                    }
                    else
                    {
                        //get all unbilled servicelogs for a child customer
                        serviceLogs = await _context.ServiceLogs
                                           .Include(s => s.Subscriptions)
                                           .Include(s => s.Customer).Where(s => s.SerDate <= request.InvoiceDate)
                                           .ApplySpecification(new UnBilledRenewInvoiceByCustomerChildSpecification(request.CustomerId))
                                           .ToListAsync(cancellationToken);
                    }

                    // Validate serviceLogs exists in database
                    if (!serviceLogs.Any()) return await Result<int>.FailureAsync("No unbilled service logs found for the specified criteria.");

                    // Add Invoice Desc as basic information
                    invoice.IsTaxable = customer.IsTaxable;
                    invoice.Description = "فاتورة تجديد اشتراك";
                    var xresult = await GenerateInvoiceAsync(_context, invoice, serviceLogs, request.IgnoreTaxes, cancellationToken);
                    return await Result<int>.SuccessAsync(xresult);
                }
            case InvoiceType.Subscription:
                {
                    //var customer = await _context.Customers.Where(c => c.Id == request.CustomerId).FirstAsync(cancellationToken);

                    if (customer.BillingPlan == BillingPlan.Advanced && customer.ParentId is null)
                    {
                        // Get all unbilled servicelogs for for all child customers of a parent
                        serviceLogs = await _context.ServiceLogs
                                           .Include(s => s.Subscriptions)
                                           .Include(s => s.Customer).Where(s => s.SerDate <= request.InvoiceDate)
                                           .ApplySpecification(new UnBilledSubscriptionInvoiceByCustomerParentSpecification(request.CustomerId))
                                           .ToListAsync(cancellationToken);
                    }
                    else
                    {
                        //get all unbilled servicelogs for a child customer
                        serviceLogs = await _context.ServiceLogs
                                           .Include(s => s.Subscriptions)
                                           .Include(s => s.Customer).Where(s => s.SerDate <= request.InvoiceDate)
                                           .ApplySpecification(new UnBilledSubscriptionInvoiceByCustomerChildSpecification(request.CustomerId))
                                           .ToListAsync(cancellationToken);
                    }

                    // Validate serviceLogs exists in database
                    if (!serviceLogs.Any()) return await Result<int>.FailureAsync("No unbilled service logs found for the specified criteria.");

                    // Add Invoice Desc as basic information
                    invoice.IsTaxable = customer.IsTaxable;
                    invoice.Description = "فاتورة تفعيل / إلغاء تفعيل الاشتراك";
                    var xresult = await GenerateInvoiceAsync(_context, invoice, serviceLogs, request.IgnoreTaxes, cancellationToken);
                    return await Result<int>.SuccessAsync(xresult);
                }
            case InvoiceType.Support:
                {
                    //var customer = await _context.Customers.Where(c => c.Id == request.CustomerId).FirstAsync(cancellationToken);

                    if (customer.BillingPlan == BillingPlan.Advanced && customer.ParentId is null)
                    {
                        // Get all unbilled servicelogs for for all child customers of a parent
                        serviceLogs = await _context.ServiceLogs
                                           .Include(s => s.Subscriptions)
                                           .Include(s => s.Customer).Where(s => s.SerDate <= request.InvoiceDate)
                                           .ApplySpecification(new UnBilledSupportInvoiceByCustomerParentSpecification(request.CustomerId))
                                           .ToListAsync(cancellationToken);
                    }
                    else
                    {
                        //get all unbilled servicelogs for a child customer
                        serviceLogs = await _context.ServiceLogs
                                           .Include(s => s.Subscriptions)
                                           .Include(s => s.Customer).Where(s => s.SerDate <= request.InvoiceDate)
                                           .ApplySpecification(new UnBilledSupportInvoiceByCustomerChildSpecification(request.CustomerId))
                                           .ToListAsync(cancellationToken);
                    }

                    // Validate serviceLogs exists in database
                    if (!serviceLogs.Any()) return await Result<int>.FailureAsync("No unbilled service logs found for the specified criteria.");

                    // Add Invoice Desc as basic information
                    invoice.IsTaxable = customer.IsTaxable;
                    invoice.Description = "فاتورة اعمال دعم فني";
                    var xresult = await GenerateInvoiceAsync(_context, invoice, serviceLogs, request.IgnoreTaxes, cancellationToken);
                    return await Result<int>.SuccessAsync(xresult);

                }

            default:
                {
                    return await Result<int>.FailureAsync("requested Invoice Type not implemented");
                }
        }

        // Call main generation method with all found IDs
  
        //CC:  IsTaxable     =>    Invoice:   IgnoreTaxes

        //       False                          False           ==> To = x, Tx = 0, GT = To             Invoice: IsTaxable = False           
        //       False                           True           ==> To = x, Tx = 0, GT = To             Invoice: IsTaxable = False   
        //       True                           False           ==> To = x, Tx = x/100, GT = To + Tx    Invoice: IsTaxable = true
        //       True                            True           ==> To = x, Tx = 0, GT = To             Invoice: IsTaxable = true    

    }





private async Task<int> GenerateInvoiceAsync(IApplicationDbContext _context,InvoiceDto xinvoice, List<ServiceLog> serviceLogs,bool ignoreTaxes, CancellationToken cancellationToken)
{
        // Start database transaction to ensure data consistency
        //using var transaction = await _context.Database.BeginTransactionAsync();

    try
    {
        // Process service logs and convert them to invoice item groups
        await InvoiceLogic.ProcessServiceLogsForInvoiceAsync(xinvoice, serviceLogs);

            xinvoice.DiscountAmount = xinvoice.Total * (xinvoice.DiscountRate/100);

            xinvoice.TaxableAmount = xinvoice.Total - xinvoice.DiscountAmount;

            if (ignoreTaxes)
            {
                xinvoice.TaxAmount = 0.0m;
                xinvoice.GrandTotal = xinvoice.TaxableAmount;
            }

            else

            {
                var taxAmount = Math.Round((xinvoice.TaxableAmount * (xinvoice.TaxRate / 100)), 3, MidpointRounding.AwayFromZero);
                xinvoice.TaxAmount = taxAmount;
                xinvoice.GrandTotal = Math.Round((xinvoice.TaxableAmount + taxAmount), 3, MidpointRounding.AwayFromZero);
            }

            var invoice = Mapper.FromDto(xinvoice);
            // Save invoice and all related entities to database
            _context.Invoices.Add(invoice);
            // raise a update domain event
            invoice.AddDomainEvent(new InvoiceUpdatedEvent(invoice));

        return await _context.SaveChangesAsync(cancellationToken);

    }
    catch (Exception ex)
    {
        //await transaction.RollbackAsync(); // Rollback on error to maintain data consistency
        //_logger.LogError(ex, $"Error generating invoice for customer {customerId}");
        throw; // Re-throw exception for caller to handle
    }
}


}



