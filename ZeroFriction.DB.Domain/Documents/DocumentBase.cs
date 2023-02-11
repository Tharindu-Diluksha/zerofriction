using Newtonsoft.Json;

namespace ZeroFriction.DB.Domain.Documents
{
    public abstract class DocumentBase
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("partitionKey")]
        public virtual string PartitionKey { get; set; }

        [JsonProperty("_etag")]
        public string ETag { get; set; }

        [JsonProperty("createdOnUtc")]
        public DateTime CreatedOnUtc { get; set; }

        [JsonProperty("createdById")]
        public string CreatedById { get; set; }

        [JsonProperty("updatedOnUtc")]
        public DateTime UpdatedOnUtc { get; set; }

        [JsonProperty("updatedById")]
        public string UpdatedById { get; set; }

        [JsonProperty("isDeleted")]
        public bool IsDeleted { get; set; }

        [JsonProperty("docType")]
        public abstract string DocType { get; }
    }
}
