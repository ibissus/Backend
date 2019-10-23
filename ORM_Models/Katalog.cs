using System;
using System.Collections.Generic;

namespace KompaniaPchor.ORM_Models
{
    public partial class Katalog
    {
        public Katalog()
        {
            _Pliki = new HashSet<Plik>();
        }

        public int IdKatalogu { get; set; }
        public int? IdKataloguNadrzednego { get; set; }
        public int NrKompanii { get; set; }
        public int? NrPlutonu { get; set; }
        public string Nazwa { get; set; }

        public virtual Kompania _Kompania { get; set; }
        public virtual Pluton _Pluton { get; set; }
        public virtual Katalog _KatalogNadrzedny { get; set; }
        public virtual ICollection<Katalog> _KatalogiPodrzedne { get; set; }
        public virtual ICollection<Plik> _Pliki { get; set; }
    }
}
