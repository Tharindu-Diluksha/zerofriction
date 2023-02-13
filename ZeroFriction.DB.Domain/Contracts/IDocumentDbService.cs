using ZeroFriction.DB.Domain.Documents;
using ZeroFriction.DB.Domain.Dtos;

namespace ZeroFriction.DB.Domain.Contracts
{
    public interface IDocumentDbService
    {
        Task<DocumentUpdateResultDto> CreateDocumentAsync<T>(string partitionKey, T document)
            where T : DocumentBase, new();

        Task<DocumentUpdateResultDto> ReplaceDocumentAsync<T>(string partitionKey, string documentId, T document, string eTag)
            where T : DocumentBase, new();

        Task DeleteDocumentAsync<T>(string partitionKey, string documentId)
            where T : DocumentBase, new();

        Task<T> GetDocumentAsync<T>(string partitionKey, string documentId);
    }
}
