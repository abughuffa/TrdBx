using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.SmsConfigTemplates.Specifications;

#nullable disable warnings
/// <summary>
/// Specifies the different views available for the SmsConfigTemplate list.
/// </summary>
public enum SmsConfigTemplateListView
{
    [Description("All")]
    All,
    [Description("Created Toady")]
    TODAY,
    [Description("Created within the last 30 days")]
    LAST_30_DAYS
}
/// <summary>
/// A class for applying advanced filtering options to SmsConfigTemplate lists.
/// </summary>
public class SmsConfigTemplateAdvancedFilter: PaginationFilter
{

    public int? TrackingUnitModelId { get; set; } = null;
    public TimeSpan LocalTimezoneOffset { get; set; }
    public SmsConfigTemplateListView ListView { get; set; } = SmsConfigTemplateListView.All;

}