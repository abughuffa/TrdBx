namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.RestoreBackup.Commands.Create;

public record CreateBackupCommand(string BackupName) : IRequest<Result<bool>>;


public class CreateBackupCommandHandler : IRequestHandler<CreateBackupCommand, Result<bool>>
{
    private readonly IBackupRestoreService _service;
    public CreateBackupCommandHandler(IBackupRestoreService service)
    {
        _service = service;
    }
    public async ValueTask<Result<bool>> Handle(CreateBackupCommand request, CancellationToken cancellationToken)
    {
 
        var result = await _service.CreateBackupAsync(request.BackupName);

        return await Result<bool>.SuccessAsync(result);

    }
}
