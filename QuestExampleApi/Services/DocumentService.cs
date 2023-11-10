using Microsoft.AspNetCore.Mvc;
using QuestExampleApi.Contracts;
using QuestExampleApi.Model;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Collections.Generic;
using System.Text.Json;

namespace QuestExampleApi.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpClientFactory _clientFactory;
        public IEnumerable<ProductDTO>? _products { get; private set; }

        public DocumentService(HttpClient client, IHttpClientFactory clientFactory)
        {
            _httpClient = client;
            _clientFactory = clientFactory;
        }
        public async Task<DocumentCreateResponse> DocumentCreate(DocumentCreateRequest request, CancellationToken cancellationToken)
        {
            var response = new DocumentCreateResponse();
            try
            {
                //Örnek:1 - Example:1
                var req1 = new HttpRequestMessage(HttpMethod.Get, "https://dummyjson.com/products/1");
                req1.Headers.Add("Accept", "application/json");
                req1.Headers.Add("User-Agent", "HttpClientFactory-Sample");
                var client1 = _clientFactory.CreateClient();
                var res1 = await client1.SendAsync(req1);
                if (res1.IsSuccessStatusCode)
                {
                    using var resStream1 = await res1.Content.ReadAsStreamAsync();
                    _products = await JsonSerializer.DeserializeAsync<IEnumerable<ProductDTO>>(resStream1);
                }

                //Örnek:2 - Example:2
                var req2 = new HttpRequestMessage(HttpMethod.Get, "products/2");
                var client2 = _clientFactory.CreateClient("dummyjson");
                var res2 = await client2.SendAsync(req2,cancellationToken);
                if (res2.IsSuccessStatusCode)
                {
                    using var resStream2 = await res2.Content.ReadAsStreamAsync();
                    _products = await JsonSerializer.DeserializeAsync<IEnumerable<ProductDTO>>(resStream2);
                }

                //Örnek:3 - Example:3

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
