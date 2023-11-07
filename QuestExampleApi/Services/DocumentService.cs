using Microsoft.AspNetCore.Mvc;
using QuestExampleApi.Contracts;
using QuestExampleApi.Model;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Text;

namespace QuestExampleApi.Services
{
    public class DocumentService : IDocumentService
    {
        public async Task<DocumentCreateResponseModel> DocumentCreate(DocumentCreateRequestModel request)
        {
            var response = new DocumentCreateResponseModel();
            try
            {
                //var doc = Document.Create(container => container.Page(page =>//QuestPDF türlerini içe aktarıyoruz.
                //{
                //    page.Size(PageSizes.A4);
                //    page.Margin(2, Unit.Centimetre);
                //    page.DefaultTextStyle(x => x.FontSize(12));//Bir sayfanın bazı temel özelliklerini belirleriz: sayfa boyutu, kenar boşlukları ve varsayılan yazı tipi boyutu

                //    page.Content() .Column(x => x.Item().Text(Placeholders.Paragraph()));//İçerik, İçerik ile birlikte eklenir. Bir sütun metinden oluşur. Sütuna bir metin öğesi ekliyoruz. Placeholders.Paragraph, metnin lorem impsum paragrafını oluşturur.
                //}));

                //doc.GeneratePdf("test.pdf");

                //Document.Create(container =>
                //{
                //    container.Page(page =>
                //    {
                //        page.Size(PageSizes.A4);
                //        page.Margin(2, Unit.Centimetre);
                //        page.Background(Colors.White);
                //        page.DefaultTextStyle(x => x.FontSize(20));

                //        page.Header()
                //            .Text("Hello PDF!")
                //            .SemiBold().FontSize(36).FontColor(Colors.Blue.Medium);

                //        page.Content()
                //            .PaddingVertical(1, Unit.Centimetre)
                //            .Column(x =>
                //            {
                //                x.Spacing(20);

                //                x.Item().Text(Placeholders.LoremIpsum());
                //                x.Item().Image(Placeholders.Image(200, 100));
                //            });

                //        page.Footer()
                //            .AlignCenter()
                //            .Text(x =>
                //            {
                //                x.Span("Page ");
                //                x.CurrentPageNumber();
                //            });
                //    });
                //}).GeneratePdf("hello.pdf");

                Document document = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(2, Unit.Centimetre);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontSize(20));
                        page.Header()
                            .Text("Merhaba || Hello PDF !")
                            .SemiBold().FontSize(36).FontColor(Colors.Blue.Medium);

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
