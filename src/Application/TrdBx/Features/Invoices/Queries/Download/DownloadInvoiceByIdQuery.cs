using CleanArchitecture.Blazor.Application.Features.Invoices.Queries;
using CleanArchitecture.Blazor.Application.Features.Invoices.Specifications;
using CleanArchitecture.Blazor.Domain.Entities;


namespace CleanArchitecture.Blazor.Application.Features.Invoices.Queries.Download;

public class DownloadInvoiceByIdQuery : InvoiceAdvancedFilter, IRequest<Result<byte[]>>
{
    public int Id { get; set; }

    public DownloadInvoiceByIdQuery(int id)
    {
        Id = id;
    }
}

public class DownloadInvoiceByIdQueryHandler :
         IRequestHandler<DownloadInvoiceByIdQuery, Result<byte[]>>
{


    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IStringLocalizer<DownloadInvoiceByIdQueryHandler> _localizer;
    //private readonly IPDFService _pdfService;
    //public DownloadInvoiceByIdQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IPDFService pdfService,
    //    IStringLocalizer<DownloadInvoiceByIdQueryHandler> localizer
    //    )
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _pdfService = pdfService;
    //    _localizer = localizer;
    //}

    private readonly IApplicationDbContext _context;
    private readonly IStringLocalizer<DownloadInvoiceByIdQueryHandler> _localizer;
    private readonly IPDFService _pdfService;
    public DownloadInvoiceByIdQueryHandler(
        IApplicationDbContext context,
        IPDFService pdfService,
        IStringLocalizer<DownloadInvoiceByIdQueryHandler> localizer
        )
    {
        _context = context;
        _pdfService = pdfService;
        _localizer = localizer;
    }

#nullable disable warnings
    public async Task<Result<byte[]>> Handle(DownloadInvoiceByIdQuery request, CancellationToken cancellationToken)
    {

        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var invoice = await _context.Invoices.Include(x => x.InvoiceItems)
                                                    .Include(x => x.Customer)
                                                    .Where(o => o.Id == request.Id)
                                                    .AsNoTracking().FirstAsync();

        if (invoice is null) return null;

        var serviceLogIds = invoice.InvoiceItems.Select(x => x.ServiceLogId).ToList();

        var serviceLogs = await _context.ServiceLogs.Include(s => s.Subscriptions).Where(s => serviceLogIds.Contains(s.Id)).ToListAsync();

        var invItems = new List<InvItem>();



        var Serial = 1;

        var SubSerial = 1;

        foreach (var item in invoice.InvoiceItems)
        {
            var sl = serviceLogs.Where(s => s.Id == item.ServiceLogId).First();

            SubSerial = 1;

            invItems.Add(new InvItem
            {
                Serial = Serial,
                SubSerial = SubSerial,
                Desc = sl.Desc,
                SubTotal = sl.Amount,
                ItemTotal = item.Amount
            });
            if (sl.Subscriptions.Any())
            {
                foreach (var sub in sl.Subscriptions)
                {
                    SubSerial++;

                    invItems.Add(new InvItem
                    {
                        Serial = Serial,
                        SubSerial = SubSerial,
                        Desc = sub.Desc,
                        SubTotal = sub.Amount,
                        ItemTotal = item.Amount
                    });
                }
            }

            Serial++;
        }

        byte[] result;


        Dictionary<string, string> Params;

        Params = new Dictionary<string, string>();
        Params.Add("InvNo", invoice.InvNo);
        Params.Add("InvDate", invoice.InvDate.ToString());
        //Params.Add("DueDate", invoice.DueDate.ToString());
        Params.Add("To", invoice.Customer.Name);
        Params.Add("InvDesc", invoice.InvDesc);
        Params.Add("Total", invoice.Total.ToString());
        Params.Add("Taxes", invoice.Taxes.ToString());
        Params.Add("GrangTotal", invoice.GrangTotal.ToString());
        Params.Add("GrangTotalInText", ToWord.ConvertToWord(invoice.GrangTotal));



        Dictionary<string, Func<InvItem, object?>> mappers;

        mappers = new Dictionary<string, Func<InvItem, object?>>
                {
                    { _localizer["Serial"], item => item.Serial },
                    { _localizer["SubSerial"], item => item.SubSerial },
                    { _localizer["Desc"], item => item.Desc },
                    { _localizer["SubTotal"], item => item.SubTotal },
                    { _localizer["ItemTotal"], item => item.ItemTotal }

        };

        result = await _pdfService.ExportReportAsync(invItems, Params, mappers);

        return await Result<byte[]>.SuccessAsync(result);
    }
}



