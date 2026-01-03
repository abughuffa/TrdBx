namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.Summaries.Invoices.Caching;

public static class InvoiceSummaryCacheKey
{
    public const string GetCacheKey = "get-InvoiceSummary";

    public static IEnumerable<string> Tags => new string[] { "invoicesummary" };
    public static void Refresh()
    {
        FusionCacheFactory.RemoveByTags(Tags);
    }
}

