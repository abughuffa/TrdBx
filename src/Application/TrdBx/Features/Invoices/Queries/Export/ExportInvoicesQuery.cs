using CleanArchitecture.Blazor.Application.Features.Invoices.Caching;
using CleanArchitecture.Blazor.Application.Features.Invoices.DTOs;
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

    private readonly TypeAdapterConfig _typeAdapterConfig;
        private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly IExcelService _excelService;
    private readonly IStringLocalizer<ExportInvoicesQueryHandler> _localizer;
    private readonly InvoiceDto _dto = new();
        public ExportInvoicesQueryHandler(
            TypeAdapterConfig typeAdapterConfig,
            IApplicationDbContextFactory dbContextFactory,
            IExcelService excelService,
            IStringLocalizer<ExportInvoicesQueryHandler> localizer
            )
        {
            _typeAdapterConfig = typeAdapterConfig;
            _dbContextFactory = dbContextFactory;
            _excelService = excelService;
            _localizer = localizer;
        }
#nullable disable warnings
    public async ValueTask<Result<byte[]>> Handle(ExportInvoicesQuery request, CancellationToken cancellationToken)
    {

        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await db.Invoices.ApplySpecification(request.Specification)
        //           .OrderBy($"{request.OrderBy} {request.SortDirection}")
        //           .ProjectTo<InvoiceDto>(_mapper.ConfigurationProvider)
        //           .AsNoTracking()
        //           .ToListAsync(cancellationToken);

        var data = await context.Invoices.ApplySpecification(request.Specification)
    .OrderBy($"{request.OrderBy} {request.SortDirection}")
    .ProjectToType<InvoiceDto>(_typeAdapterConfig)
    .AsNoTracking()
    .ToListAsync(cancellationToken);

        var result = await _excelService.ExportAsync(data,
            new Dictionary<string, Func<InvoiceDto, object?>>()
            {
                    {_localizer[_dto.GetMemberDisplayName(x=>x.Id)],item => item.Id},
                    {_localizer[_dto.GetMemberDisplayName(x=>x.InvoiceNo)],item => item.InvoiceNo},
{_localizer[_dto.GetMemberDisplayName(x=>x.InvoiceDate)],item => item.InvoiceDate},
{_localizer[_dto.GetMemberDisplayName(x=>x.DueDate)],item => item.DueDate},
{_localizer[_dto.GetMemberDisplayName(x=>x.InvoiceType)],item => item.InvoiceType},
{_localizer[_dto.GetMemberDisplayName(x=>x.IStatus)],item => item.IStatus},
{_localizer[_dto.GetMemberDisplayName(x=>x.CustomerId)],item => item.CustomerId},
{_localizer[_dto.GetMemberDisplayName(x=>x.Description)],item => item.Description},

{_localizer[_dto.GetMemberDisplayName(x=>x.Total)],item => item.Total},
{_localizer[_dto.GetMemberDisplayName(x=>x.DiscountAmount)],item => item.DiscountAmount},
{_localizer[_dto.GetMemberDisplayName(x=>x.TaxableAmount)],item => item.TaxableAmount},
{_localizer[_dto.GetMemberDisplayName(x=>x.TaxAmount)],item => item.TaxAmount},
{_localizer[_dto.GetMemberDisplayName(x=>x.GrandTotal)],item => item.GrandTotal}

            }
            , _localizer[_dto.GetClassDescription()]);

        return await Result<byte[]>.SuccessAsync(result);

    }
}
