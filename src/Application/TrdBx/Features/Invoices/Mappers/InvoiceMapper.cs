using CleanArchitecture.Blazor.Application.Features.Invoices.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.Invoices.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
//[UseStaticMapper(typeof(InvoiceItemGroupMapper))]
public static partial class Mapper
{
    [MapProperty(nameof(Invoice.Customer.Name), nameof(InvoiceDto.Customer))]
    public static partial InvoiceDto ToDto(Invoice source);
    public static partial Invoice FromDto(InvoiceDto dto);
    public static partial IQueryable<InvoiceDto> ProjectTo(this IQueryable<Invoice> q);
}

