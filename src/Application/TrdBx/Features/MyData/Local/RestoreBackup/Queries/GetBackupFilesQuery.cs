using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.RestoreBackup.DTOs;

namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.RestoreBackup.Queries;

public record GetBackupsQuery : IRequest<Result<List<RestoreBackupFileDto>>>;

public class GetBackupsQueryHandler : IRequestHandler<GetBackupsQuery, Result<List<RestoreBackupFileDto>>>
{
    private readonly IBackupRestoreService _service;

    public GetBackupsQueryHandler(IBackupRestoreService service)
    {
        _service = service;
    }

    public async ValueTask<Result<List<RestoreBackupFileDto>>> Handle(GetBackupsQuery request, CancellationToken cancellationToken)
    {
        var backups = await _service.GetBackupsAsync();
        return Result<List<RestoreBackupFileDto>>.Success(backups);
    }
}
