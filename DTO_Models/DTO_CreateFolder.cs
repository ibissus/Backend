using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KompaniaPchor.DTO_Models
{
    public class DTO_CreateFolder
    {
        public int CompanyId { get; set; }
        public int? PlatoonId { get; set; }
        public int? RootFolderId { get; set; }
        public string Name { get; set; }
    }
}
