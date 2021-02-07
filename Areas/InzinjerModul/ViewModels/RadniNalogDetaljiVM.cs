using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Areas.InzinjerModul.ViewModels
{
    public class RadniNalogDetaljiVM
    {
        public int RadniNalogId { get; set; }
        public string BrojRadnogNaloga { get; set; }      
        public string DatumPocetkaRadova { get; set; }       
        public string DatumZavrsetkaRadova { get; set; }
        public string UkupnoSatiRada { get; set; }
        public string RadniNalogPath { get; set; }      
        public string NazivObjekta { get; set; }
        public string NazivKlijenta { get; set; }
        public List<string> ClanoviServisa { get; set; }

        public int ObjekatId { get; set; }
    }
}
