using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Blazor.Infrastructure.Configurations;

/// <summary>
///     Configuration wrapper for the database section
/// </summary>
public partial class DatabaseSettings : IValidatableObject
{

    /// <summary>
    ///     The Backup Settings being used to backup / restore database
    /// </summary>
    public BackupSettings BackupSettings { get; set; } = null;

    private IEnumerable<ValidationResult> TrdBxValidate(ValidationContext validationContext)
    {
 
        if (BackupSettings is null)
            yield return new ValidationResult(
                $"{nameof(BackupSettings)}.{nameof(BackupSettings)} is not configured",
                new[] { nameof(BackupSettings) });
    }
}

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