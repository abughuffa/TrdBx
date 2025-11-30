// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.WialonUnits.DTOs;
using CleanArchitecture.Blazor.Application.Features.WialonUnits.Mappers;
using CleanArchitecture.Blazor.Application.Features.WialonUnits.Specifications;


namespace CleanArchitecture.Blazor.Application.Features.WialonUnits.Queries.Export;

public class ExportWialonUnitsQuery : WialonUnitAdvancedFilter, IRequest<Result<byte[]>>
{
    public WialonUnitAdvancedSpecification Specification => new(this);
}

public class ExportWialonUnitsQueryHandler :
         IRequestHandler<ExportWialonUnitsQuery, Result<byte[]>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //private readonly IExcelService _excelService;
    //private readonly IStringLocalizer<ExportWialonUnitsQueryHandler> _localizer;
    //private readonly WialonUnitDto _dto = new();
    //public ExportWialonUnitsQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper,
    //    IExcelService excelService,
    //    IStringLocalizer<ExportWialonUnitsQueryHandler> localizer
    //    )
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //    _excelService = excelService;
    //    _localizer = localizer;
    //}
    private readonly IApplicationDbContext _context;
    private readonly IExcelService _excelService;
    private readonly IStringLocalizer<ExportWialonUnitsQueryHandler> _localizer;
    private readonly WialonUnitDto _dto = new();
    public ExportWialonUnitsQueryHandler(
        IApplicationDbContext context,
        IExcelService excelService,
        IStringLocalizer<ExportWialonUnitsQueryHandler> localizer
        )
    {
        _context = context;
        _excelService = excelService;
        _localizer = localizer;
    }

#nullable disable warnings
    public async Task<Result<byte[]>> Handle(ExportWialonUnitsQuery request, CancellationToken cancellationToken)
    {
        byte[] result;
        List<WialonUnitDto> data;
        Dictionary<string, Func<WialonUnitDto, object?>> mappers;

        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        mappers = new Dictionary<string, Func<WialonUnitDto, object?>>
                {
                     {_localizer[_dto.GetMemberDescription(x=>x.UnitName)],item => item.UnitName},
                                    {_localizer[_dto.GetMemberDescription(x=>x.Account)],item => item.Account},
                                    {_localizer[_dto.GetMemberDescription(x=>x.UnitSNo)],item => item.UnitSNo},
                                    {_localizer[_dto.GetMemberDescription(x=>x.SimCardNo)],item => item.SimCardNo},
                                    {_localizer[_dto.GetMemberDescription(x=>x.Deactivation)],item => item.Deactivation},
                                    {_localizer[_dto.GetMemberDescription(x=>x.StatusOnWialon)],item => item.StatusOnWialon},
                                    {_localizer[_dto.GetMemberDescription(x=>x.Note)],item => item.Note}
                };


        switch (request.ListView)
        {
            case WialonUnitListView.All:
                {
                    //data = await _context.WialonUnits.ApplySpecification(request.Specification)
                    //                              .OrderBy($"{request.OrderBy} {request.SortDirection}")
                    //                                .AsNoTracking()
                    //     .ProjectTo<WialonUnitDto>(_mapper.ConfigurationProvider)
                    //     .ToListAsync(cancellationToken);
                    data = await _context.WialonUnits.ApplySpecification(request.Specification)
    .OrderBy($"{request.OrderBy} {request.SortDirection}")
    .ProjectTo()
    .AsNoTracking()
    .ToListAsync(cancellationToken);

                    break;
                }
            case WialonUnitListView.UnitsNotExistOnTrdBx:
                {
                    var tUnitSNo = await _context.TrackingUnits.Select(o => o.SNo).AsNoTracking().ToListAsync(cancellationToken);

                    //data = await _context.WialonUnits.Where(o => !tUnitSNo.Contains(o.UnitSNo))
                    //          .ApplySpecification(request.Specification)
                    //          .OrderBy($"{request.OrderBy} {request.SortDirection}")
                    //          .AsNoTracking()
                    //     .ProjectTo<WialonUnitDto>(_mapper.ConfigurationProvider)

                    //.ToListAsync(cancellationToken);

                    data = await _context.WialonUnits.Where(o => !tUnitSNo.Contains(o.UnitSNo))
                        .ApplySpecification(request.Specification)
.OrderBy($"{request.OrderBy} {request.SortDirection}")
.ProjectTo()
.AsNoTracking()
.ToListAsync(cancellationToken);

                    break;
                }
            case WialonUnitListView.UnitsWithSimCardNotExistOnLibyana:
                {
                    var lSimCards = await _context.LibyanaSimCards.Select(o => o.SimCardNo).AsNoTracking().ToListAsync(cancellationToken);

                    //data = await _context.WialonUnits.Where(o => !lSimCards.Contains(o.SimCardNo))
                    //     .ApplySpecification(request.Specification)
                    //     .OrderBy($"{request.OrderBy} {request.SortDirection}")
                    //     .AsNoTracking()
                    //     .ProjectTo<WialonUnitDto>(_mapper.ConfigurationProvider)
                    //     .ToListAsync(cancellationToken);

                    data = await _context.WialonUnits.Where(o => !lSimCards.Contains(o.SimCardNo))
            .ApplySpecification(request.Specification)
.OrderBy($"{request.OrderBy} {request.SortDirection}")
.ProjectTo()
.AsNoTracking()
.ToListAsync(cancellationToken);

                    break;
                }
            default:
                {
                    data = new List<WialonUnitDto>();
                    break;
                }
        }

        result = await _excelService.ExportAsync(data, mappers, _localizer[_dto.GetClassDescription()]);
        return await Result<byte[]>.SuccessAsync(result);


    }
}
