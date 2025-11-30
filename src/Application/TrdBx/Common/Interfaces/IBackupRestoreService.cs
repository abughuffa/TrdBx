using CleanArchitecture.Blazor.Application.Common.Models;
using CleanArchitecture.Blazor.Application.Features.DbAdmininstraion.DTOs;

namespace CleanArchitecture.Blazor.Application.Common.Interfaces;
public interface IBackupRestoreService
{
    Task<List<BackupFileDto>> GetBackupsAsync();
    Task<bool> CreateBackupAsync(string backupName);
    Task<Result> RestoreBackupAsync(string backupName);
    Task<string> GetBackupPathAsync();
    Task<Result<int>> DeleteBackupAsync(string backupName);
}
