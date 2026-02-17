namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.RestoreBackup.Commands.Restore;

public record RestoreBackupCommand(string BackupName) : IRequest<bool>;

public class RestoreBackupCommandHandler : IRequestHandler<RestoreBackupCommand, bool>
{
    private readonly IBackupRestoreService _service;
    public RestoreBackupCommandHandler(IBackupRestoreService service)
    {
        _service = service;
    }
    public async Task<bool> Handle(RestoreBackupCommand request, CancellationToken cancellationToken)
    {
        var result = await _service.RestoreBackupAsync(request.BackupName);

        return result;
    }
}
