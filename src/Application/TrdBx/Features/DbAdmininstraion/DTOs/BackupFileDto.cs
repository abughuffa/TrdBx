namespace CleanArchitecture.Blazor.Application.Features.DbAdmininstraion.DTOs;
public class BackupFileDto
{
    public required string Name { get; set; }
    public required string Path { get; set; }
    public long Size { get; set; }
    public DateTime Created { get; set; }
    public string FormattedSize => FormatSize(Size);
    public string FormattedDate => Created.ToString("yyyy-MM-dd HH:mm:ss");

    private static string FormatSize(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB", "TB" };
        var order = 0;
        double len = bytes;
        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len /= 1024;
        }
        return $"{len:0.##} {sizes[order]}";
    }
}
