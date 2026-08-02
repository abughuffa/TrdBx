namespace CleanArchitecture.Blazor.Domain.Entities;
public class BackupFile
{
    public string? Name { get; set; }
    public string? Path { get; set; }
    public long? Size { get; set; }
    public DateTime? Created { get; set; }
}

public class CPrice
    {
        public int TrackingUnitModelId { get; set; }
        public decimal Price { get; set; } = 0.0m;
        public decimal Host { get; set; } = 0.0m;
        public decimal Gprs { get; set; } = 0.0m;
    }

public class InvItem
{
    public int Serial { get; set; }
    public int SubSerial { get; set; }
    public string? Description { get; set; }
    public decimal SubTotal { get; set; } = 0.0m;
    public decimal ItemTotal { get; set; } = 0.0m;

}