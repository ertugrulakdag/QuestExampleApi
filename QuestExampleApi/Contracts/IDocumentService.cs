using QuestExampleApi.Model;

namespace QuestExampleApi.Contracts
{
    public interface IDocumentService
    {
        Task<DocumentCreateResponseModel> DocumentCreate(DocumentCreateRequestModel request);

    }
}
