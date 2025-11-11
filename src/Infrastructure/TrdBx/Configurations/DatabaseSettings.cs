//using System.ComponentModel.DataAnnotations;

//namespace CleanArchitecture.Blazor.Infrastructure.Configurations;

///// <summary>
/////     Configuration wrapper for the database section
///// </summary>
//public partial class DatabaseSettings : IValidatableObject
//{

//    /// <summary>
//    ///     The Backup Settings being used to backup / restore database
//    /// </summary>
//    public BackupSettings BackupSettings { get; set; } = null;

//    private IEnumerable<ValidationResult> TrdBxValidate(ValidationContext validationContext)
//    {
 
//        if (BackupSettings.Path is null)
//            yield return new ValidationResult(
//                $"{nameof(BackupSettings.Path)}.{nameof(BackupSettings.Path)} is not configured",
//                new[] { nameof(BackupSettings.Path) });
//        if (BackupSettings.RetentionDays == 0)
//            yield return new ValidationResult(
//                $"{nameof(BackupSettings.RetentionDays)}.{nameof(BackupSettings.RetentionDays)} is not configured",
//                new[] { nameof(BackupSettings.RetentionDays) });
//    }
//}

//public class BackupSettings
//{
//    /// <summary>
//    ///     Represents the Path of database backup files
//    /// </summary>
//    public string Path { get; set; } = string.Empty;
//    /// <summary>
//    ///     Represents Retention Days of database backup files to delete
//    /// </summary>
//    public int RetentionDays { get; set; } = 30;


//}