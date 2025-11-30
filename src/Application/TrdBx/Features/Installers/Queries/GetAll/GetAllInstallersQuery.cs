using CleanArchitecture.Blazor.Application.Common.Interfaces.Identity;
using CleanArchitecture.Blazor.Application.Features.Installers.DTOs;
using CleanArchitecture.Blazor.Application.Features.Installers.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.Installers.Queries.GetAll;

public class GetAvaliableInstallersQuery : IRequest<IEnumerable<InstallerDto>>
{
   //public string CacheKey => InstallerCacheKey.GetAvaliableCacheKey;
   //public IEnumerable<string> Tags => InstallerCacheKey.Tags;
}

public class GetAvaliableInstallersQueryHandler :
     IRequestHandler<GetAvaliableInstallersQuery, IEnumerable<InstallerDto>>
{

    private readonly IIdentityService _userService;

    //private readonly IMapper _mapper;
    //public GetAvaliableInstallersQueryHandler(
    //    IIdentityService userService,
    //    IMapper mapper)
    //{
    //    _mapper = mapper;
    //    _userService = userService;
    //}

    public GetAvaliableInstallersQueryHandler(
      IIdentityService userService)
    {
        _userService = userService;
    }


    public async Task<IEnumerable<InstallerDto>> Handle(GetAvaliableInstallersQuery request, CancellationToken cancellationToken)
    {
        //var data = _userService.DataSource.Where(x => x.IsActive && x.AssignedRoles.Contains("Installer"));

        var data = await _userService.GetUsers(null, cancellationToken);

         var result = data.Where(x => x.IsActive && x.AssignedRoles.Contains("Installer"));

        return result.XProjectTo();
    }
}
