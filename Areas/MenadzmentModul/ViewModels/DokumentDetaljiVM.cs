using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Areas.MenadzmentModul.ViewModels
{
    public class DokumentDetaljiVM
    {

        [HiddenInput]
        public int DokumentId { get; set; }

        [HiddenInput]
        public string Naziv { get; set; }

        [HiddenInput]
        public string TipDokumenta { get; set; }

        [HiddenInput]
        public string DatumIzdavanja { get; set; }

        [HiddenInput]
        public string SluzbeniList { get; set; }

        [HiddenInput]
        public string DokumentPath { get; set; }

        [HiddenInput]
        public string Korisnik { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Display(Name = "status dokumenta")]
        public bool DokumentStatus { get; set; }
    }
}
