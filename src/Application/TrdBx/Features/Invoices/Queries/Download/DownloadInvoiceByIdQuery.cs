
using CleanArchitecture.Blazor.Application.Features.Invoices.DTOs;
using CleanArchitecture.Blazor.Application.Features.Invoices.Specifications;


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


      private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly TypeAdapterConfig _typeAdapterConfig;
        private readonly IStringLocalizer<DownloadInvoiceByIdQueryHandler> _localizer;
    private readonly IPDFService _pdfService;
    public DownloadInvoiceByIdQueryHandler(
        TypeAdapterConfig typeAdapterConfig,
        IApplicationDbContextFactory dbContextFactory,
        IPDFService pdfService,
        IStringLocalizer<DownloadInvoiceByIdQueryHandler> localizer)
    {
        _typeAdapterConfig = typeAdapterConfig;
        _dbContextFactory = dbContextFactory;
                _pdfService = pdfService;
        _localizer = localizer;
    }



#nullable disable warnings
    public async ValueTask<Result<byte[]>> Handle(DownloadInvoiceByIdQuery request, CancellationToken cancellationToken)
    {

        await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var invoice = await _context.Invoices.Include(i => i.Customer)
                                                    .Include(i => i.InvoiceItemGroups).ThenInclude(ig => ig.InvoiceItems)   
                                                    .Where(i => i.Id == request.Id)
                                                     .ProjectToType<InvoiceDto>(_typeAdapterConfig)
                                                    .AsNoTracking().FirstAsync();



        if (invoice is null) return null;

        var result = await _pdfService.ExportInvoiceAsync(invoice);
        return await Result<byte[]>.SuccessAsync(result);
    }
}



