using Microsoft.AspNetCore.Mvc;
using QuestExampleApi.Contracts;
using QuestExampleApi.Model;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestExampleApi.Services
{
    public class DocumentService : IDocumentService
    {
        public async Task<DocumentCreateResponse> DocumentCreate(DocumentCreateRequest request)
        {
            var response = new DocumentCreateResponse();
            try
            {
                Document document = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);//Sayfa Boyutu
                        page.Margin(2, Unit.Centimetre);//Sağ ve Sol Boşluk Ayarı
                        page.PageColor(Colors.White);//Sayfa Rengi
                        page.DefaultTextStyle(x => x.FontSize(10));
                        page.Header()
                            .Text("Merhaba PDF || Hello PDF ! ")
                            .SemiBold().FontSize(36)
                            .FontColor(Colors.Red.Darken1);

                        page.Content()
                                    .Column(x =>
                                    {
                                        x.Item().Text(Placeholders.Sentence());
                                        x.Spacing(10);
                                        x.Item().Text(Placeholders.Sentence()).FontSize(15);
                                        x.Spacing(10);
                                        x.Item().Text(Placeholders.LoremIpsum());
                                        x.Spacing(10);
                                        x.Item().Text(Placeholders.Paragraph()).FontFamily("Georgia");
                                        x.Spacing(10);
                                        x.Item().Text(Placeholders.Sentence()).FontColor(Colors.Blue.Darken3);
                                        x.Spacing(10);
                                        x.Item().Text(txt =>
                                        {
                                            txt.Span("C# ").Italic();
                                            txt.Span("is a modern, object-oriented, and type-safe programming language.");
                                        });

                                    });

                        page.Footer()
                            .AlignCenter()
                            .Text(x =>
                            {
                                x.Span("Sayfa ");
                                x.CurrentPageNumber();
                            });
                    });
                });
                byte[] pdfBytes = document.GeneratePdf();
                MemoryStream ms = new MemoryStream(pdfBytes);


                response.Cevap = Convert.ToBase64String(pdfBytes);

            }
            catch (Exception ex)
            {
                response.Cevap = ex.Message;
            }
            return response;
        }
    }
}
