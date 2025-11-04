namespace CleanArchitecture.Blazor.Application.Features.DbTasks.Commands.Delete;
// Application.Features.TrdBx.DbAdministration.Commands.DeleteBackup
public record DeleteBackupCommand(string BackupName) : IRequest<Result>;

public class DeleteBackupCommandHandler : IRequestHandler<DeleteBackupCommand, Result>
{
    private readonly IBackupRestoreService _service;

    public DeleteBackupCommandHandler(IBackupRestoreService service)
    {
        _service = service;
    }

    public Task<Result> Handle(DeleteBackupCommand request, CancellationToken cancellationToken)
    {
        return _service.DeleteBackupAsync(request.BackupName);
    }
}