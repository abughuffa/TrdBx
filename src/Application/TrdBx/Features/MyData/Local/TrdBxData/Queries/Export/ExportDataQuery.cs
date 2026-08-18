// // using ClosedXML.Excel;

// using System.Drawing;

// namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.Queries.Export;

// public class ExportDataQuery :  IRequest<Result<byte[]>>
// {

// }

// public class ExportDataQueryHandler :
//          IRequestHandler<ExportDataQuery, Result<byte[]>>
// {
//     //private readonly IApplicationDbContextFactory _dbContextFactory;
//     //private readonly IMapper _mapper;
//     //private readonly IExcelService _excelService;
//     //private readonly IStringLocalizer<ExportDataQueryHandler> _localizer;
//     //private readonly InvoiceDto _dto = new();
//     //public ExportDataQueryHandler(
//     //    IApplicationDbContextFactory dbContextFactory,
//     //    IMapper mapper,
//     //    IExcelService excelService,
//     //    IStringLocalizer<ExportDataQueryHandler> localizer
//     //    )
//     //{
//     //    _dbContextFactory = dbContextFactory;
//     //    _mapper = mapper;
//     //    _excelService = excelService;
//     //    _localizer = localizer;
//     //}

//     private readonly TypeAdapterConfig _typeAdapterConfig;
//         private readonly IApplicationDbContextFactory _dbContextFactory;
//     private readonly IExcelService _excelService;
//     private readonly IStringLocalizer<ExportDataQueryHandler> _localizer;
//         public ExportDataQueryHandler(
//             TypeAdapterConfig typeAdapterConfig,
//             IApplicationDbContextFactory dbContextFactory,
//             IExcelService excelService,
//             IStringLocalizer<ExportDataQueryHandler> localizer
//             )
//         {
//             _typeAdapterConfig = typeAdapterConfig;
//             _dbContextFactory = dbContextFactory;
//             _excelService = excelService;
//             _localizer = localizer;
//         }
// #nullable disable warnings
//     public async ValueTask<Result<byte[]>> Handle(ExportDataQuery request, CancellationToken cancellationToken)
//     {

//    await using var context = await _dbContextFactory.CreateAsync(cancellationToken);
//         var dataToExport = new List<(string SheetName, IEnumerable<object> Data)>
//                                     {
//                                         ("TrackingUnitModels", await context.TrackingUnitModels.ToListAsync()),
//                                         ("SProviders", await context.SProviders.ToListAsync()),
//                                         ("SPackages", await context.SPackages.ToListAsync()),
//                                         ("SimCards", await context.SimCards.ToListAsync()),
//                                         ("Customers", await context.Customers.ToListAsync()),
//                                         ("TrackedAssets", await context.TrackedAssets.ToListAsync()),
//                                         ("TrackingUnits", await context.TrackingUnits.ToListAsync()),
//                                         ("CusPrices", await context.CusPrices.ToListAsync()),
//                                         ("ServiceLogs", await context.ServiceLogs.ToListAsync()),
//                                         ("Subscriptions", await context.Subscriptions.ToListAsync()),
//                                         ("WialonTasks", await context.WialonTasks.ToListAsync()),
//                                         ("Tickets", await context.Tickets.ToListAsync()),

//                                         ("LibyanaSimCards", await context.LibyanaSimCards.ToListAsync()),
//                                         ("WialonUnits", await context.WialonUnits.ToListAsync()),
//                                         ("InvoiceItems", await context.InvoiceItems.ToListAsync()),
//                                         ("InvoiceItemGroups", await context.InvoiceItemGroups.ToListAsync()),
//                                         ("Invoices", await context.Invoices.ToListAsync()),
//                                     };


//         byte[] fileBytes = ExportMultipleTables(dataToExport);

//         return await Result<byte[]>.SuccessAsync(fileBytes);




//     }


//     private byte[] ExportMultipleTables(List<(string SheetName, IEnumerable<object> Data)> tables)
//     {
//         using (var workbook = new XLWorkbook())
//         {
//             foreach (var table in tables)
//             {
//                 var ws = workbook.Worksheets.Add(table.SheetName);

//                 // Get the type of objects in the collection
//                 var firstItem = table.Data.FirstOrDefault();
//                 if (firstItem == null) continue;

//                 var properties = firstItem.GetType().GetProperties();

//                 // 1. Add Headers using Property Names
//                 for (int i = 0; i < properties.Length; i++)
//                 {
//                     var cell = ws.Cell(1, i + 1);
//                     cell.Value = properties[i].Name; // Actual property/column name
//                     cell.Style.Fill.BackgroundColor = Color.LightBlue;
//                     cell.Style.Font.Bold = true;
//                 }

//                 // 2. Add Data Rows
//                 int rowIndex = 2;
//                 foreach (var item in table.Data)
//                 {
//                     for (int i = 0; i < properties.Length; i++)
//                     {
//                         var value = properties[i].GetValue(item);
//                         ws.Cell(rowIndex, i + 1).Value = value == null ? Blank.Value : value.ToString();
//                     }
//                     rowIndex++;
//                 }

//                 ws.Columns().AdjustToContents();
//             }

//             using (var stream = new MemoryStream())
//             {
//                 workbook.SaveAs(stream);
//                 return stream.ToArray();
//             }
//         }
//     }

// }



