using Microsoft.AspNetCore.Mvc;
using ServisApp.Areas.AdministratorModul.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Areas.AdministratorModul.ViewModels
{
    public class ZahtjevStatusDodajVM
    {
        [HiddenInput]
        public int ZahtjevStatusId { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [StringLength(50, ErrorMessageResourceName = "CustomStringLengthMax", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Remote(action: nameof(ZahtjevStatusController.ProvjeraNazivaStatusaZahtjeva), controller: "ZahtjevStatus", AdditionalFields = nameof(ZahtjevStatusId))]
        [Display(Name = "naziv statusa")]
        public string Naziv { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [StringLength(255, ErrorMessageResourceName = "CustomStringLengthMax", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Display(Name = "opis statusa")]
        public string Opis { get; set; }
    }
}
