namespace CleanArchitecture.Blazor.Domain.Entities;
public class BackupFile
{
    public string? Name { get; set; }
    public string? Path { get; set; }
    public long? Size { get; set; }
    public DateTime? Created { get; set; }
}
