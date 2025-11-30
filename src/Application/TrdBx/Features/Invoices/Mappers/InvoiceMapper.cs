
using CleanArchitecture.Blazor.Application.Features.Invoices.Commands.Create;
using CleanArchitecture.Blazor.Application.Features.Invoices.Commands.Update;
using CleanArchitecture.Blazor.Application.Features.Invoices.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.Invoices.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class Mapper
{
    public static partial InvoiceDto ToDto(Invoice source);
    public static partial Invoice FromDto(InvoiceDto dto);
    //public static partial CusPrice FromEditCommand(AddEditCusPriceCommand command);
    public static partial Invoice FromCreateCommand(CreateInvoiceCommand command);
    public static partial UpdateInvoiceCommand ToUpdateCommand(InvoiceDto dto);
    //public static partial AddEditCusPriceCommand CloneFromDto(CusPriceDto dto);
    public static partial void ApplyChangesFrom(UpdateInvoiceCommand source, Invoice target);
    //public static partial void ApplyChangesFrom(AddEditCusPriceCommand source, CusPrice target);
    public static partial IQueryable<InvoiceDto> ProjectTo(this IQueryable<Invoice> q);
}

