using System;
using System.Collections.Generic;

namespace KompaniaPchor.ORM_Models
{
    public class Prosba
    {
        public int IdProsby { get; set; }
        public int NrKompanii { get; set; }
        public int? NrPlutonu { get; set; }
        public int? IdZatwierdzajacego { get; set; }
        public int IdZglaszajacego { get; set; }
        public bool Obsluzona { get; set; } = false;
        public TypProsby TypProsby { get; set; }

        public virtual Zolnierz _Zatwierdzajacy { get; set; }
        public virtual Zolnierz _Zglaszajacy { get; set; }
        public virtual Kompania _Kompania { get; set; }
        public virtual Pluton _Pluton { get; set; }
    }

    public enum TypProsby
    {
        /// <summary>Company Commander</summary>
        CC,
        /// <summary>Platoon Commander</summary>
        PC,
        /// <summary>Platoon Commander Assistant</summary>
        PA,
        /// <summary>Join Company Group</summary>
        JC,
        /// <summary>Join Platoon Group</summary>
        JP
    };
}
