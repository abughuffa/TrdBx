using CleanArchitecture.Blazor.Application.Features.WialonTasks.Caching;
using CleanArchitecture.Blazor.Application.Features.WialonTasks.DTOs;
using CleanArchitecture.Blazor.Application.Features.WialonTasks.Mappers;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.WialonTasks.Commands.Import;

public class ImportWialonTasksCommand : ICacheInvalidatorRequest<Result<int>>
{
    public string FileName { get; set; }
    public byte[] Data { get; set; }
    public string CacheKey => WialonTaskCacheKey.GetAllCacheKey;
     public IEnumerable<string> Tags => WialonTaskCacheKey.Tags;
    public ImportWialonTasksCommand(string fileName, byte[] data)
    {
        FileName = fileName;
        Data = data;
    }
}
public record class CreateWialonTasksTemplateCommand : IRequest<Result<byte[]>>
{

}

public class ImportWialonTasksCommandHandler :
             IRequestHandler<CreateWialonTasksTemplateCommand, Result<byte[]>>,
             IRequestHandler<ImportWialonTasksCommand, Result<int>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IStringLocalizer<ImportWialonTasksCommandHandler> _localizer;
    //private readonly IExcelService _excelService;
    //private readonly WialonTaskDto _dto = new() { Desc = string.Empty };
    //private readonly IMapper _mapper;
    //public ImportWialonTasksCommandHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper,
    //    IExcelService excelService,
    //    IStringLocalizer<ImportWialonTasksCommandHandler> localizer)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _localizer = localizer;
    //    _excelService = excelService;
    //    _mapper = mapper;
    //}


    private readonly IApplicationDbContext _context;
    private readonly IStringLocalizer<ImportWialonTasksCommandHandler> _localizer;
    private readonly IExcelService _excelService;
    private readonly WialonTaskDto _dto = new();
    public ImportWialonTasksCommandHandler(
        IApplicationDbContext context,
        IExcelService excelService,
        IStringLocalizer<ImportWialonTasksCommandHandler> localizer)
    {
        _context = context;
        _localizer = localizer;
        _excelService = excelService;
    }
#nullable disable warnings
    public async Task<Result<int>> Handle(ImportWialonTasksCommand request, CancellationToken cancellationToken)
    {
        ////await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var result = await _excelService.ImportAsync(request.Data, mappers: new Dictionary<string, Func<DataRow, WialonTaskDto, object?>>
            {
               { _localizer[_dto.GetMemberDescription(x=>x.Id)], (row, item) => item.Id = int.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.Id)]].ToString()) },
               { _localizer[_dto.GetMemberDescription(x=>x.ServiceLogId)], (row, item) => item.ServiceLogId = int.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.ServiceLogId)]].ToString())},
               { _localizer[_dto.GetMemberDescription(x=>x.TrackingUnitId)], (row, item) => item.TrackingUnitId = int.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.TrackingUnitId)]].ToString())},
               { _localizer[_dto.GetMemberDescription(x=>x.Desc)], (row, item) => item.Desc = row[_localizer[_dto.GetMemberDescription(x=>x.Desc)]].ToString() },
               //{ _localizer[_dto.GetMemberDescription(x=>x.ExcDate)], (row, item) => item.ExcDate = row[_localizer[_dto.GetMemberDescription(x=>x.ExcDate)]].ToString().IsNullOrEmpty() ? null : DateOnly.FromDateTime(DateTime.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.ExcDate)]].ToString()))},
               { _localizer[_dto.GetMemberDescription(x=>x.ExcDate)], (row, item) => item.ExcDate = DateOnly.FromDateTime(DateTime.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.ExcDate)]].ToString()))},
               { _localizer[_dto.GetMemberDescription(x=>x.APITask)], (row, item) => item.APITask = (APITask)Convert.ToInt32(row[_localizer[_dto.GetMemberDescription(x=>x.APITask)]].ToString()) },
               { _localizer[_dto.GetMemberDescription(x=>x.IsExecuted)], (row, item) => item.IsExecuted = bool.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.IsExecuted)]].ToString())},

            }, _localizer[_dto.GetClassDescription()]);
        if (result.Succeeded && result.Data is not null)
        {
            foreach (var dto in result.Data)
            {
                var exists = await _context.WialonTasks.AnyAsync(x => x.Id == dto.Id, cancellationToken);
                if (!exists)
                {
                    ////var item = _mapper.Map<WialonTask>(dto);

                    var item = Mapper.FromDto(dto);

                    // add create domain events if this entity implement the IHasDomainEvent interface
                    // item.AddDomainEvent(new ContactCreatedEvent(item));
                    await _context.WialonTasks.AddAsync(item, cancellationToken);
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
    public async Task<Result<byte[]>> Handle(CreateWialonTasksTemplateCommand request, CancellationToken cancellationToken)
    {
        // TODO: Implement ImportWialonTasksCommandHandler method 
        var fields = new string[] {
                   // TODO: Define the fields that should be generate in the template, for example:
                   _localizer[_dto.GetMemberDescription(x=>x.Id)],
                   _localizer[_dto.GetMemberDescription(x=>x.ServiceLogId)],
_localizer[_dto.GetMemberDescription(x=>x.TrackingUnitId)],
_localizer[_dto.GetMemberDescription(x=>x.Desc)],
_localizer[_dto.GetMemberDescription(x=>x.APITask)],
_localizer[_dto.GetMemberDescription(x=>x.ExcDate)],
_localizer[_dto.GetMemberDescription(x=>x.IsExecuted)]
                };
        var result = await _excelService.CreateTemplateAsync(fields, _localizer[_dto.GetClassDescription()]);
        return await Result<byte[]>.SuccessAsync(result);
    }
}
