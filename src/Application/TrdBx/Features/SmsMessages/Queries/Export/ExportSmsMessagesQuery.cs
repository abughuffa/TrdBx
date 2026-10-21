using CleanArchitecture.Blazor.Application.Features.SmsMessages.Caching;
using CleanArchitecture.Blazor.Application.Features.SmsMessages.DTOs;
// using CleanArchitecture.Blazor.Application.Features.SmsMessages.Mappers;
using CleanArchitecture.Blazor.Application.Features.SmsMessages.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.SmsMessages.Queries.Export;

public class ExportSmsMessagesQuery : SmsMessageAdvancedFilter, ICacheableRequest<Result<byte[]>>
{
      public SmsMessageAdvancedSpecification Specification => new SmsMessageAdvancedSpecification(this);
       public IEnumerable<string> Tags => SmsMessageCacheKey.Tags;
    public override string ToString()
    {
        return $"Listview:{ListView}:-{LocalTimezoneOffset.TotalHours}, Search:{Keyword}, {OrderBy}, {SortDirection}";
    }
    public string CacheKey => SmsMessageCacheKey.GetExportCacheKey($"{this}");

}
    
public class ExportSmsMessagesQueryHandler :
         IRequestHandler<ExportSmsMessagesQuery, Result<byte[]>>
{


    private readonly TypeAdapterConfig _typeAdapterConfig;
        private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly IExcelService _excelService;
    private readonly IStringLocalizer<ExportSmsMessagesQueryHandler> _localizer;
    private readonly SmsMessageDto _dto = new();
        public ExportSmsMessagesQueryHandler(
            TypeAdapterConfig typeAdapterConfig,
            IApplicationDbContextFactory dbContextFactory,
            IExcelService excelService,
            IStringLocalizer<ExportSmsMessagesQueryHandler> localizer
            )
        {
            _typeAdapterConfig = typeAdapterConfig;
            _dbContextFactory = dbContextFactory;
            _excelService = excelService;
            _localizer = localizer;
        }
#nullable disable warnings
    public async ValueTask<Result<byte[]>> Handle(ExportSmsMessagesQuery request, CancellationToken cancellationToken)
        {


        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);


        var data = await context.SmsMessages.ApplySpecification(request.Specification)
    .OrderBy($"{request.OrderBy} {request.SortDirection}")
    .ProjectToType<SmsMessageDto>(_typeAdapterConfig)
    .AsNoTracking()
    .ToListAsync(cancellationToken);

        var result = await _excelService.ExportAsync(data, new Dictionary<string, Func<SmsMessageDto, object?>>()
            {
                    {_localizer[_dto.GetMemberDisplayName(x=>x.Id)],item => item.Id},
                    {_localizer[_dto.GetMemberDisplayName(x=>x.To)],item => item.To},
                    {_localizer[_dto.GetMemberDisplayName(x=>x.Body)],item => item.Body},

            }
                    , _localizer[_dto.GetClassDescription()]);

        return await Result<byte[]>.SuccessAsync(result);

        }
}
