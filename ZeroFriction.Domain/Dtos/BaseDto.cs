using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroFriction.Domain.Dtos
{
    public class BaseDto
    {
        public string Id { get; set; }

        public bool IsDeleted { get; set; }

        public string UpdatedById { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public DateTime UpdatedOnUtc { get; set; }

        public string ETag { get; set; }
    }
}
