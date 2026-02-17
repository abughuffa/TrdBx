namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.RestoreBackup.Commands.Create;

public record CreateBackupCommand(string BackupName) : IRequest<bool>;


public class CreateBackupCommandHandler : IRequestHandler<CreateBackupCommand, bool>
{
    private readonly IBackupRestoreService _service;
    public CreateBackupCommandHandler(IBackupRestoreService service)
    {
        _service = service;
    }
    public async Task<bool> Handle(CreateBackupCommand request, CancellationToken cancellationToken)
    {
        return await _service.CreateBackupAsync(request.BackupName);
    }
}
