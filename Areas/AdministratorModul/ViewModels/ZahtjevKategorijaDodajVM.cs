using Microsoft.AspNetCore.Mvc;
using ServisApp.Areas.AdministratorModul.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Areas.AdministratorModul.ViewModels
{
    public class ZahtjevKategorijaDodajVM
    {
        [HiddenInput]
        public int ZahtjevKategorijaId { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [StringLength(50, ErrorMessageResourceName = "CustomStringLengthMax", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Remote(action: nameof(ZahtjevKategorijaController.ProvjeraNazivaKategorijeZahtjeva), controller: "ZahtjevKategorija", AdditionalFields = nameof(ZahtjevKategorijaId))]
        [Display(Name = "naziv kategorije")]
        public string Naziv { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [StringLength(255, ErrorMessageResourceName = "CustomStringLengthMax", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Display(Name = "opis kategorije")]
        public string Opis { get; set; }
    }
}
