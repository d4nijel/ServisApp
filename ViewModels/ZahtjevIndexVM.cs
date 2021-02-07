using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.ViewModels
{
    public class ZahtjevIndexVM
    {
        public class Row
        {
            public int ZahtjevId { get; set; }
            public string Naslov { get; set; }
            public string DatumKreiranja { get; set; }
            public string KategorijaZahtjeva { get; set; }
            public int StatusZahtjevaId { get; set; }
            public string StatusZahtjeva { get; set; }
            public string KlijentskiRacun { get; set; }
            public string Korisnik { get; set; }
            public string Klijent { get; set; }
        }
        public List<Row> Rows { get; set; }
        public bool Dodijeli { get; set; }
        public bool IsKlijent { get; set; }
        public bool IsArhiva { get; set; }
    }
}
