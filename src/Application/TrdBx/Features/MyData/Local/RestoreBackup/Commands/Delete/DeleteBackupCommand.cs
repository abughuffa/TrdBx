using System.Runtime.CompilerServices;

namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.RestoreBackup.Commands.Delete;

public record DeleteBackupCommand(string BackupName) : IRequest<Result<int>>;

public class DeleteBackupCommandHandler : IRequestHandler<DeleteBackupCommand, Result<int>>
{
    private readonly IBackupRestoreService _service;

    public DeleteBackupCommandHandler(IBackupRestoreService service)
    {
        _service = service;
    }

    public async ValueTask<Result<int>> Handle(DeleteBackupCommand request, CancellationToken cancellationToken)
    {
        return await Result<int>.SuccessAsync(await _service.DeleteBackupAsync(request.BackupName));
    }
}