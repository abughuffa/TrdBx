namespace CleanArchitecture.Blazor.Application.Features.DbAdmininstraion.Commands.Restore;

public record RestoreBackupCommand(string BackupName) : IRequest<Result>;

public class RestoreBackupCommandHandler : IRequestHandler<RestoreBackupCommand, Result>
{
    private readonly IBackupRestoreService _service;
    public RestoreBackupCommandHandler(IBackupRestoreService service)
    {
        _service = service;
    }
    public Task<Result> Handle(RestoreBackupCommand request, CancellationToken cancellationToken)
    {
        return _service.RestoreBackupAsync(request.BackupName);
    }
}
