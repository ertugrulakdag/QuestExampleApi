using System.Data;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestExampleApi.Helpers;

public abstract class GeneratePdf
{
    public static void ComposeHeader(IContainer container, string? upHeader, string? subHeader)
    {
        container.Row(row =>
        {
            row.ConstantItem(66).Image("wwwroot/images/organizasyonara-logo-300-300.png");
            row.RelativeItem().Column(column =>
            {
                column.Item().AlignCenter().Text(upHeader)
                    .Style(TextStyle.Default.FontSize(20).SemiBold().FontColor(Colors.Blue.Medium));
                column.Item().AlignCenter().Text(DateTime.Now.Year.ToString()).FontSize(12).Bold();
                column.Item().AlignCenter().Text(subHeader).FontSize(12);
            });
            row.ConstantItem(66).Image("wwwroot/images/organizasyonara-logo-300-300.jpg");
        });
    }
    public static void ComposeFooter(IContainer container)
    {
        container.Container()
            .Background(Colors.Grey.Lighten3)
            .Padding(3)
            .AlignCenter()
            .Text(x =>
            {
                x.Span("Sayfa ").FontSize(10);
                x.CurrentPageNumber().FontSize(10);
                x.Span(" / ").FontSize(10);
                x.TotalPages().FontSize(10);
            });
    }
    
    public static void ComposeDataTable(IContainer container, DataTable dataTable)
    {
        const float borderLine = (float)0.10;
        const float fontSize = 12;

        container
            .PaddingLeft(0)
            .PaddingRight(0)
            .Container()
            .Background(Colors.Grey.Lighten3)
            .Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    for (var i = 0; i < dataTable.Columns.Count; i++)
                    {
                        columns.RelativeColumn();
                    }
                });

                table.Header(header =>
                {
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        header.Cell()
                            .Border(1)
                            .PaddingLeft(2)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text(column.ColumnName)
                            .Bold()
                            .FontSize(fontSize);
                    }
                });


                foreach (DataRow row in dataTable.Rows)
                {
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        var cellValue = row[column];
                        var cellText = cellValue switch
                        {
                            DateTime dateTimeValue => dateTimeValue.ToString("dd.MM.yyyy HH:mm"),
                            int or long or decimal => cellValue.ToString(),
                            _ => cellValue.ToString()
                        };

                        table.Cell()
                            .BorderBottom(borderLine)
                            .BorderVertical(1)
                            .PaddingLeft(2)
                            .AlignCenter()
                            .Text(cellText)
                            .FontSize(fontSize);
                    }
                }

                for (var i = 0; i < dataTable.Columns.Count; i++)
                {
                    table.Cell().BorderBottom(1);
                }
            });
    }
}