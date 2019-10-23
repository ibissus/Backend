using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KompaniaPchor.DTO_Models
{
    public class DTO_FileUpload
    {
        public int ContentFolder { get; set; }
        public string Description { get; set; }
        public IFormFile File { get; set; }
    }
}
