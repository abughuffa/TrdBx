using System;
using System.Collections.Generic;
using System.Text;
using CleanArchitecture.Blazor.Domain.Common.Entities;

namespace CleanArchitecture.Blazor.Application.TrdBx.Features.Invoices.DTOs;

public class InvoiceItemDto
{
    [Description("Id")]
    public int Id { get; set; }
    public int SubSerialIndex { get; set; }
    public int InvoiceItemGroupId { get; set; }
    public int SubscriptionId { get; set; }
    public string? Description { get; set; }
    //public DateOnly StartDate { get; set; }
    //public DateOnly EndDate { get; set; }
    public decimal Amount { get; set; } = 0.0m;
}
