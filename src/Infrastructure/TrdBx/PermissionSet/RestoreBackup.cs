using System.ComponentModel;
namespace CleanArchitecture.Blazor.Infrastructure.PermissionSet;

public static partial class Permissions
{
    [DisplayName("RestoreBackup Permissions")]
    [Description("Set permissions for Backup/Restore operations.")]
    public static class RestoreBackup
    {
        [Description("Allows viewing Backup file.")]
        public const string View = "Permissions.RestoreBackup.View";

        [Description("Allows creating new backup file.")]
        public const string Create = "Permissions.RestoreBackup.Create";

        [Description("Allows restore backup file.")]
        public const string Restore = "Permissions.RestoreBackup.Restore";

        [Description("Allows deleting Backup files.")]
        public const string Delete = "Permissions.RestoreBackup.Delete";
    }
}
public class RestoreBackupAccessRights
{
    public bool View { get; set; }
    public bool Create { get; set; }
    public bool Restore { get; set; }
    public bool Delete { get; set; }

}

