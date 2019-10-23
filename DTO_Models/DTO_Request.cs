using KompaniaPchor.ORM_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KompaniaPchor.DTO_Models
{
    public class DTO_Request
    {
        public int CompanyId { get; set; }
        public int? PlatoonId { get; set; }
        public TypProsby RequestType { get; set; }
    }
}
