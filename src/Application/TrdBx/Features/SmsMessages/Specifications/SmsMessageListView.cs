using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.SmsMessages.Specifications;

#nullable disable warnings
/// <summary>
/// Specifies the different views available for the SimCard list.
/// </summary>
public enum SmsMessageListView
{
    [Description("All")]
    All,
    [Description("Created Toady")]
    TODAY,
    [Description("Created within the last 30 days")]
    LAST_30_DAYS
}
/// <summary>
/// A class for applying advanced filtering options to SimCard lists.
/// </summary>
public class SmsMessageAdvancedFilter: PaginationFilter
{
    
    

    public SmsStatus SmsStatus { get; set; }
    public TimeSpan LocalTimezoneOffset { get; set; }
    public SmsMessageListView ListView { get; set; } = SmsMessageListView.All;
    //public UserProfile? CurrentUser { get; set; }
}