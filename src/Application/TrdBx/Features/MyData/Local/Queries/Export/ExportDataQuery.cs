using ClosedXML.Excel;

namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.Queries.Export;

public class ExportDataQuery :  IRequest<Result<byte[]>>
{

}

public class ExportDataQueryHandler :
         IRequestHandler<ExportDataQuery, Result<byte[]>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //private readonly IExcelService _excelService;
    //private readonly IStringLocalizer<ExportDataQueryHandler> _localizer;
    //private readonly InvoiceDto _dto = new();
    //public ExportDataQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper,
    //    IExcelService excelService,
    //    IStringLocalizer<ExportDataQueryHandler> localizer
    //    )
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //    _excelService = excelService;
    //    _localizer = localizer;
    //}

    private readonly IApplicationDbContext _context;
    private readonly IExcelService _excelService;
    private readonly IStringLocalizer<ExportDataQueryHandler> _localizer;
    //private readonly InvoiceDto _dto = new();
    public ExportDataQueryHandler(
        IApplicationDbContext context,
        IExcelService excelService,
        IStringLocalizer<ExportDataQueryHandler> localizer
        )
    {
        _context = context;
        _excelService = excelService;
        _localizer = localizer;
    }
    public async Task<Result<byte[]>> Handle(ExportDataQuery request, CancellationToken cancellationToken)
    {

        var dataToExport = new List<(string SheetName, IEnumerable<object> Data)>
                                    {
                                        ("TrackingUnitModels", await _context.TrackingUnitModels.ToListAsync()),
                                        ("SProviders", await _context.SProviders.ToListAsync()),
                                        ("SPackages", await _context.SPackages.ToListAsync()),
                                        ("SimCards", await _context.SimCards.ToListAsync()),
                                        ("Customers", await _context.Customers.ToListAsync()),
                                        ("TrackedAssets", await _context.TrackedAssets.ToListAsync()),
                                        ("TrackingUnits", await _context.TrackingUnits.ToListAsync()),
                                        ("CusPrices", await _context.CusPrices.ToListAsync()),
                                        ("ServiceLogs", await _context.ServiceLogs.ToListAsync()),
                                        ("Subscriptions", await _context.Subscriptions.ToListAsync()),
                                        ("WialonTasks", await _context.WialonTasks.ToListAsync()),
                                        ("Tickets", await _context.Tickets.ToListAsync()),

                                        ("LibyanaSimCards", await _context.LibyanaSimCards.ToListAsync()),
                                        ("WialonUnits", await _context.WialonUnits.ToListAsync()),
                                        //("XInvoiceItems", await _context.XInvoiceItems.ToListAsync()),
                                        //("XInvoiceItemGroups", await _context.XInvoiceItemGroups.ToListAsync()),
                                        //("XInvoices", await _context.XInvoices.ToListAsync()),
                                    };


        byte[] fileBytes = ExportMultipleTables(dataToExport);

        return await Result<byte[]>.SuccessAsync(fileBytes);




    }


    private byte[] ExportMultipleTables(List<(string SheetName, IEnumerable<object> Data)> tables)
    {
        using (var workbook = new XLWorkbook())
        {
            foreach (var table in tables)
            {
                var ws = workbook.Worksheets.Add(table.SheetName);

                // Get the type of objects in the collection
                var firstItem = table.Data.FirstOrDefault();
                if (firstItem == null) continue;

                var properties = firstItem.GetType().GetProperties();

                // 1. Add Headers using Property Names
                for (int i = 0; i < properties.Length; i++)
                {
                    var cell = ws.Cell(1, i + 1);
                    cell.Value = properties[i].Name; // Actual property/column name
                    cell.Style.Fill.BackgroundColor = XLColor.LightBlue;
                    cell.Style.Font.Bold = true;
                }

                // 2. Add Data Rows
                int rowIndex = 2;
                foreach (var item in table.Data)
                {
                    for (int i = 0; i < properties.Length; i++)
                    {
                        var value = properties[i].GetValue(item);
                        ws.Cell(rowIndex, i + 1).Value = value == null ? Blank.Value : value.ToString();
                    }
                    rowIndex++;
                }

                ws.Columns().AdjustToContents();
            }

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                return stream.ToArray();
            }
        }
    }

}



