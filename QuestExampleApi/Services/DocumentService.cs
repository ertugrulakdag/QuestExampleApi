using QuestPDF.Helpers;
using QuestPDF.Fluent;
using QuestExampleApi.Contracts;
using QuestExampleApi.Helpers;
using QuestExampleApi.Model;
using QuestPDF.Infrastructure;

namespace QuestExampleApi.Services;

public class DocumentService : IDocumentService
{
    private readonly ILogger<DocumentService> _logger;
    private readonly float _fontSize = 10;

    public DocumentService(ILogger<DocumentService> logger)
    {
        _logger = logger;
    }

    public async Task<DocumentCreateResponse> DocumentCreate(DocumentCreateRequest request,
        CancellationToken cancellationToken)
    {
        var response = new DocumentCreateResponse();
        try
        {
            var (master, detail) = await Helper.FaturaVerisiOlusturAsync();

            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(10);
                    page.Header().Element(c => GeneratePdf.ComposeHeader(c, "ÜST BAŞLIK", "Alt Başlık"));
                    
                    page.Content().Column(column =>
                    {
                        column.Item().PaddingVertical(2);
                        column.Item().Text(text =>
                        {
                            text.Span("Fatura No: ").Bold().FontSize(_fontSize);
                            text.Span(master.Rows[0]["FaturaNo"].ToString()).FontSize(_fontSize);
                        });
                        column.Item().Text(text =>
                        {
                            text.Span("Fatura Tarihi: ").Bold().FontSize(_fontSize);
                            text.Span(Convert.ToDateTime(master.Rows[0]["FaturaTarihi"]).ToString("dd.MM.yyyy")).FontSize(12);
                        });
                        column.Item().Text(text =>
                        {
                            text.Span("Cari Adı: ").Bold().FontSize(_fontSize);
                            text.Span(master.Rows[0]["CariAdi"].ToString()).FontSize(_fontSize);
                        });
                        column.Item().Text(text =>
                        {
                            text.Span("Toplam Tutar: ").Bold().FontSize(_fontSize);
                            text.Span(master.Rows[0]["ToplamTutar"].ToString()).FontSize(_fontSize);
                        });
                        column.Item().PaddingVertical(5);  
                        column.Item().Element(c => GeneratePdf.ComposeDataTable(c, detail));
                    });
                    
                    page.Footer().Element(GeneratePdf.ComposeFooter);
                });
            });
            var pdfBytes = document.GeneratePdf();
            response.Cevap = Convert.ToBase64String(pdfBytes);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                $"{ex.Message} {(ex.InnerException != null ? " Inner Exception: " + ex.InnerException.Message : string.Empty)}");
            response.Cevap =
                $"{ex.Message} {(ex.InnerException != null ? " Inner Exception: " + ex.InnerException.Message : string.Empty)}";
        }

        return response;
    }
}