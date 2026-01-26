using CleanArchitecture.Blazor.Application.Features.Invoices.DTOs;
using CleanArchitecture.Blazor.Application.Features.Invoices.Helper;
using CleanArchitecture.Blazor.Application.Features.XInvoices.Helper;
using CleanArchitecture.Blazor.Application.TrdBx.Features.Invoices.DTOs;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Document = QuestPDF.Fluent.Document;

namespace CleanArchitecture.Blazor.Infrastructure.Services;

public partial class PDFService : IPDFService
{
    public async Task<byte[]> ExportInvoiceAsync(InvoiceDto xinvoice)
    {
        using var stream = new MemoryStream();

        await Task.Run(() =>
        {
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(10, Unit.Millimetre);
                    page.Size(PageSizes.A4);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x
                        .FontFamily("Calibri")
                        .FontSize(12)
                        .DirectionFromRightToLeft());

                    // Main Header with Table Header
                    page.Header().Column(column =>
                    {
                        // Invoice Information
                        column.Item().Table(invoiceHeaderTable =>
                        {
                            invoiceHeaderTable.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(70);
                                columns.ConstantColumn(70);
                                columns.RelativeColumn();
                                columns.ConstantColumn(85);
                            });

                            invoiceHeaderTable.Cell().Row(1).Column(1).Text(text =>
                            {
                                text.AlignLeft();
                                text.Span($"{xinvoice.InvoiceNo:d}");
                            });
                            invoiceHeaderTable.Cell().Row(1).Column(2).Text(text =>
                            {
                                text.AlignRight();
                                text.Span("ر. الفاتورة: ");
                            });
                            invoiceHeaderTable.Cell().Row(2).Column(1).Text(text =>
                            {
                                text.AlignLeft();
                                text.Span($"{xinvoice.InvoiceDate:d}");
                            });
                            invoiceHeaderTable.Cell().Row(2).Column(2).Text(text =>
                            {
                                text.AlignRight();
                                text.Span("ت. الفاتورة: ");
                            });
                            invoiceHeaderTable.Cell().Row(1).RowSpan(2).Column(3).ColumnSpan(2).Text(text =>
                            {
                                text.AlignRight();
                                text.Span(xinvoice.Description)
                                    .SemiBold()
                                    .FontSize(24)
                                    .FontColor(Colors.DeepOrange.Darken1);
                            });
                            invoiceHeaderTable.Cell().Row(3).Column(1).ColumnSpan(4).PaddingTop(5); /// empty line
                            invoiceHeaderTable.Cell().Row(4).Column(4).Text(": السيد(ة) / السادة").Bold().AlignRight();
                            invoiceHeaderTable.Cell().Row(4).Column(1).ColumnSpan(3).Text(xinvoice.DisplayCusName).AlignRight();
                            invoiceHeaderTable.Cell().Row(5).Column(1).ColumnSpan(4).PaddingTop(5).LineHorizontal(1, Unit.Point);
                            invoiceHeaderTable.Cell().Row(6).Column(1).ColumnSpan(4).PaddingTop(5); /// empty line
                        });

                        // Repeating Table Header - This will appear on every page
                        column.Item().PaddingTop(10).Table(tableHeader =>
                        {
                            tableHeader.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(25, Unit.Millimetre);
                                columns.ConstantColumn(25, Unit.Millimetre);
                                columns.ConstantColumn(30, Unit.Millimetre);
                                columns.RelativeColumn();
                                columns.ConstantColumn(15, Unit.Millimetre);
                            });

                            tableHeader.Cell().ColumnSpan(5).PaddingTop(10); /// empty line
                            // Table Header Row
                            tableHeader.Cell().Element(HeaderCellStyle).Text("القيمة").FontSize(10).Bold().AlignCenter();
                            tableHeader.Cell().ColumnSpan(3).Element(HeaderCellStyle).Text("البيــــــــــــــــــــــــــــــــــــــان").FontSize(10).Bold().AlignRight();
                            tableHeader.Cell().Element(HeaderCellStyle).Text("ت").FontSize(10).Bold().AlignCenter();
                        });
                    });

                    page.Content().Column(contentColumn =>
                    {
                        // Create the items table
                        contentColumn.Item().Table(itemsTable =>
                        {
                            itemsTable.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(25, Unit.Millimetre);
                                columns.ConstantColumn(25, Unit.Millimetre);
                                columns.ConstantColumn(30, Unit.Millimetre);
                                columns.RelativeColumn();
                                columns.ConstantColumn(15, Unit.Millimetre);
                            });

                            // Add all groups to the main table
                            var reorderedGroups = xinvoice.InvoiceItemGroups.OrderBy(x => x.SerialIndex).ToList();
                            foreach (var group in reorderedGroups)
                            {
                                AddGroupToTable(itemsTable, group);
                            }
                        });

                        // Totals section - will automatically flow to the last page
                        contentColumn.Item().PaddingTop(20).Table(totalsTable =>
                        {
                            totalsTable.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(25, Unit.Millimetre);
                                columns.ConstantColumn(25, Unit.Millimetre);
                                columns.ConstantColumn(30, Unit.Millimetre);
                                columns.RelativeColumn();
                                columns.ConstantColumn(15, Unit.Millimetre);
                            });
                            AddTotalRow(totalsTable, xinvoice.Total, ":المجموع");
                            AddTotalRow(totalsTable, xinvoice.DiscountAmount, ":التخفيض");
                            AddTotalRow(totalsTable, xinvoice.TaxableAmount, ":الاجمالي");
                            AddTotalRow(totalsTable, xinvoice.TaxAmount, ":الضريبة");
                            AddTotalRow(totalsTable, xinvoice.GrandTotal, ":الاجمالي الكلي");
                            totalsTable.Cell().ColumnSpan(5).PaddingTop(10).Element(CellStyle).Text(ToWord.ConvertToWord(xinvoice.GrandTotal)).AlignCenter().Bold();
                        });
                    });

                    page.Footer().Column(column =>
                    {
                        // Invoice Information
                        column.Item().Table(invoiceFooterTable =>
                        {
                            invoiceFooterTable.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                            });

                            invoiceFooterTable.Cell().PaddingTop(5).LineHorizontal(1, Unit.Point);
                            invoiceFooterTable.Cell().Text(text =>
                            {
                                text.AlignCenter();
                                text.Span("شركة عين النسر للإستشارات وتقنية الأنظمة الأمنية - الدور الرابع - مبنى القرقني الظهرة - طرابلس - ليبيا").Bold().FontSize(8).FontColor(Colors.Grey.Darken2);
                            });
                            invoiceFooterTable.Cell().Text(text =>
                            {
                                text.AlignCenter();
                                text.Span("info@eagleeye.ly").Bold().FontSize(8).FontColor(Colors.Grey.Darken2);
                            });

                            invoiceFooterTable.Cell().Text(text =>
                            {
                                text.Span("الصفحة ");
                                text.CurrentPageNumber();
                                text.Span(" من ");
                                text.TotalPages();
                            });
                        });
                    });
                });
            }).GeneratePdf(stream);
        });
        return stream.ToArray();
    }

    // Helper method to add a group to table
    private static void AddGroupToTable(TableDescriptor table, InvoiceItemGroupDto group)
    {
        // Group header
        table.Cell().Element(GroupHeaderCellStyle).Text(group.Amount.ToString("N3")).Bold().AlignCenter().FontSize(8);
        table.Cell().ColumnSpan(3).Element(GroupHeaderCellStyle).Text(group.Description).Bold().AlignRight().FontSize(8);
        table.Cell().Element(GroupHeaderCellStyle).Text(group.SerialIndex.ToString()).FontSize(10).Bold().AlignCenter();

        // Group items
        var reorderedItems = group.InvoiceItems.OrderBy(x => x.SubSerialIndex);
        foreach (var item in reorderedItems)
        {
            // Keep items together with their group header
            table.Cell().Element(CellStyle).Text(item.Amount.ToString("N3")).FontSize(8).AlignCenter();
            table.Cell().ColumnSpan(3).Element(CellStyle).Text(item.Description).FontSize(8).AlignRight();
            table.Cell().Element(CellStyle).Text($"{item.SubSerialIndex}-{group.SerialIndex}").FontSize(10).Bold().AlignCenter();
        }

        // Group subtotal
        if (group.InvoiceItems.Any())
        {
            table.Cell().Element(GroupHeaderCellStyle).Text(group.SubTotal.ToString("N3")).Bold().AlignCenter().FontSize(8);
            table.Cell().ColumnSpan(3).Element(CellStyle);
            table.Cell().Element(CellStyle).Text("--").FontSize(8).AlignCenter();
        }
    }

    // Helper method for total rows
    private static void AddTotalRow(TableDescriptor table, decimal amount, string label)
    {
        table.Cell().ColumnSpan(2).Element(TotalCellStyle).Text(amount.ToString("N3") + "د.ل").AlignCenter().Bold();
        table.Cell().Element(TotalCellStyle).Text(label).AlignRight().Bold();

        // Empty cells for the rest of the row
        table.Cell().ColumnSpan(2);
    }

    // Cell style definitions
    private static IContainer HeaderCellStyle(IContainer container)
    {
        return container.PaddingVertical(3).BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Background(Colors.Grey.Darken1);
    }

    private static IContainer CellStyle(IContainer container)
    {
        return container.PaddingVertical(3).BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2);
    }

    private static IContainer GroupHeaderCellStyle(IContainer container)
    {
        return container.PaddingVertical(3).Background(Colors.Grey.Lighten4).BorderTop(0.5f);
    }

    private static IContainer TotalCellStyle(IContainer container)
    {
        return container.PaddingVertical(5).Background(Colors.Grey.Lighten3).BorderBottom(1).BorderColor(Colors.Black);
    }
}