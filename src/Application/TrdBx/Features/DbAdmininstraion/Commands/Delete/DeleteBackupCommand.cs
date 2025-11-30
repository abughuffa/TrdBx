namespace CleanArchitecture.Blazor.Application.Features.DbAdmininstraion.Commands.Delete;
// Application.Features.TrdBx.DbAdministration.Commands.DeleteBackup
public record DeleteBackupCommand(string BackupName) : IRequest<Result<int>>;

public class DeleteBackupCommandHandler : IRequestHandler<DeleteBackupCommand, Result<int>>
{
    private readonly IBackupRestoreService _service;

    public DeleteBackupCommandHandler(IBackupRestoreService service)
    {
        _service = service;
    }

    public Task<Result<int>> Handle(DeleteBackupCommand request, CancellationToken cancellationToken)
    {
        return _service.DeleteBackupAsync(request.BackupName);
    }
}