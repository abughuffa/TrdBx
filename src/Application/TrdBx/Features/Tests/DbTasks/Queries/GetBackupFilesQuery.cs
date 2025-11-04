using CleanArchitecture.Blazor.Application.Features.DbTasks.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.DbTasks.Queries;

public record GetBackupsQuery : IRequest<List<BackupFileDto>>;

public class GetBackupsQueryHandler : IRequestHandler<GetBackupsQuery, List<BackupFileDto>>
{
    private readonly IBackupRestoreService _service;

    public GetBackupsQueryHandler(IBackupRestoreService service)
    {
        _service = service;
    }

    public Task<List<BackupFileDto>> Handle(GetBackupsQuery request, CancellationToken cancellationToken)
    {
        return _service.GetBackupsAsync();
    }
}
