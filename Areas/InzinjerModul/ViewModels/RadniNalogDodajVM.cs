using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServisApp.Areas.InzinjerModul.Controllers;
using ServisApp.Util;
using ServisApp.Util.CustomValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Areas.InzinjerModul.ViewModels
{
    public class RadniNalogDodajVM
    {
        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [StringLength(50, ErrorMessageResourceName = "CustomStringLengthMaxMin", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Remote(action: nameof(RadniNalogController.ProvjeraBrojaRadnogNaloga), controller: "RadniNalog")]
        [RegularExpression("^((00[1-9]|0[1-9]\\d|[1-9]\\d\\d))(\\-)(0[1-9]|1[012])(\\/)([0-9]{2})$", ErrorMessageResourceName = "CustomREWorkOrder", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Display(Name = "broj radnog naloga")]
        public string BrojRadnogNaloga { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [DataType(DataType.DateTime)]
        [Display(Name = "datum i vrijeme početka radova")]
        public DateTime DatumPocetkaRadova { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [DataType(DataType.DateTime)]
        [DatumZavrsetkaVeceOdDatumPocetka]
        [Display(Name = "datum i vrijeme završetka radova")]
        public DateTime DatumZavrsetkaRadova { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [DataType(DataType.Upload)]
        [CstPDFValidation(2)]
        [Display(Name = "radni nalog")]
        public IFormFile RadniNalog { get; set; }

        public List<SelectListItem> Objekti { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceName = "CustomRegularExpressionSelectList", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Display(Name = "objekat")]
        public int ObjekatId { get; set; }

        //----------------------------------------------------------------------------------//
        public List<SelectListItem> ClanoviServisa { get; set; }
    }
}
