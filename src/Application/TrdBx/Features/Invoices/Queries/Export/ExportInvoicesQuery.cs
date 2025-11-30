using CleanArchitecture.Blazor.Application.Features.Invoices.Caching;
using CleanArchitecture.Blazor.Application.Features.Invoices.DTOs;
using CleanArchitecture.Blazor.Application.Features.Invoices.Mappers;
using CleanArchitecture.Blazor.Application.Features.Invoices.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.Invoices.Queries.Export;

public class ExportInvoicesQuery : InvoiceAdvancedFilter, ICacheableRequest<Result<byte[]>>
{
      public InvoiceAdvancedSpecification Specification => new InvoiceAdvancedSpecification(this);
      public IEnumerable<string> Tags => InvoiceCacheKey.Tags;
    public override string ToString()
    {
        return $"Listview:{ListView}, Search:{Keyword},Client/Customer:{CustomerId},InvoiceType:{InvoiceType},IStatus:{IStatus}, SortDirection:{SortDirection}, OrderBy:{OrderBy}, {PageNumber}, {PageSize}";
    }
    public string CacheKey => InvoiceCacheKey.GetExportCacheKey($"{this}");
}
    
public class ExportInvoicesQueryHandler :
         IRequestHandler<ExportInvoicesQuery, Result<byte[]>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //private readonly IExcelService _excelService;
    //private readonly IStringLocalizer<ExportInvoicesQueryHandler> _localizer;
    //private readonly InvoiceDto _dto = new();
    //public ExportInvoicesQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper,
    //    IExcelService excelService,
    //    IStringLocalizer<ExportInvoicesQueryHandler> localizer
    //    )
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //    _excelService = excelService;
    //    _localizer = localizer;
    //}

    private readonly IApplicationDbContext _context;
    private readonly IExcelService _excelService;
    private readonly IStringLocalizer<ExportInvoicesQueryHandler> _localizer;
    private readonly InvoiceDto _dto = new();
    public ExportInvoicesQueryHandler(
        IApplicationDbContext context,
        IExcelService excelService,
        IStringLocalizer<ExportInvoicesQueryHandler> localizer
        )
    {
        _context = context;
        _excelService = excelService;
        _localizer = localizer;
    }
    public async Task<Result<byte[]>> Handle(ExportInvoicesQuery request, CancellationToken cancellationToken)
        {

        //await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await db.Invoices.ApplySpecification(request.Specification)
        //           .OrderBy($"{request.OrderBy} {request.SortDirection}")
        //           .ProjectTo<InvoiceDto>(_mapper.ConfigurationProvider)
        //           .AsNoTracking()
        //           .ToListAsync(cancellationToken);

        var data = await _context.Invoices.ApplySpecification(request.Specification)
    .OrderBy($"{request.OrderBy} {request.SortDirection}")
    .ProjectTo()
    .AsNoTracking()
    .ToListAsync(cancellationToken);

        var result = await _excelService.ExportAsync(data,
            new Dictionary<string, Func<InvoiceDto, object?>>()
            {
                    {_localizer[_dto.GetMemberDescription(x=>x.Id)],item => item.Id},
                    {_localizer[_dto.GetMemberDescription(x=>x.InvNo)],item => item.InvNo},
{_localizer[_dto.GetMemberDescription(x=>x.InvDate)],item => item.InvDate},
{_localizer[_dto.GetMemberDescription(x=>x.DueDate)],item => item.DueDate},
{_localizer[_dto.GetMemberDescription(x=>x.InvoiceType)],item => item.InvoiceType},
{_localizer[_dto.GetMemberDescription(x=>x.IStatus)],item => item.IStatus},
{_localizer[_dto.GetMemberDescription(x=>x.CustomerId)],item => item.CustomerId},
{_localizer[_dto.GetMemberDescription(x=>x.InvDesc)],item => item.InvDesc},

{_localizer[_dto.GetMemberDescription(x=>x.Total)],item => item.Total},
{_localizer[_dto.GetMemberDescription(x=>x.Taxes)],item => item.Taxes},
{_localizer[_dto.GetMemberDescription(x=>x.GrangTotal)],item => item.GrangTotal}

            }
            , _localizer[_dto.GetClassDescription()]);

        return await Result<byte[]>.SuccessAsync(result);

        }
}
