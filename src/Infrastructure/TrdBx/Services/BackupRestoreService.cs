// Infrastructure.Services
using CleanArchitecture.Blazor.Application.Features.DbAdmininstraion.DTOs;
using CleanArchitecture.Blazor.Infrastructure.Configurations;
using Microsoft.AspNetCore.Hosting;

namespace CleanArchitecture.Blazor.Infrastructure.Services;
public class BackupRestoreService : IBackupRestoreService
{
    private readonly IDatabaseBackupRestoreStrategy _strategy;
    private readonly IWebHostEnvironment _environment;
    private readonly DatabaseSettings _databaseSettings;
    private readonly ILogger<BackupRestoreService> _logger;

    public BackupRestoreService(
        IDatabaseBackupRestoreStrategy strategy,
        IWebHostEnvironment environment,
        IOptions<DatabaseSettings> databaseSettings,
        ILogger<BackupRestoreService> logger)
    {
        _strategy = strategy;
        _environment = environment;
        _databaseSettings = databaseSettings.Value;
        _logger = logger;
    }

    public async Task<List<BackupFileDto>> GetBackupsAsync()
    {
        try
        {
            var backupPath = GetBackupPath();
            return await _strategy.GetBackupsAsync(backupPath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving backups");
            throw;
        }
    }

    public async Task<bool> CreateBackupAsync(string backupName)
    {
        try
        {
            var backupPath = GetBackupPath();
            var result = await _strategy.CreateBackupAsync(
                _databaseSettings.ConnectionString,
                backupPath,
                backupName);

            if (result)
            {
                _logger.LogInformation("Backup created successfully: {BackupName}", backupName);

                return true;
            }
            else
            {
                _logger.LogError("Error creating backup: {BackupName}", backupName);
                return false;
            }
               

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating backup: {BackupName}", backupName);
            return false;
        }
    }

    public async Task<Result> RestoreBackupAsync(string backupName)
    {
        try
        {
            var backupPath = GetBackupPath();
            var result = await _strategy.RestoreBackupAsync(
                _databaseSettings.ConnectionString,
                backupPath,
                backupName);

            if (result)
            {
                _logger.LogInformation("Backup restored successfully: {BackupName}", backupName);
                return await Result.SuccessAsync();
            }
               

            _logger.LogError("Error restoring backup: {BackupName}", backupName);
            return await Result.FailureAsync("Restore Faild!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error restoring backup: {BackupName}", backupName);
            return await Result.FailureAsync("Restore Faild!");
        }
    }

    public async Task<Result<int>> DeleteBackupAsync(string backupName)
    {
        try
        {
            var backupPath = GetBackupPath();
            var backupFile = Path.Combine(backupPath, backupName);

            if (File.Exists(backupFile))
            {
                File.Delete(backupFile);
                _logger.LogInformation("Backup deleted successfully: {BackupName}", backupName);
               return await Result<int>.SuccessAsync(1);
            }

            _logger.LogWarning("Backup file not found: {BackupName}", backupName);
            return await Result<int>.FailureAsync("Deleted Faild!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting backup: {BackupName}", backupName);
            return await Result<int>.FailureAsync("Deleted Faild!");
        }
    }

    public async Task<string> GetBackupPathAsync()
    {
        return await Task.FromResult(GetBackupPath());
    }

    private string GetBackupPath()
    {
        var basePath = _databaseSettings.BackupSettings?.Path ?? "Backups";

        // If it's a relative path, combine with content root
        if (!Path.IsPathRooted(basePath))
            basePath = Path.Combine(_environment.ContentRootPath, basePath);

        return basePath;
    }
}