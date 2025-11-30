using CleanArchitecture.Blazor.Application.Features.ServiceLogs.Commands.Import;
using CleanArchitecture.Blazor.Application.Features.Subscriptions.Mappers;
using CleanArchitecture.Blazor.Application.Features.Subscriptions.Caching;
using CleanArchitecture.Blazor.Application.Features.Subscriptions.DTOs;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Enums;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.Subscriptions.Commands.Import;

public class ImportSubscriptionsCommand : ICacheInvalidatorRequest<Result<int>>
{
    public string FileName { get; set; }
    public byte[] Data { get; set; }
    public string CacheKey => SubscriptionCacheKey.GetAllCacheKey;
     public IEnumerable<string> Tags => SubscriptionCacheKey.Tags;
    public ImportSubscriptionsCommand(string fileName, byte[] data)
    {
        FileName = fileName;
        Data = data;
    }
}
public record class CreateSubscriptionsTemplateCommand : IRequest<Result<byte[]>>
{

}

public class ImportSubscriptionsCommandHandler :
             IRequestHandler<CreateSubscriptionsTemplateCommand, Result<byte[]>>,
             IRequestHandler<ImportSubscriptionsCommand, Result<int>>
{
    //    private readonly IApplicationDbContextFactory _dbContextFactory;
    //    private readonly IStringLocalizer<ImportSubscriptionsCommandHandler> _localizer;
    //    private readonly IExcelService _excelService;
    //    private readonly SubscriptionDto _dto = new() { Desc = string.Empty };
    //    private readonly IMapper _mapper;
    //    public ImportSubscriptionsCommandHandler(
    //        IApplicationDbContextFactory dbContextFactory,
    //        IMapper mapper,
    //        IExcelService excelService,
    //        IStringLocalizer<ImportSubscriptionsCommandHandler> localizer)
    //    {
    //        _dbContextFactory = dbContextFactory;
    //        _localizer = localizer;
    //        _excelService = excelService;
    //        _mapper = mapper;
    //    }

    private readonly IApplicationDbContext _context;
    private readonly IStringLocalizer<ImportSubscriptionsCommandHandler> _localizer;
    private readonly IExcelService _excelService;
    private readonly SubscriptionDto _dto = new();
    public ImportSubscriptionsCommandHandler(
        IApplicationDbContext context,
        IExcelService excelService,
        IStringLocalizer<ImportSubscriptionsCommandHandler> localizer)
    {
        _context = context;
        _localizer = localizer;
        _excelService = excelService;
    }
#nullable disable warnings
    public async Task<Result<int>> Handle(ImportSubscriptionsCommand request, CancellationToken cancellationToken)
    {


        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var result = await _excelService.ImportAsync(request.Data, mappers: new Dictionary<string, Func<DataRow, SubscriptionDto, object?>>
            {
               { _localizer[_dto.GetMemberDescription(x=>x.Id)], (row, item) => item.Id = int.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.Id)]].ToString()) },
               { _localizer[_dto.GetMemberDescription(x=>x.ServiceLogId)], (row, item) => item.ServiceLogId = int.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.ServiceLogId)]].ToString())},
               { _localizer[_dto.GetMemberDescription(x=>x.TrackingUnitId)], (row, item) => item.TrackingUnitId = int.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.TrackingUnitId)]].ToString())},
               { _localizer[_dto.GetMemberDescription(x=>x.Desc)], (row, item) => item.Desc = row[_localizer[_dto.GetMemberDescription(x=>x.Desc)]].ToString() },
               { _localizer[_dto.GetMemberDescription(x=>x.CaseCode)], (row, item) => item.CaseCode = int.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.CaseCode)]].ToString())},
               { _localizer[_dto.GetMemberDescription(x=>x.SsDate)], (row, item) => item.SsDate = DateOnly.FromDateTime(DateTime.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.SsDate)]].ToString()))},
               { _localizer[_dto.GetMemberDescription(x=>x.SeDate)], (row, item) => item.SeDate = DateOnly.FromDateTime(DateTime.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.SeDate)]].ToString()))},
               { _localizer[_dto.GetMemberDescription(x=>x.LastPaidFees)], (row, item) => item.LastPaidFees = (SubPackageFees)Convert.ToInt32(row[_localizer[_dto.GetMemberDescription(x=>x.LastPaidFees)]].ToString()) },
               { _localizer[_dto.GetMemberDescription(x=>x.DailyFees)], (row, item) => item.DailyFees = decimal.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.DailyFees)]].ToString())},
               { _localizer[_dto.GetMemberDescription(x=>x.Days)], (row, item) => item.Days = int.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.Days)]].ToString())},
               { _localizer[_dto.GetMemberDescription(x=>x.Amount)], (row, item) => item.Amount = decimal.Parse(row[_localizer[_dto.GetMemberDescription(x=>x.Amount)]].ToString())},
        }, _localizer[_dto.GetClassDescription()]);
        if (result.Succeeded && result.Data is not null)
        {
            foreach (var dto in result.Data)
            {
                //var exists = await _context.Subscriptions.AnyAsync(x => x.s == dto.SimNo, cancellationToken);
                //if (!exists)
                //{
                //var item = _mapper.Map<Subscription>(dto);
                var item = Mapper.FromDto(dto);
                // add create domain events if this entity implement the IHasDomainEvent interface
                // item.AddDomainEvent(new ContactCreatedEvent(item));
                await _context.Subscriptions.AddAsync(item, cancellationToken);
                //}
            }
            await _context.SaveChangesAsync(cancellationToken);
            return await Result<int>.SuccessAsync(result.Data.Count());
        }
        else
        {
            return await Result<int>.FailureAsync(result.Errors);
        }
    }
    public async Task<Result<byte[]>> Handle(CreateSubscriptionsTemplateCommand request, CancellationToken cancellationToken)
    {

        var fields = new string[] {
                           _localizer[_dto.GetMemberDescription(x=>x.Id)],
                   _localizer[_dto.GetMemberDescription(x=>x.ServiceLogId)],
_localizer[_dto.GetMemberDescription(x=>x.TrackingUnitId)],
_localizer[_dto.GetMemberDescription(x=>x.CaseCode)],
_localizer[_dto.GetMemberDescription(x=>x.LastPaidFees)],
_localizer[_dto.GetMemberDescription(x=>x.SsDate)],
_localizer[_dto.GetMemberDescription(x=>x.SeDate)],
_localizer[_dto.GetMemberDescription(x=>x.Desc)],
_localizer[_dto.GetMemberDescription(x=>x.DailyFees)],
_localizer[_dto.GetMemberDescription(x=>x.Days)],
_localizer[_dto.GetMemberDescription(x=>x.Amount)],

                };
        var result = await _excelService.CreateTemplateAsync(fields, _localizer[_dto.GetClassDescription()]);
        return await Result<byte[]>.SuccessAsync(result);
    }
}

