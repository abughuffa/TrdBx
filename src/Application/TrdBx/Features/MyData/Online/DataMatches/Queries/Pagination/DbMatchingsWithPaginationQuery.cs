// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.DataMatches.Mappers;
using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.DataMatches.Caching;
using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.DataMatches.DTOs;
using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.DataMatches.Specifications;

namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.DataMatches.Queries.Pagination;


public class DataMatchesWithPaginationQuery : DataMatchAdvancedFilter, ICacheableRequest<PaginatedData<DataMatchDto>>
{
    public IEnumerable<string>? Tags => DataMatchCacheKey.Tags;
    public DataMatchAdvancedSpecification Specification => new(this);
    public string CacheKey => DataMatchCacheKey.GetPaginationCacheKey($"{this}");
    public override string ToString()
    {
        return $"Listview:{ListView}, Search:{Keyword},StatusOnWialon:{StatusOnWialon},StatusOnTrdBx:{StatusOnTrdBx}, SortDirection:{SortDirection}, OrderBy:{OrderBy}, {PageNumber}, {PageSize}";
    }

}

public class DataMatchesWithPaginationQueryHandler :
         IRequestHandler<DataMatchesWithPaginationQuery, PaginatedData<DataMatchDto>?>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public DataMatchesWithPaginationQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    public DataMatchesWithPaginationQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedData<DataMatchDto>?> Handle(DataMatchesWithPaginationQuery request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        PaginatedData<DataMatchDto>  data;

        switch (request.ListView)
        {
            case DataMatchListView.MatchedBySimCardOnly:
                {

                    data = await (from w in _context.WialonUnits
                                  join t in _context.TrackingUnits on w.SimCardNo equals t.SimCard.SimCardNo
                                  where w.UnitSNo != t.SNo && w.SimCardNo != null && t.SimCard != null
                                  select new DataMatch
                                  {
                                      Account = w.Account,
                                      Client = t.Customer.Parent != null ? t.Customer.Parent.Name : null,
                                      Customer = t.Customer.Name,
                                      WUnitSNo = w.UnitSNo,
                                      TUnitSNo = t.SNo,
                                      WSimCardNo = w.SimCardNo,
                                      TSimCardNo = t.SimCard.SimCardNo,
                                      StatusOnWialon = w.StatusOnWialon,
                                      StatusOnTrdBx = t.UStatus,
                                      WNote = w.Note
                                  }).OrderBy($"{request.OrderBy} {request.SortDirection}")
                                                  .ProjectToPaginatedDataAsync(request.Specification,
                                                    request.PageNumber,
                                                    request.PageSize,
                                                    Mapper.ToDto,
                                                    cancellationToken);
                    break;
                }
            case DataMatchListView.MatchedByUnitOnly:
                {
                    data = await (from w in _context.WialonUnits
                    join t in _context.TrackingUnits on w.UnitSNo equals t.SNo
                    where w.SimCardNo != t.SimCard.SimCardNo && w.UnitSNo != null && t.SimCard != null
                 select new DataMatch
                 {
                     Account = w.Account,
                     Client = t.Customer.Parent != null ? t.Customer.Parent.Name : null,
                     Customer = t.Customer.Name,
                     WUnitSNo = w.UnitSNo,
                     TUnitSNo = t.SNo,
                     WSimCardNo = w.SimCardNo,
                     TSimCardNo = t.SimCard.SimCardNo,
                     StatusOnWialon = w.StatusOnWialon,
                     StatusOnTrdBx = t.UStatus,
                     WNote = w.Note
                 }).OrderBy($"{request.OrderBy} {request.SortDirection}")
                                                   .ProjectToPaginatedDataAsync(request.Specification,
                                                    request.PageNumber,
                                                    request.PageSize,
                                                    Mapper.ToDto,
                                                    cancellationToken);
                    break;
                }
            case DataMatchListView.MatchedByUnitAndSimCard:
                {

        data = await (from w in _context.WialonUnits
                      join t in _context.TrackingUnits on w.UnitSNo equals t.SNo
                      where w.SimCardNo == t.SimCard.SimCardNo && w.UnitSNo != null && t.SimCard != null
                      select new DataMatch
                      {
                          Account = w.Account,
                          Client = t.Customer.Parent != null ? t.Customer.Parent.Name : null,
                          Customer = t.Customer.Name,
                          WUnitSNo = w.UnitSNo,
                          TUnitSNo = t.SNo,
                          WSimCardNo = w.SimCardNo,
                          TSimCardNo = t.SimCard.SimCardNo,
                          StatusOnWialon = w.StatusOnWialon,
                          StatusOnTrdBx = t.UStatus,
                          WNote = w.Note
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


        return data;
    }
}





