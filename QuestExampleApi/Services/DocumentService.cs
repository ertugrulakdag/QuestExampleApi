﻿using Microsoft.AspNetCore.Mvc;
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
        private readonly ILogger<DocumentService> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly DummyJsonService _dummyJsonService;
        public DocumentService(ILogger<DocumentService> logger, IHttpClientFactory clientFactory, DummyJsonService dummyJsonService)
        {
            _logger = logger;
            _clientFactory = clientFactory;
            _dummyJsonService = dummyJsonService;
        }
        public async Task<DocumentCreateResponse> DocumentCreate(DocumentCreateRequest request, CancellationToken cancellationToken)
        {
            var response = new DocumentCreateResponse();
            try
            {
                List<ProductDTO> products = new List<ProductDTO>();

                //    //Örnek:1 - Example:1
                var req1 = new HttpRequestMessage(HttpMethod.Get, "https://dummyjson.com/products/1");
                req1.Headers.Add("Accept", "application/json");
                req1.Headers.Add("User-Agent", "HttpClientFactory-Sample");
                var client1 = _clientFactory.CreateClient();
                var res1 = await client1.SendAsync(req1);
                if (res1.IsSuccessStatusCode)
                {
                    using var resStream1 = await res1.Content.ReadAsStreamAsync();
                    var product1 = await JsonSerializer.DeserializeAsync<ProductDTO>(resStream1);
                    if (product1 != null)
                    {
                        products.Add(product1);
                    }
                }

                //Örnek:2 - Example:2
                var req2 = new HttpRequestMessage(HttpMethod.Get, "products/2");
                var client2 = _clientFactory.CreateClient("dummyjson");
                var res2 = await client2.SendAsync(req2, cancellationToken);
                if (res2.IsSuccessStatusCode)
                {
                    using var resStream2 = await res2.Content.ReadAsStreamAsync();
                    var product2 = await JsonSerializer.DeserializeAsync<ProductDTO>(resStream2);
                    if (product2 != null)
                    {
                        products.Add(product2);
                    }
                }

                //Örnek:3 - Example:3
                var product3 = await _dummyJsonService.GetProduct(3);
                if (product3 != null)
                {
                    products.Add(product3);
                }


                //Örnek:4 - Example:4
                var product4 = await _dummyJsonService.GetRequest<ProductDTO>("products/4");
                if (product4 != null)
                {
                    products.Add(product4);
                }

                //Örnek: 5 - Example:5(Post Örneği)
                PostExampleRequest postExampleRequest = new PostExampleRequest
                {
                    Title = $"t-i-t-l-e {DateTime.Now.ToShortTimeString()}"
                };
                var postExample = await _dummyJsonService.PostRequest<PostExampleRequest, PostExampleResponse>("products/add", postExampleRequest);
                _logger.LogInformation($"Id:{postExample?.Id}");
                float borderLine = (float)0.10;
                Document document = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Margin(10);
                        page.Size(PageSizes.A4);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontSize(12));

                        page.Header()
                            .AlignCenter()
                            .Text("Invoice #: 2023-77")
                            .SemiBold().FontSize(24).FontColor(Colors.Grey.Darken4);

                        page.Content()

                            .Padding(5)
                            .Border(borderLine, Unit.Point)
                             .Padding(5)
                            .Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(100);
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Border(1, Unit.Point).PaddingLeft(3).AlignLeft().Text("Sıra");
                                    header.Cell().Border(1, Unit.Point).PaddingLeft(3).Text("Title");
                                    header.Cell().Border(1, Unit.Point).PaddingLeft(3).Text("Category");
                                });

                                foreach (var item in products)
                                { 
                                    table.Cell().BorderBottom(borderLine, Unit.Point).BorderVertical(1, Unit.Point).PaddingLeft(3).AlignLeft().Text($"{item.Id} #");
                                    table.Cell().BorderBottom(borderLine, Unit.Point).BorderVertical(1, Unit.Point).PaddingLeft(3).Text(item.Title);
                                    table.Cell().BorderBottom(borderLine, Unit.Point).BorderVertical(1, Unit.Point).PaddingLeft(3).Text(item.Category);
                                }
                                table.Cell().BorderBottom(1, Unit.Point);
                                table.Cell().BorderBottom(1, Unit.Point);
                                table.Cell().BorderBottom(1, Unit.Point);
                            }) ;
                        page.Footer()
                           .AlignCenter()
                           .Text(x =>
                           {
                               x.Span("Sayfa ");
                               x.TotalPages();
                               x.Span(" / ");
                               x.CurrentPageNumber();
                           });

                    });
                    
                });

                byte[] pdfBytes = document.GeneratePdf();
                MemoryStream ms = new MemoryStream(pdfBytes);

                response.Products = products;
                response.Cevap = Convert.ToBase64String(pdfBytes);

            }
            catch (Exception ex)
            {
                response.Cevap = $"{ex.Message} {(ex.InnerException != null ? " Inner Exception: " + ex.InnerException.Message : string.Empty)}";
            }
            return response;
        }
    }
}
