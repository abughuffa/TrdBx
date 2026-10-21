using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.RestoreBackup.DTOs;
using Microsoft.Data.SqlClient;

namespace CleanArchitecture.Blazor.Infrastructure.Services.RestoreBackupStrategies;
public class SqlServerBackupRestoreStrategy : IDatabaseBackupRestoreStrategy
{
    public string BackupFileExtension => ".bak";

    public async Task<bool> CreateBackupAsync(string connectionString, string backupPath, string backupName)
    {
        return await Task.FromResult(false);


    }

    public async Task<bool> RestoreBackupAsync(string connectionString, string backupPath, string backupName)
    {
        return await Task.FromResult(false);
    }

    public async Task<List<RestoreBackupFileDto>> GetBackupsAsync(string backupPath)
    {
        if (!Directory.Exists(backupPath))
            Directory.CreateDirectory(backupPath);

        var backupFiles = new List<RestoreBackupFileDto>();

        foreach (var file in Directory.GetFiles(backupPath).Where(f => f.EndsWith(BackupFileExtension)))
        {
            var fileInfo = new FileInfo(file);
            backupFiles.Add(new RestoreBackupFileDto
            {
                Name = Path.GetFileName(file),
                Path = file,
                Size = fileInfo.Length,
                Created = fileInfo.CreationTime
            });
        }

        return await Task.FromResult(backupFiles.OrderByDescending(f => f.Created).ToList());
    }
}
