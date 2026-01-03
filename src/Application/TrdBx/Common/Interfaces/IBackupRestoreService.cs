
using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.BackupRestore.DTOs;

namespace CleanArchitecture.Blazor.Application.Common.Interfaces;
public interface IBackupRestoreService
{
    Task<List<BackupFileDto>> GetBackupsAsync();
    Task<bool> CreateBackupAsync(string backupName);
    Task<bool> RestoreBackupAsync(string backupName);
    //Task<string> GetBackupPathAsync();
    Task<Result<int>> DeleteBackupAsync(string backupName);
}
