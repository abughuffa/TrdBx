// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Diagnostics.Caching;
using CleanArchitecture.Blazor.Application.Features.Diagnostics.DTOs;
using CleanArchitecture.Blazor.Application.Features.Diagnostics.Mappers;
using CleanArchitecture.Blazor.Application.Features.Diagnostics.Specifications;
using CleanArchitecture.Blazor.Application.Features.SimCards.DTOs;
using CleanArchitecture.Blazor.Application.Features.SimCards.Queries.Export;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.Diagnostics.Queries.Pagination;


public class DiagnosticsWithPaginationQuery : DiagnosticAdvancedFilter, ICacheableRequest<PaginatedData<DiagnosticDto>>
{

    public IEnumerable<string>? Tags => DiagnosticCacheKey.Tags;
    public DiagnosticAdvancedSpecification Specification => new(this);
    public string CacheKey => DiagnosticCacheKey.GetPaginationCacheKey($"{this}");
    public override string ToString()
    {
        return $"Listview:{ListView}, Search:{Keyword}, StatusOnWialon:{StatusOnWialon}, StatusOnTrdBx:{StatusOnTrdBx}, SimCardStatus:{SimCardStatus}, ExpiersBefore:{ExpiersBefore}, SortDirection:{SortDirection}, OrderBy:{OrderBy}, {PageNumber}, {PageSize}";
    }

}

