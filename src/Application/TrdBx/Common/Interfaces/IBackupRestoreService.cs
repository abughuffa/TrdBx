
using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.RestoreBackup.DTOs;

namespace CleanArchitecture.Blazor.Application.Common.Interfaces;
public interface IBackupRestoreService
{
    Task<List<RestoreBackupFileDto>> GetBackupsAsync();
    Task<bool> CreateBackupAsync(string backupName);
    Task<bool> RestoreBackupAsync(string backupName);
    //Task<string> GetBackupPathAsync();
    Task<Result<int>> DeleteBackupAsync(string backupName);
}
