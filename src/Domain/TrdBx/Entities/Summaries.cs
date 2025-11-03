namespace CleanArchitecture.Blazor.Domain.Entities;

public class ServiceLogSummary 
{
    public int Checks { get; set; } = 0;
    public int Installs { get; set; } = 0;
    public int Replaces { get; set; } = 0;
    public int Supports { get; set; } = 0;
    public int Subscriptions { get; set; } = 0;
    public int Renews { get; set; } = 0;
    public int Counts { get; set; } = 0;
}


public class SimCardSummary
{
    public int News { get; set; } = 0;
    public int Installeds { get; set; } = 0;
    public int Recovereds { get; set; } = 0;
    public int Useds { get; set; } = 0;
    public int Losts { get; set; } = 0;
    public int Counts { get; set; } = 0;

}


public class InvoiceSummary
{
    public int Drafts { get; set; } = 0;
    public int SentToTaxs { get; set; } = 0;
    public int Readys { get; set; } = 0;
    public int Billeds { get; set; } = 0;
    public int Paids { get; set; } = 0;
    public int Canceleds { get; set; } = 0;
    public int Counts { get; set; } = 0;

}

public class TrackingUnitSummary
{
    public int News { get; set; } = 0;
    public int Reserveds { get; set; } = 0;
    public int InstalledActiveGprss { get; set; } = 0;
    public int InstalledActiveHostings { get; set; } = 0;
    public int InstalledActives { get; set; } = 0;
    public int InstalledInactives { get; set; } = 0;
    public int Recovereds { get; set; } = 0;
    public int Useds { get; set; } = 0;
    public int Damageds { get; set; } = 0;
    public int Losts { get; set; } = 0;
    public int Counts { get; set; } = 0;

}