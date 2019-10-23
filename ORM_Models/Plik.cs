using System;
using System.Collections.Generic;

namespace KompaniaPchor.ORM_Models
{
    public partial class Plik
    {
        public Guid IdPliku { get; set; }
        public int? IdKatalogu { get; set; }
        public string Rozszerzenie { get; set; }
        public string Naglowek { get; set; }
        public string Opis { get; set; }
        public DateTime Dodano { get; set; }

        public virtual Katalog _Katalog { get; set; }
        // na potrzeby planu zajęć
        public int? NrKompanii { get; set; }
        public virtual Kompania _Kompania { get; set; }
    }
}
