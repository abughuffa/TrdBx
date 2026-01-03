
namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.Charts.DTOs;

[Description("Chart")]
public class ChartDto
{
    [Description("Date")]
    public DateOnly Date { get; set; }

    [Description("Items")]
    public List<string>? Items { get; set; }


}


