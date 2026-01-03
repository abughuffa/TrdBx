using CleanArchitecture.Blazor.Application.Features.ServiceLogs.Commands.Import;
using CleanArchitecture.Blazor.Application.Features.SProviders.Mappers;
using CleanArchitecture.Blazor.Application.Features.SProviders.Caching;
using CleanArchitecture.Blazor.Application.Features.SProviders.DTOs;
using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.SProviders.Commands.Import;

public class ImportSProvidersCommand : ICacheInvalidatorRequest<Result<int>>
{
    public string FileName { get; set; }
    public byte[] Data { get; set; }
    public string CacheKey => SProviderCacheKey.GetAllCacheKey;
     public IEnumerable<string> Tags => SProviderCacheKey.Tags;
    public ImportSProvidersCommand(string fileName, byte[] data)
    {
        FileName = fileName;
        Data = data;
    }
}
public record class CreateSProvidersTemplateCommand : IRequest<Result<byte[]>>
{

}

public class ImportSProvidersCommandHandler :
             IRequestHandler<CreateSProvidersTemplateCommand, Result<byte[]>>,
             IRequestHandler<ImportSProvidersCommand, Result<int>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IStringLocalizer<ImportSProvidersCommandHandler> _localizer;
    //private readonly IExcelService _excelService;
    //private readonly SProviderDto _dto = new();
    //private readonly IMapper _mapper;
    //public ImportSProvidersCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper,
    //    IExcelService excelService,
    //    IStringLocalizer<ImportSProvidersCommandHandler> localizer)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _localizer = localizer;
    //    _excelService = excelService;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    private readonly IStringLocalizer<ImportSProvidersCommandHandler> _localizer;
    private readonly IExcelService _excelService;
    private readonly SProviderDto _dto = new();
    public ImportSProvidersCommandHandler(
        IApplicationDbContext context,
        IExcelService excelService,
        IStringLocalizer<ImportSProvidersCommandHandler> localizer)
    {
        _context = context;
        _localizer = localizer;
        _excelService = excelService;
    }
#nullable disable warnings
    public async Task<Result<int>> Handle(ImportSProvidersCommand request, CancellationToken cancellationToken)
    {

        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var result = await _excelService.ImportAsync(request.Data, mappers: new Dictionary<string, Func<DataRow, SProviderDto, object?>>
            {
                { _localizer[_dto.GetMemberDescription(x=>x.Id)], (row, item) => item.Id = int.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.Id)]].ToString()) },
                { _localizer[_dto.GetMemberDescription(x=>x.Name)], (row, item) => item.Name = row[_localizer[_dto.GetMemberDescription(x=>x.Name)]].ToString() },
            }, _localizer[_dto.GetClassDescription()]);
        if (result.Succeeded && result.Data is not null)
        {
            foreach (var dto in result.Data)
            {
                var exists = await _context.SProviders.AnyAsync(x => x.Name == dto.Name, cancellationToken);
                if (!exists)
                {
                    //var item = _mapper.Map<SProvider>(dto);

                    var item = Mapper.FromDto(dto);
                    // add create domain events if this entity implement the IHasDomainEvent interface
                    // item.AddDomainEvent(new ContactCreatedEvent(item));
                    await _context.SProviders.AddAsync(item, cancellationToken);
                }
            }
            await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(result.Data.Count());
        }
        else
        {
            return await Result<int>.FailureAsync(result.Errors);
        }

    }
    public async Task<Result<byte[]>> Handle(CreateSProvidersTemplateCommand request, CancellationToken cancellationToken)
    {
        var fields = new string[] {
                  _localizer[_dto.GetMemberDescription(x=>x.Name)],

                };
        var result = await _excelService.CreateTemplateAsync(fields, _localizer[_dto.GetClassDescription()]);
        return await Result<byte[]>.SuccessAsync(result);
    }
}

