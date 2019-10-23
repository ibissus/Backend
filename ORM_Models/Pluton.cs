using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace KompaniaPchor.ORM_Models
{
    public class Pluton
    {
        public int NrPlutonu { get; set; }
        public int NrKompanii { get; set; }
        public int? IdDowodcy { get; set; }
        public virtual Zolnierz _Dowodca { get; set; }
        public virtual Kompania _Kompania { get; set; }
        public virtual ICollection<Prosba> _Prosby { get; set; }
        public virtual ICollection<Katalog> _Katalogi { get; set; }
        public virtual ICollection<Zolnierz> _Zolnierze { get; set; }
        [NotMapped]
        public virtual ICollection<Zolnierz> _Funkcyjni { get {
                return _Zolnierze == null ?  null : _Zolnierze.Where(s => s.Funkcyjny == true).ToList();
            } }
    }
}
