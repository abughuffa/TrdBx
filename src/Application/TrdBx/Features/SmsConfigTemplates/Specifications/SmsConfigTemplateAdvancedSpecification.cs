

namespace CleanArchitecture.Blazor.Application.Features.SmsConfigTemplates.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for advanced filtering of SmsConfigTemplates.
/// </summary>
public class SmsConfigTemplateAdvancedSpecification : Specification<SmsConfigTemplate>
{
    public SmsConfigTemplateAdvancedSpecification(SmsConfigTemplateAdvancedFilter filter)
    {
        var today = DateTime.UtcNow;
        var todayrange = today.GetDateRange(SmsConfigTemplateListView.TODAY.ToString(), filter.LocalTimezoneOffset);
        var last30daysrange = today.GetDateRange(SmsConfigTemplateListView.LAST_30_DAYS.ToString(),filter.LocalTimezoneOffset);

        Query.Where(q => q.Name != null)
             .Where(filter.Keyword, !string.IsNullOrEmpty(filter.Keyword))
             .Where(x => x.TrackingUnitModelId.Equals(filter.TrackingUnitModelId), filter.TrackingUnitModelId is not null)
             .Where(x => x.CreatedAt >= todayrange.Start && x.CreatedAt < todayrange.End.AddDays(1), filter.ListView == SmsConfigTemplateListView.TODAY)
             .Where(x => x.CreatedAt >= last30daysrange.Start, filter.ListView == SmsConfigTemplateListView.LAST_30_DAYS);
       
    }
}
