using CleanArchitecture.Blazor.Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Document = QuestPDF.Fluent.Document;

namespace CleanArchitecture.Blazor.Infrastructure.Services;

public partial class PDFService : IPDFService
{


    public async Task<byte[]> ExportReportAsync<TData>(IEnumerable<TData> data, Dictionary<string, string> param, Dictionary<string, Func<TData, object?>> mappers)
    {
        var stream = new MemoryStream();

        var dataList = data.ToList() as List<InvItem>;

        var groupedData = dataList.GroupBy(item => item.Serial).ToDictionary(g => g.Key, g => g.ToList());

        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(10, Unit.Millimetre);
                page.Size(PageSizes.A4);
                page.PageColor(QuestPDF.Helpers.Colors.White);
                page.DefaultTextStyle(x => x.FontSize(12).DirectionFromRightToLeft());

                // Page Header
                page.Header().Table(table =>

                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(70);
                        columns.ConstantColumn(70);
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    });

                    table.Cell().Row(1).Column(1).Text(text =>
                    {
                        text.AlignLeft();
                        text.Span($"{param["InvNo"]:d}");
                    });

                    table.Cell().Row(1).Column(2).Text(text =>
                    {
                        text.AlignRight();
                        text.Span("ر. الفاتورة: ");
                    });

                    table.Cell().Row(2).Column(1).Text(text =>
                    {
                        text.AlignLeft();
                        text.Span($"{param["InvDate"]:d}");
                    });

                    table.Cell().Row(2).Column(2).Text(text =>
                    {
                        text.AlignRight();
                        text.Span("ت. الفاتورة: ");
                    });

                    table.Cell().Row(1).RowSpan(2).Column(3).ColumnSpan(2).Text(text =>
                    {
                        text.AlignRight();
                        text.Span(param["InvDesc"]).SemiBold().FontSize(24).FontColor(QuestPDF.Helpers.Colors.Blue.Medium);
                    });

                    table.Cell().ColumnSpan(4).Text(string.Format("إلى: {0}.", param["To"])).AlignRight();
                    table.Cell().ColumnSpan(4).LineHorizontal(1, Unit.Point);
                });



                page.Content().Table(table =>
                {

                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(25, Unit.Millimetre);
                        columns.ConstantColumn(25, Unit.Millimetre);
                        columns.RelativeColumn();
                        columns.ConstantColumn(15, Unit.Millimetre);
                        columns.ConstantColumn(15, Unit.Millimetre);
                    });

                    table.Header(header =>
                    {
                        header.Cell().Element(HeaderCellStyle).Text("المجموع").FontSize(10).Bold();
                        header.Cell().Element(HeaderCellStyle).Text("القيمة").FontSize(10).Bold();
                        header.Cell().Element(HeaderCellStyle).Text("البيــــــــــــــــان").FontSize(10).Bold();
                        header.Cell().Element(HeaderCellStyle).Text("ت.ف").FontSize(10).Bold();
                        header.Cell().Element(HeaderCellStyle).Text("ت").FontSize(10).Bold();
                    });

                    foreach (var group in groupedData)
                    {
                        bool isFirstRow1 = true;
                        bool isFirstRow2 = true;

                        foreach (var item in group.Value)
                        {
                            if (isFirstRow2)
                            {
                                table.Cell().RowSpan((uint)group.Value.Count).Element(DataCellStyle).Text(item.ItemTotal.ToString()).FontSize(10).Bold().AlignCenter();
                                isFirstRow2 = false;
                            }
                            table.Cell().Element(DataCellStyle).Text(item.SubTotal.ToString()).FontSize(8).AlignCenter();
                            table.Cell().Element(DataCellStyle).Text(item.Desc).FontSize(8).AlignRight();
                            table.Cell().Element(DataCellStyle).Text(item.SubSerial.ToString()).FontSize(8).AlignCenter();

                            if (isFirstRow1)
                            {
                                table.Cell().RowSpan((uint)group.Value.Count).Element(DataCellStyle).Text(group.Key.ToString()).FontSize(10).Bold().AlignCenter();
                                isFirstRow1 = false;
                            }
                        }
                    }

                    table.Cell().Element(DataCellStyle).Text(param["Total"].ToString()).AlignCenter().Bold();
                    table.Cell().ColumnSpan(3).Element(DataCellStyle).Text("المجموع: ").AlignLeft().Bold();
                    table.Cell().Element(DataCellStyle).Text("--").AlignCenter().Bold();

                    table.Cell().Element(DataCellStyle).Text(param["Taxes"].ToString()).AlignCenter().Bold();
                    table.Cell().ColumnSpan(3).Element(DataCellStyle).Text("الضريبة: ").AlignLeft().Bold();
                    table.Cell().Element(DataCellStyle).Text("--").AlignCenter().Bold();

                    table.Cell().Element(DataCellStyle).Text(param["GrangTotal"].ToString()).AlignCenter().Bold();
                    table.Cell().ColumnSpan(3).Element(DataCellStyle).Text("المجموع الكلي: ").AlignLeft().Bold();
                    table.Cell().Element(DataCellStyle).Text("--").AlignCenter().Bold();

                    table.Cell().ColumnSpan(5);
                    table.Cell().ColumnSpan(5).Element(DataCellStyle).Text(param["GrangTotalInText"]).AlignCenter().Bold();
                });


                // Page Footer
                page.Footer().Row(row =>
                {
                    row.RelativeItem()
                        .AlignCenter()
                        .Text(text =>
                        {
                            text.TotalPages();
                            text.Span("الصفحة ");
                            text.CurrentPageNumber();
                            text.Span(" من ");
                        });
                });
            });
        }).GeneratePdf(stream);

        return await Task.FromResult(stream.ToArray());
    }

    static IContainer HeaderCellStyle(IContainer container)
    {
        return container.Border(1).Background(QuestPDF.Helpers.Colors.Grey.Lighten3).Padding(5).AlignCenter();
    }

    static IContainer DataCellStyle(IContainer container)
    {
        return container.Border(1).Padding(5).AlignMiddle();
    }

}