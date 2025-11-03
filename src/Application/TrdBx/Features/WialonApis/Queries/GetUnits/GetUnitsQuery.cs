//using CleanArchitecture.Blazor.Application.Common.Interfaces.TrdBx;
//using CleanArchitecture.Blazor.Application.Features.WialonApis.Models;

//public record GetUnitsQuery : IRequest<UnitListResponse>;

//public class GetUnitsQueryHandler : IRequestHandler<GetUnitsQuery, UnitListResponse>
//{
//    private readonly IXWialonService _wialonService;

//    public GetUnitsQueryHandler(IXWialonService wialonService)
//    {
//        _wialonService = wialonService;
//    }

//    public async Task<UnitListResponse> Handle(GetUnitsQuery request, CancellationToken cancellationToken)
//    {
//        return await _wialonService.ExecuteRequestAsync<UnitListResponse>(
//            svc: "core/search_items",
//            parameters: new
//            {
//                spec = new
//                {
//                    itemsType = "avl_unit",
//                    propName = "sys_name",
//                    propValueMask = "*",
//                    sortType = "sys_name"
//                },
//                force = 1,
//                flags = 1,
//                from = 0,
//                to = 50
//            });
//    }
//}