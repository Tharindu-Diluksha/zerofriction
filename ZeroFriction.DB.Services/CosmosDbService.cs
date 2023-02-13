namespace ZeroFriction.DB.Services
{
    using Microsoft.Azure.Cosmos;
    using System;
    using System.Threading.Tasks;
    using ZeroFriction.DB.Domain.Contracts;
    using ZeroFriction.DB.Domain.Documents;
    using ZeroFriction.DB.Domain.Dtos;
    using ZeroFriction.DB.Domain.Exceptions;

    /// <summary>
    /// A wrapper class for Microsoft CosmosClient.
    public class CosmosDbService : IDocumentDbService
    {
        private readonly string _databaseId;
        private CosmosClient _client;
        private Database _database;
        private Container _container;
        public CosmosDbService(DbInfo dbInfo)
        {
            _databaseId = dbInfo.DatabaseName;
            _client = new CosmosClient(accountEndpoint: dbInfo.DatabaseURI, authKeyOrResourceToken: dbInfo.DatabaseKey);
            _database = _client.GetDatabase(id: _databaseId);
            _container = _database.GetContainer(id: dbInfo.CollectionId);

        }

        /// <summary>
        /// Create document 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="partitionKey"></param>
        /// <param name="document"></param>
        /// <returns></returns>
        public virtual async Task<DocumentUpdateResultDto> CreateDocumentAsync<T>(string partitionKey, T document)
            where T : DocumentBase, new()
        {
            var result = await _container.CreateItemAsync<T>(item: document, partitionKey: new PartitionKey(partitionKey));
            return new DocumentUpdateResultDto
            {
                Id = result.Resource.Id,
                ETag = result.ETag
            };
        }


        /// <summary>
        /// Replace document
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="partitionKey"></param>
        /// <param name="documentId"></param>
        /// <param name="document"></param>
        /// <param name="eTag"></param>
        /// <returns></returns>
        /// <exception cref="ConcurrencyException"></exception>
        public virtual async Task<DocumentUpdateResultDto> ReplaceDocumentAsync<T>(string partitionKey, string documentId, T document, string eTag)
            where T : DocumentBase, new()
        {
            try
            {
                var result = await _container.ReplaceItemAsync<T>(item: document, id: documentId, partitionKey: new PartitionKey(partitionKey),
                    requestOptions: new ItemRequestOptions { IfMatchEtag = eTag});
                return new DocumentUpdateResultDto
                {
                    Id = result.Resource.Id,
                    ETag = result.ETag
                };
            }
            catch (CosmosException ex)
            {
                // If ETag mismatch throws concurrency exception
                if (ex.StatusCode == System.Net.HttpStatusCode.PreconditionFailed)
                {
                    throw new ConcurrencyException();
                }

                throw;
            }
        }

        /// <summary>
        /// Delete Document
        /// </summary>
        /// <param name="partitionKey"></param>
        /// <param name="documentId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual async Task DeleteDocumentAsync<T>(string partitionKey, string documentId)
            where T : DocumentBase, new()
        {
            try
            {
                await _container.DeleteItemAsync<T>(id: documentId, partitionKey: new PartitionKey(partitionKey));
            }
            catch (CosmosException ex)
            {
                // if the document not found throws document not found execption
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new DocumentNotFoundException();
                }

                throw;
            }
        }

        /// <summary>
        /// Get document by Id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="partitionKey"></param>
        /// <param name="documentId"></param>
        /// <returns></returns>
        /// <exception cref="DocumentNotFoundException"></exception>
        public virtual async Task<T> GetDocumentAsync<T>(string partitionKey, string documentId)
        {
            try
            {
                var result = await _container.ReadItemAsync<T>(id: documentId, partitionKey: new PartitionKey(partitionKey));
                return result.Resource;
            }
            catch (CosmosException ex)
            {
                // if the document not found throws document not found execption
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new DocumentNotFoundException();
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
