using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Areas.OrganizatorModul.ViewModels
{
    public class ObjekatDetaljiVM
    {
        [HiddenInput]
        public int ObjekatId { get; set; }
        public string Naziv { get; set; }
        public string Ulica { get; set; }
        public string KontaktOsoba { get; set; }
        public string KontaktBrojFiksni { get; set; }
        public string KontaktBrojMobitel { get; set; }
        public string KontaktEmail { get; set; }
        public bool ObjekatStatus { get; set; }
        public string NazivKlijenta { get; set; }
        public string NazivMjesta { get; set; }
    }
}
