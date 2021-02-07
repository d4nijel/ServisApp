using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Areas.MenadzmentModul.ViewModels
{
    public class KlijentDetaljiVM
    {
        public string Naziv { get; set; }
        public string SkraceniNaziv { get; set; }
        public string IdBroj { get; set; }
        public string Ulica { get; set; }
        public string Sjediste { get; set; }
        public string KontaktOsoba { get; set; }
        public string KontaktBrojFiksni { get; set; }
        public string KontaktBrojMobitel { get; set; }
        public string KontaktEmail { get; set; }
        public bool KlijentStatus { get; set; }
    }
}