public class DiagnosticsWithPaginationQueryHandler : IRequestHandler<DiagnosticsWithPaginationQuery, PaginatedData<DiagnosticDto>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public DiagnosticsWithPaginationQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    private readonly IExcelService _excelService;
    private readonly IStringLocalizer<DiagnosticsWithPaginationQueryHandler> _localizer;
    private readonly SimCardDto _dto = new();
    public DiagnosticsWithPaginationQueryHandler(
        IApplicationDbContext context,
        IExcelService excelService,
        IStringLocalizer<DiagnosticsWithPaginationQueryHandler> localizer
        )
    {
        _context = context;
        _excelService = excelService;
        _localizer = localizer;
    }

    public async Task<PaginatedData<DiagnosticDto>> Handle(DiagnosticsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        PaginatedData<DiagnosticDto> diagnostics;

        switch (request.ListView)
        {
            case DiagnosticListView.SimCardsOfUnitsWhichAreExistOnTrdBxAndWialon:
                {
                    diagnostics = await (from l in _context.LibyanaSimCards
                                                   join u in _context.TrackingUnits on l.SimCardNo equals u.SimCard.SimCardNo
                                                   join w in _context.WialonUnits on
                                                             new { UnitSNo = u.SNo, SimCardNo = l.SimCardNo } equals
                                                             new { UnitSNo = w.UnitSNo, SimCardNo = w.SimCardNo }
                                             select new Diagnostic
                                             {
                                                 Account = w.Account,
                                                 Client = u.Customer.Parent.Name,
                                                 Customer = u.Customer.Name,
                                                 UnitSNo = u.SNo,
                                                 SimCardNo = l.SimCardNo,
                                                 SimCardStatus = l.SimCardStatus,
                                                 StatusOnWialon = w.StatusOnWialon,
                                                 StatusOnTrdBx = u.UStatus,
                                                 WNote = w.Note,
                                                 Balance = l.Balance,
                                                 LDExDate = l.DExDate,
                                                 LDOExpired = l.DOExpired
                                             })
                       .OrderBy($"{request.OrderBy} {request.SortDirection}")
                                             .ProjectToPaginatedDataAsync(request.Specification,
                                                    request.PageNumber,
                                                    request.PageSize,
                                                    Mapper.ToDto,
                                                    cancellationToken);
                    break;
                  
                }
            case DiagnosticListView.SimCardsOfUnitsWhichAreNotExistOnWialon:
                {
                    diagnostics = await (from l in _context.LibyanaSimCards
                                             join u in _context.TrackingUnits on l.SimCardNo equals u.SimCard.SimCardNo
                                             join w in _context.WialonUnits on
                                                 new { UnitSNo = u.SNo, SimCardNo = l.SimCardNo } equals
                                                 new { UnitSNo = w.UnitSNo, SimCardNo = w.SimCardNo } into wialonJoin
                                             from w in wialonJoin.DefaultIfEmpty()
                                             where w == null // This gives us records where there's no matching WialonUnit (W.SimCardNo IS NULL)
                                             select new Diagnostic
                                             {
                                                 Account = u.Customer.Account,
                                                 Client = u.Customer.Parent.Name,
                                                 Customer = u.Customer.Name,
                                                 UnitSNo = u.SNo,
                                                 SimCardNo = l.SimCardNo,
                                                 SimCardStatus = l.SimCardStatus,
                                                 StatusOnWialon = w.StatusOnWialon,
                                                 StatusOnTrdBx = u.UStatus,
                                                 WNote = w.Note,
                                                 Balance = l.Balance,
                                                 LDExDate = l.DExDate,
                                                 LDOExpired = l.DOExpired

                                            }).OrderBy($"{request.OrderBy} {request.SortDirection}")
                                              .ProjectToPaginatedDataAsync(request.Specification,
                                                    request.PageNumber,
                                                    request.PageSize,
                                                    Mapper.ToDto,
                                                    cancellationToken);

                    break;
                    return diagnostics;

                }
            case DiagnosticListView.SimCardsOfUnitsWhichAreNotExistOnTrdBx:
                {
                    diagnostics = await (from l in _context.LibyanaSimCards
                                             join u in _context.TrackingUnits on l.SimCardNo equals u.SimCard.SimCardNo into unitJoin
                                             from u in unitJoin.DefaultIfEmpty()
                                             join w in _context.WialonUnits on
                                                 new { UnitSNo = u.SNo, SimCardNo = l.SimCardNo } equals
                                                 new { UnitSNo = w.UnitSNo, SimCardNo = w.SimCardNo } into wialonJoin
                                             from w in wialonJoin.DefaultIfEmpty()
                                             where u == null // This gives us records where there's no matching Unit (T.SimCardNo IS NULL)
                                             select new Diagnostic
                                             {
                                                 Account = u.Customer.Account,
                                                 Client = u.Customer.Parent.Name,
                                                 Customer = u.Customer.Name,
                                                 UnitSNo = u.SNo,
                                                 SimCardNo = l.SimCardNo,
                                                 SimCardStatus = l.SimCardStatus,
                                                 StatusOnWialon = w.StatusOnWialon,
                                                 StatusOnTrdBx = u.UStatus,
                                                 WNote = w.Note,
                                                 Balance = l.Balance,
                                                 LDExDate = l.DExDate,
                                                 LDOExpired = l.DOExpired
                                             }).OrderBy($"{request.OrderBy} {request.SortDirection}")
                                               .ProjectToPaginatedDataAsync(request.Specification,
                                                    request.PageNumber,
                                                    request.PageSize,
                                                    Mapper.ToDto,
                                                    cancellationToken);
                    break;
                    //return diagnostics;


                }
            case DiagnosticListView.SimCardsOfUnitsWhichAreNotExistOnTrdBxOrWialon:
                {

                    diagnostics = await (from l in _context.LibyanaSimCards
                                             where !_context.TrackingUnits.Any(u => u.SimCard.SimCardNo == l.SimCardNo) &&
                                                   !_context.WialonUnits.Any(w => w.SimCardNo == l.SimCardNo)
                                             select new Diagnostic
                                             {
                                                 Account = null,
                                                 Client = null,
                                                 Customer = null,
                                                 UnitSNo = null,
                                                 SimCardNo = l.SimCardNo,
                                                 SimCardStatus = l.SimCardStatus,
                                                 StatusOnWialon = Domain.Enums.WStatus.Null,
                                                 StatusOnTrdBx = UStatus.Null,
                                                 WNote = null,
                                                 Balance = l.Balance,
                                                 LDExDate = l.DExDate,
                                                 LDOExpired = l.DOExpired
                                             }).OrderBy($"{request.OrderBy} {request.SortDirection}")
                                             .ProjectToPaginatedDataAsync(request.Specification,
                                                    request.PageNumber,
                                                    request.PageSize,
                                                    Mapper.ToDto,
                                                    cancellationToken);

                    break;
                }
            default:
                {
                    return null;
                } 
        }

       
        return diagnostics;


    }
}

//    public static class PaginatedDataHelper
//{
//    public static async Task<PaginatedData<T>> ToPaginatedDataAsync<T>(
//        this IQueryable<T> query,
//        int pageNumber,
//        int pageSize,
//        CancellationToken cancellationToken = default)
//    {
//        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
//        pageSize = pageSize <= 0 ? 10 : pageSize;


//        var totalCount = await query.CountAsync(cancellationToken);
//        var items = await query
//            .Skip((pageNumber - 1) * pageSize)
//            .Take(pageSize)
//            .ToListAsync(cancellationToken);

//        return new PaginatedData<T>(items, totalCount, pageNumber, pageSize);
//    }
//}





