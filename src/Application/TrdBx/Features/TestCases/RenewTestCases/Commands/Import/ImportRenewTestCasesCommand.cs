//using CleanArchitecture.Blazor.Application.Features.RenewTestCases.Caching;
//using CleanArchitecture.Blazor.Application.Features.TestCases.RenewTestCases.DTOs;

//namespace CleanArchitecture.Blazor.Application.Features.TestCases.RenewTestCases.Commands.Import;

//public class ImportRenewTestCasesCommand  : ICacheInvalidatorRequest<Result<int>>
//{
//    public string FileName { get; set; }
//    public byte[] Data { get; set; }
//    public string CacheKey => RenewTestCaseCacheKey.GetAllCacheKey;
//     public IEnumerable<string> Tags => RenewTestCaseCacheKey.Tags;
//    public ImportRenewTestCasesCommand(string fileName, byte[] data)
//    {
//        FileName = fileName;
//        Data = data;
//    }
//}
//public record class CreateRenewTestCasesTemplateCommand : IRequest<Result<byte[]>>
//{

//}

//public class ImportRenewTestCasesCommandHandler :
//             IRequestHandler<CreateRenewTestCasesTemplateCommand, Result<byte[]>>,
//             IRequestHandler<ImportRenewTestCasesCommand, Result<int>>
//{
//    private readonly IMapper _mapper;
//    private readonly IApplicationDbContext _context;
//    private readonly IStringLocalizer<ImportRenewTestCasesCommandHandler> _localizer;
//    private readonly IExcelService _excelService;
//    private readonly RenewTestCaseDto _dto = new();

//    public ImportRenewTestCasesCommandHandler(
//        IApplicationDbContext context,
//        IExcelService excelService,
//        IStringLocalizer<ImportRenewTestCasesCommandHandler> localizer, IMapper mapper)
//    {
//        _mapper = mapper;
//        _context = context;
//        _localizer = localizer;
//        _excelService = excelService;
//    }
//#nullable disable warnings
//    public async Task<Result<int>> Handle(ImportRenewTestCasesCommand request, CancellationToken cancellationToken)
//    {
//        var result = await _excelService.ImportAsync(request.Data, mappers: new Dictionary<string, Func<DataRow, RenewTestCaseDto, object?>>
//            {
//             { _localizer[_dto.GetMemberDescription(x=>x.Id)],
//                (row, item) => item.Id = int.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.Id)]].ToString()) },
//             { _localizer[_dto.GetMemberDescription(x=>x.CaseCode)], (row, item) => item.CaseCode = row[_localizer[_dto.GetMemberDescription(x=>x.CaseCode)]].ToString() != null ? Convert.ToInt32(row[_localizer[_dto.GetMemberDescription(x => x.CaseCode)]].ToString()) : null },
//             { _localizer[_dto.GetMemberDescription(x=>x.TrackingUnitId)],
//                (row, item) => item.TrackingUnitId = int.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.TrackingUnitId)]].ToString()) },
//             { _localizer[_dto.GetMemberDescription(x=>x.InstallerId)],
//                (row, item) => item.InstallerId = row[_localizer[_dto.GetMemberDescription(x=>x.InstallerId)]].ToString() },
//             { _localizer[_dto.GetMemberDescription(x=>x.SNo)],
//                (row, item) => item.SNo = row[_localizer[_dto.GetMemberDescription(x=>x.SNo)]].ToString() },
//             { _localizer[_dto.GetMemberDescription(x=>x.TsDate)],
//                (row, item) => item.TsDate = DateOnly.FromDateTime(DateTime.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.TsDate)]].ToString()))},
//        }, _localizer[_dto.GetClassDescription()]);


//        if (result.Succeeded && result.Data is not null)
//        {
//            foreach (var dto in result.Data)
//            {
//                var exists = await _context.RenewTestCases.AnyAsync(x => x.Id == dto.Id, cancellationToken);
//                if (!exists)
//                {
//                    var item = _mapper.Map<RenewTestCase>(dto);
//                    // add create domain events if this entity implement the IHasDomainEvent interface
//                    // item.AddDomainEvent(new RenewTestCaseCreatedEvent(item));
//                    await _context.RenewTestCases.AddAsync(item, cancellationToken);
//                }
//            }
//            await _context.SaveChangesAsync(cancellationToken);
//            return await Result<int>.SuccessAsync(result.Data.Count());
//        }
//        else
//        {
//            return await Result<int>.FailureAsync(result.Errors);
//        }
//    }

//    public async Task<Result<byte[]>> Handle(CreateRenewTestCasesTemplateCommand request, CancellationToken cancellationToken)
//    {
//        var fields = new string[] {
//                   // Define the fields that should be generate in the template.
//                   _localizer[_dto.GetMemberDescription(x=>x.Id)],
//                   _localizer[_dto.GetMemberDescription(x=>x.CaseCode)],
//                _localizer[_dto.GetMemberDescription(x=>x.TrackingUnitId)],
//                _localizer[_dto.GetMemberDescription(x=>x.InstallerId)],
//                _localizer[_dto.GetMemberDescription(x=>x.SNo)],
//                _localizer[_dto.GetMemberDescription(x=>x.TsDate)],

//                };
//        var result = await _excelService.CreateTemplateAsync(fields, _localizer[_dto.GetClassDescription()]);
//        return await Result<byte[]>.SuccessAsync(result);
//    }


//}