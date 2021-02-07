using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Areas.InzinjerModul.ViewModels
{
    public class IzvjestajDetaljiVM
    {
        [HiddenInput]
        public int RadniNalogId { get; set; }
        public string BrojIzvjestaja { get; set; }
        public string DatumKreiranja { get; set; }
        public string IzvjestajPath { get; set; }
        public string IzvjestajStatus { get; set; }
        public string Korisnik { get; set; }
        public string NazivIspitivanja { get; set; }
        public string NazivIspitivanjaOznaka { get; set; }

        public int ObjekatId { get; set; }
    }
}
