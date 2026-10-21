using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using CleanArchitecture.Blazor.Domain.Common.Entities;

namespace CleanArchitecture.Blazor.Application.Features.Invoices.DTOs;

public class InvoiceItemGroupDto
{

   [Display(Name = "Id")]
    public int Id { get; set; }
    public int SerialIndex { get; set; }
    public int InvoiceId { get; set; }
    public int ServiceLogId { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; } = 0.0m;
    public decimal SubTotal { get; set; } = 0.0m;
    public List<InvoiceItemDto>? InvoiceItems { get; set; } = null;
}