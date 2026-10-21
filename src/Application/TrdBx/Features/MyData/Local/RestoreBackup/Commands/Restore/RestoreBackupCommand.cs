namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.RestoreBackup.Commands.Restore;

public record RestoreBackupCommand(string BackupName) : IRequest<Result<bool>>;

public class RestoreBackupCommandHandler : IRequestHandler<RestoreBackupCommand, Result<bool>>
{
    private readonly IBackupRestoreService _service;
    public RestoreBackupCommandHandler(IBackupRestoreService service)
    {
        _service = service;
    }
    public async ValueTask<Result<bool>> Handle(RestoreBackupCommand request, CancellationToken cancellationToken)
    {
        var result = await _service.RestoreBackupAsync(request.BackupName);

        return await Result<bool>.SuccessAsync(result);
     

        
    }
}
