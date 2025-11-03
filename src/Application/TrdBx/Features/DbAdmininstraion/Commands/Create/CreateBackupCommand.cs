namespace CleanArchitecture.Blazor.Application.Features.DbAdmininstraion.Commands.Create;

public record CreateBackupCommand(string BackupName) : IRequest<bool>;


public class CreateBackupCommandHandler : IRequestHandler<CreateBackupCommand, bool>
{
    private readonly IBackupRestoreService _service;
    public CreateBackupCommandHandler(IBackupRestoreService service)
    {
        _service = service;
    }
    public Task<bool> Handle(CreateBackupCommand request, CancellationToken cancellationToken)
    {
        return _service.CreateBackupAsync(request.BackupName);
    }
}
