using System;
using System.Collections.Generic;

namespace KompaniaPchor.ORM_Models
{
    public partial class Kompania
    {
        public Kompania()
        {
            _Katalogi = new HashSet<Katalog>();
            _Prosby = new HashSet<Prosba>();
            _Zolnierze = new HashSet<Zolnierz>();
            _Plutony = new HashSet<Pluton>();
        }

        public int NrKompanii { get; set; }
        public int IdDowodcy { get; set; }
        public Guid? IdPlanuZajec { get; set; }

        public virtual Zolnierz _Dowodca { get; set; }
        public virtual ICollection<Pluton> _Plutony { get; set; }
        public virtual ICollection<Katalog> _Katalogi { get; set; }
        public virtual ICollection<Prosba> _Prosby { get; set; }
        public virtual ICollection<Zolnierz> _Zolnierze { get; set; }
        public virtual Plik _PlanZajec { get; set; }
    }
}
