using Microsoft.AspNetCore.Mvc;
using ServisApp.Areas.AdministratorModul.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Areas.AdministratorModul.ViewModels
{
    public class NazivIspitivanjaDodajVM
    {
        [HiddenInput]
        public int NazivIspitivanjaId { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [StringLength(100, ErrorMessageResourceName = "CustomStringLengthMax", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Remote(action: nameof(NazivIspitivanjaController.ProvjeraNaziva), controller: "NazivIspitivanja", AdditionalFields = nameof(NazivIspitivanjaId))]
        [Display(Name = "naziv usluge")]
        public string Naziv { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [StringLength(50, ErrorMessageResourceName = "CustomStringLengthMax", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Remote(action: nameof(NazivIspitivanjaController.ProvjeraOznake), controller: "NazivIspitivanja", AdditionalFields = nameof(NazivIspitivanjaId))]
        [Display(Name = "oznaku")]
        public string Oznaka { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Range(1, 60, ErrorMessageResourceName = "CustomRangeMinMax", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Display(Name = "period važenja usluge (mjeseci)")]
        public int PeriodVazenja { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Display(Name = "status naziva usluge")]
        public bool NazivIspitivanjaStatus { get; set; }
    }
}
