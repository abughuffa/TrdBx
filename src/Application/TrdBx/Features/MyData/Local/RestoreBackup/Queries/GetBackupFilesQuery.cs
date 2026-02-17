using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.RestoreBackup.DTOs;

namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.RestoreBackup.Queries;

public record GetBackupsQuery : IRequest<List<RestoreBackupFileDto>>;

public class GetBackupsQueryHandler : IRequestHandler<GetBackupsQuery, List<RestoreBackupFileDto>>
{
    private readonly IBackupRestoreService _service;

    public GetBackupsQueryHandler(IBackupRestoreService service)
    {
        _service = service;
    }

    public Task<List<RestoreBackupFileDto>> Handle(GetBackupsQuery request, CancellationToken cancellationToken)
    {
        return _service.GetBackupsAsync();
    }
}
