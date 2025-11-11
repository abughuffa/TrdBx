// Application.Common.Interfaces
using CleanArchitecture.Blazor.Application.Features.DbTasks.DTOs;

namespace CleanArchitecture.Blazor.Application.Common.Interfaces;
public interface IDatabaseBackupRestoreStrategy
{
    Task<bool> CreateBackupAsync(string? backupName);
    Task<bool> RestoreBackupAsync(string backupName);
    Task<List<BackupFileDto>> GetBackupsAsync();
    string BackupFileExtension { get; }
}





