// Application.Common.Interfaces
using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.BackupRestore.DTOs;

namespace CleanArchitecture.Blazor.Application.Common.Interfaces;
public interface IDatabaseBackupRestoreStrategy
{
    Task<bool> CreateBackupAsync(string connectionString, string backupPath, string backupName);
    Task<bool> RestoreBackupAsync(string connectionString, string backupPath, string backupName);
    Task<List<BackupFileDto>> GetBackupsAsync(string backupPath);
    string BackupFileExtension { get; }
}





