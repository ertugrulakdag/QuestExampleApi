using QuestExampleApi.Model;

namespace QuestExampleApi.Contracts
{
    public interface IDocumentService
    {
        Task<DocumentCreateResponse> DocumentCreate(DocumentCreateRequest request, CancellationToken cancellationToken);

    }
}
