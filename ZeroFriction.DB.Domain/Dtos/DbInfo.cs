using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroFriction.DB.Domain.Dtos
{
    public class DbInfo
    {
        public string DatabaseKey { get; set; }

        public string DatabaseName { get; set; }

        public string DatabaseURI { get; set; }

        public string CollectionId { get; set; }
    }
}
