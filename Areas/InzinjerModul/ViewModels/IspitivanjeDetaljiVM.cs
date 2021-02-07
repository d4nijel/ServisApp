using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Areas.InzinjerModul.ViewModels
{
    public class IspitivanjeDetaljiVM
    {
        public int RadniNalogId { get; set; }
        public string DatumIspitivanja { get; set; }
        public string DatumNarednogIspitivanja { get; set; }
        public string BrojDanaDoNarednogIspitivanja { get; set; }
        public string TipIspitivanja { get; set; }
        public string Napomena { get; set; }
        public string NazivIspitivanja { get; set; }
        public string NazivIspitivanjaOznaka { get; set; }
        
        public int ObjekatId { get; set; }
    }
}
