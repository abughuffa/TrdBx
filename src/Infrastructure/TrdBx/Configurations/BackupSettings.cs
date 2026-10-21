using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Blazor.Infrastructure.Configurations;


public class BackupSettings
{
    /// <summary>
    ///     Represents the Path of database backup files
    /// </summary>
    public string Path { get; set; } = string.Empty;
    /// <summary>
    ///     Represents Retention Days of database backup files to delete
    /// </summary>
    public int RetentionDays { get; set; } = 30;


}