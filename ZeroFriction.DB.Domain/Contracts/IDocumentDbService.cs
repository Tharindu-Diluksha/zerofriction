using ZeroFriction.DB.Domain.Dtos;

namespace ZeroFriction.DB.Domain.Contracts
{
    public interface IDocumentDbService
    {
        Task<DocumentUpdateResultDto> CreateDocumentAsync(string collectionId, string partitionKey, object document);

        Task<DocumentUpdateResultDto> ReplaceDocumentAsync(string collectionId, string partitionKey, string documentId, object document, string eTag);

        Task DeleteDocumentAsync(string collectionId, string partitionKey, string documentId);

        Task<T> GetDocumentAsync<T>(string collectionId, string partitionKey, string documentId);
    }
}
