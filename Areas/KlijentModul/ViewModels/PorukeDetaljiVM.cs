using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Areas.KlijentModul.ViewModels
{
    public class PorukeDetaljiVM
    {
        public string Sadrzaj { get; set; }
        public string DatumKreiranja { get; set; }
        public bool KlijentskiRacun { get; set; }
        public string KlijentskiRacunNaziv { get; set; }
        public string KlijentskiRacunSlikaPath { get; set; }
        public string KorisnikNaziv { get; set; }
        public string KorisnikSlikaPath { get; set; }
    }
}
