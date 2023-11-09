using Microsoft.AspNetCore.Mvc;
using QuestExampleApi.Contracts;
using QuestExampleApi.Model;
using System.Text.Json;

namespace QuestExampleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestDocumentController : ControllerBase
    {
        private readonly ILogger<QuestDocumentController> _logger;
        private readonly IDocumentService _documentService;

        public QuestDocumentController(ILogger<QuestDocumentController> logger, IDocumentService documentService)
        {
            _logger = logger;
            _documentService = documentService;

        }
        [HttpPost]
        [Route("DocumentCreate")]
        public async Task<DocumentCreateResponse> DocumentCreate(DocumentCreateRequest request)
        {
            _logger.LogInformation(JsonSerializer.Serialize(request));

            return await this._documentService.DocumentCreate(request);
        }
    }
}
