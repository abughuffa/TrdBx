
using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.ImpulseCharts.DTOs;

// [Description("ImpulseChart")]
// public class ImpulseChartDto
// {
//     [Display(Name = "Date")]
//     public DateOnly Date { get; set; }

//     [Display(Name = "Items")]
//     public List<ItemDto>? Items { get; set; }


// }

// public class ItemDto
// {

//     [Display(Name = "Id")]
//     public int Id { get; set; } = 0;

//     [Display(Name = "ParentName")]
//     public string ParentName { get; set; } = string.Empty;

//     [Display(Name = "ChildName")]
//     public string ChildName { get; set; } = string.Empty;

//     [Display(Name = "SNo")]
//     public string SNo { get; set; } = string.Empty;

//     [Display(Name = "SimNo")]
//     public string SimNo { get; set; } = string.Empty;

//     [Display(Name = "Status")]
//     public string Status { get; set; } = string.Empty;


// }



public class Impulse
{
    public DateOnly Date { get; set; }
    public List<ExpiryObject> ExpiryObjects { get; set; } = new();
    public int TotalCount => ExpiryObjects?.Count ?? 0;
    public string? Summary { get; set; }
}

    public class ExpiryObject
    {
        public int ObjectId { get; set; }
        public DateOnly ExDate { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string SNo { get; set; } = string.Empty;
        public string SimNo { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int? DaysRemaining { get; set; }
        public string? ObjectStatus { get; set; }  
        
    }

// public class ItemDto
// {
//     public int Id { get; set; }
//     public string ParentName { get; set; } = string.Empty;
//     public string ChildName { get; set; } = string.Empty;
//     public string SNo { get; set; } = string.Empty;
//     public string SimNo { get; set; } = string.Empty;
//     public string Status { get; set; } = string.Empty;
//     public string? CustomerName { get; set; }
//     public string? SubscriptionStatus { get; set; }
//     public int? DaysRemaining { get; set; }
// }


