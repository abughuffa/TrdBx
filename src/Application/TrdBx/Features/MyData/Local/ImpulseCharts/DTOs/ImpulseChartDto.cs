
namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.ImpulseCharts.DTOs;

[Description("ImpulseChart")]
public class ImpulseChartDto
{
    [Description("Date")]
    public DateOnly Date { get; set; }

    //[Description("Items")]
    //public List<string>? Items { get; set; }

    [Description("Items")]
    public List<ItemDto>? Items { get; set; }


}

public class ItemDto
{

    [Description("Id")]
    public int Id { get; set; } = 0;

    [Description("ParentName")]
    public string ParentName { get; set; } = string.Empty;

    [Description("ChildName")]
    public string ChildName { get; set; } = string.Empty;

    [Description("SNo")]
    public string SNo { get; set; } = string.Empty;

    [Description("SimNo")]
    public string SimNo { get; set; } = string.Empty;

    [Description("Status")]
    public string Status { get; set; } = string.Empty;


}


