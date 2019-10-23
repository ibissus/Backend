using KompaniaPchor.ORM_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KompaniaPchor.DTO_Models
{
    public class DTO_FolderContent
    {
        public ICollection<Katalog> Folders { get; set; }
        public ICollection<Plik> Files { get; set; }
        public DTO_FolderContent(ICollection<Plik> file, ICollection<Katalog> folder)
        {
            Files = file;
            Folders = folder;
        }
        public DTO_FolderContent()
        {

        }
    }
}
