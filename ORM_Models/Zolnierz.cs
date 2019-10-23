using KompaniaPchor.DTO_Models;
using System;
using System.Collections.Generic;

namespace KompaniaPchor.ORM_Models
{
    public partial class Zolnierz
    {
        public Zolnierz()
        {
            _Kompanie = new HashSet<Kompania>();
            _ZatwierdzeniaProsb = new HashSet<Prosba>();
            _ZgloszoneProsby = new HashSet<Prosba>();
            _Plutony = new HashSet<Pluton>();
        }

        public Zolnierz(DTO_RegisterForm f) : this()
        {
            Imie = f.FirstName;
            Nazwisko = f.FamilyName;
        }

        public int IdOsoby { get; set; }
        public int? NrKompanii { get; set; }
        public int? NrPlutonu { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public bool Funkcyjny { get; set; } = false;

        public virtual Kompania _Kompania { get; set; }
        public virtual Pluton _Pluton { get; set; }
        public virtual ICollection<Kompania> _Kompanie { get; set; }
        public virtual ICollection<Pluton> _Plutony { get; set; }
        public virtual ICollection<Prosba> _ZatwierdzeniaProsb { get; set; }
        public virtual ICollection<Prosba> _ZgloszoneProsby { get; set; }
    }
}
