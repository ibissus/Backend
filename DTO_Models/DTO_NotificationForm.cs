using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KompaniaPchor.DTO_Models
{
    public class DTO_NotificationForm
    {
        public int CompanyId { get; set; }
        public int? PlatoonId { get; set; } = null;
        public bool OnlyAssistants { get; set; } = false;
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
