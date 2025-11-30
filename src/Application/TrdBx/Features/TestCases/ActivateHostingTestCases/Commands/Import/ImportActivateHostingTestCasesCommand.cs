using CleanArchitecture.Blazor.Application.Features.TestCases.ActivateHostingTestCases.Caching;
using CleanArchitecture.Blazor.Application.Features.TestCases.ActivateHostingTestCases.DTOs;
using CleanArchitecture.Blazor.Application.Features.TestCases.ActivateHostingTestCases.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.TestCases.ActivateHostingTestCases.Commands.Import;

public class ImportActivateHostingTestCasesCommand : ICacheInvalidatorRequest<Result<int>>
{
    public string FileName { get; set; }
    public byte[] Data { get; set; }
    public string CacheKey => ActivateHostingTestCaseCacheKey.GetAllCacheKey;
     public IEnumerable<string> Tags => ActivateHostingTestCaseCacheKey.Tags;
    public ImportActivateHostingTestCasesCommand(string fileName, byte[] data)
    {
        FileName = fileName;
        Data = data;
    }
}
public record class CreateActivateHostingTestCasesTemplateCommand : IRequest<Result<byte[]>>
{

}

public class ImportActivateHostingTestCasesCommandHandler :
             IRequestHandler<CreateActivateHostingTestCasesTemplateCommand, Result<byte[]>>,
             IRequestHandler<ImportActivateHostingTestCasesCommand, Result<int>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IStringLocalizer<ImportActivateHostingTestCasesCommandHandler> _localizer;
    //private readonly IExcelService _excelService;
    //private readonly ActivateHostingTestCaseDto _dto = new() { InstallerId = string.Empty };
    //private readonly IMapper _mapper;
    //public ImportActivateHostingTestCasesCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper,
    //    IExcelService excelService,
    //    IStringLocalizer<ImportActivateHostingTestCasesCommandHandler> localizer)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _localizer = localizer;
    //    _excelService = excelService;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    private readonly IStringLocalizer<ImportActivateHostingTestCasesCommandHandler> _localizer;
    private readonly IExcelService _excelService;
    private readonly ActivateHostingTestCaseDto _dto = new() { InstallerId = string.Empty };
    public ImportActivateHostingTestCasesCommandHandler(
        IApplicationDbContext context,
        IExcelService excelService,
        IStringLocalizer<ImportActivateHostingTestCasesCommandHandler> localizer)
    {
        _context = context;
        _localizer = localizer;
        _excelService = excelService;
    }
#nullable disable warnings
    public async Task<Result<int>> Handle(ImportActivateHostingTestCasesCommand request, CancellationToken cancellationToken)
    {

        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var result = await _excelService.ImportAsync(request.Data, mappers: new Dictionary<string, Func<DataRow, ActivateHostingTestCaseDto, object?>>
         {
               { _localizer[_dto.GetMemberDescription(x=>x.Id)], (row, item) => item.Id = int.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.Id)]].ToString()) },
               { _localizer[_dto.GetMemberDescription(x=>x.CaseCode)], (row, item) => item.CaseCode = row[_localizer[_dto.GetMemberDescription(x=>x.CaseCode)]].ToString() != null ? Convert.ToInt32(row[_localizer[_dto.GetMemberDescription(x => x.CaseCode)]].ToString()) : null },
                { _localizer[_dto.GetMemberDescription(x=>x.TrackingUnitId)], (row, item) => item.TrackingUnitId = int.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.TrackingUnitId)]].ToString()) },
                { _localizer[_dto.GetMemberDescription(x=>x.InstallerId)], (row, item) => item.InstallerId = row[_localizer[_dto.GetMemberDescription(x=>x.InstallerId)]].ToString() },
                { _localizer[_dto.GetMemberDescription(x=>x.SNo)], (row, item) => item.SNo = row[_localizer[_dto.GetMemberDescription(x=>x.SNo)]].ToString() },
                { _localizer[_dto.GetMemberDescription(x=>x.TsDate)], (row, item) => item.TsDate = DateOnly.FromDateTime(DateTime.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.TsDate)]].ToString()))}
        }, _localizer[_dto.GetClassDescription()]);
        if (result.Succeeded && result.Data is not null)
        {
            foreach (var dto in result.Data)
            {
                var exists = await _context.ActivateHostingTestCases.AnyAsync(x => x.Id == dto.Id, cancellationToken);
                if (!exists)
                {
                    //var item = _mapper.Map<ActivateHostingTestCase>(dto);
                    var item = Mapper.FromDto(dto);
                    // add create domain events if this entity implement the IHasDomainEvent interface
                    // item.AddDomainEvent(new ContactCreatedEvent(item));
                    await _context.ActivateHostingTestCases.AddAsync(item, cancellationToken);
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
    public async Task<Result<byte[]>> Handle(CreateActivateHostingTestCasesTemplateCommand request, CancellationToken cancellationToken)
    {
        // TODO: Implement ImportActivateHostingTestCasesCommandHandler method 
        var fields = new string[] {
                   // TODO: Define the fields that should be generate in the template, for example:
                   _localizer[_dto.GetMemberDescription(x=>x.Id)],
                   _localizer[_dto.GetMemberDescription(x=>x.CaseCode)],
_localizer[_dto.GetMemberDescription(x=>x.TrackingUnitId)],
_localizer[_dto.GetMemberDescription(x=>x.InstallerId)],
_localizer[_dto.GetMemberDescription(x=>x.SNo)],
_localizer[_dto.GetMemberDescription(x=>x.TsDate)],

                };
        var result = await _excelService.CreateTemplateAsync(fields, _localizer[_dto.GetClassDescription()]);
        return await Result<byte[]>.SuccessAsync(result);
    }
}

