using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServisApp.Areas.InzinjerModul.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Areas.InzinjerModul.ViewModels
{
    public class IspitivanjeDodajVM
    {
        [HiddenInput]
        public int RadniNalogId { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [DataType(DataType.Date)]
        [Display(Name = "datum ispitivanja")]
        public DateTime DatumIspitivanja { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [StringLength(50, ErrorMessageResourceName = "CustomStringLengthMaxMin", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Display(Name = "tip ispitivanja")]
        public string TipIspitivanja { get; set; }

        [StringLength(255, ErrorMessageResourceName = "CustomStringLengthMaxMin", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Display(Name = "napomenu")]
        public string Napomena { get; set; }

        public List<SelectListItem> NaziviIspitivanja { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceName = "CustomRegularExpressionSelectList", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Display(Name = "vrstu ispitivanja")]
        public int NazivIspitivanjaId { get; set; }
    }
}
