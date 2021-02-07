using Microsoft.AspNetCore.Mvc;
using ServisApp.Areas.AdministratorModul.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Areas.AdministratorModul.ViewModels
{
    public class KantonDodajVM
    {
        [HiddenInput]
        public int KantonId { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [StringLength(50, ErrorMessageResourceName = "CustomStringLengthMax", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Remote(action: nameof(KantonController.ProvjeraNazivaKantona), controller: "Kanton", AdditionalFields = nameof(KantonId))]
        [Display(Name = "naziv kantona")]
        public string Naziv { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [StringLength(5, ErrorMessageResourceName = "CustomStringLengthMax", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Remote(action: nameof(KantonController.ProvjeraSkracenogNazivaKantona), controller: "Kanton", AdditionalFields = nameof(KantonId))]
        [Display(Name = "skraćeni naziv kantona")]
        public string SkraceniNaziv { get; set; }
    }
}
