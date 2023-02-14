using System;

namespace ZeroFriction.DB.Domain.Dtos
{
    public class DocumentUpdateResultDto
    {
        public string Id { get; set; }

        public string ETag { get; set; }

        public string UpdatedById { get; set; }

        public DateTime UpdatedOnUtc { get; set; }
    }
}
