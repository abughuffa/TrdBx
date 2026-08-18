
using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.ImpulseCharts.DTOs;

[Description("ImpulseChart")]
public class ImpulseChartDto
{
    [Display(Name = "Date")]
    public DateOnly Date { get; set; }

    [Display(Name = "Items")]
    public List<ItemDto>? Items { get; set; }


}

public class ItemDto
{

    [Display(Name = "Id")]
    public int Id { get; set; } = 0;

    [Display(Name = "ParentName")]
    public string ParentName { get; set; } = string.Empty;

    [Display(Name = "ChildName")]
    public string ChildName { get; set; } = string.Empty;

    [Display(Name = "SNo")]
    public string SNo { get; set; } = string.Empty;

    [Display(Name = "SimNo")]
    public string SimNo { get; set; } = string.Empty;

    [Display(Name = "Status")]
    public string Status { get; set; } = string.Empty;


}


