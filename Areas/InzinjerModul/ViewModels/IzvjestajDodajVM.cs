using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class IzvjestajDodajVM
    {
        [HiddenInput]
        public int RadniNalogId { get; set; }

        [HiddenInput]
        public int IspitivanjeId { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [StringLength(50, ErrorMessageResourceName = "CustomStringLengthMaxMin", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Remote(action: nameof(IzvjestajController.ProvjeraBrojaIzvjestaja), controller: "Izvjestaj")]
        [RegularExpression("^((000[1-9]|00[1-9]\\d|0[1-9]\\d\\d|[1-9]\\d\\d\\d))(\\-)(0[1-9]|1[012])(\\-)([0-9]{2})$", ErrorMessageResourceName = "CustomREReport", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Display(Name = "broj izvještaja")]
        public string BrojIzvjestaja { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [DataType(DataType.Date)]
        [Display(Name = "datum kreiranja izvještaja")]
        public DateTime DatumKreiranja { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(CustomErrorMessages))]
        [DataType(DataType.Upload)]
        [CstPDFValidation(2)]
        [Display(Name = "izvještaj")]
        public IFormFile Izvjestaj { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(CustomErrorMessages))]
        [Display(Name = "status izvještaja")]
        public bool IzvjestajStatus { get; set; }

        //-----------------------------------------------//
        [HiddenInput]
        public string NazivIspitivanja { get; set; }

        [HiddenInput]
        public string NazivIspitivanjaOznaka { get; set; }
    }
}
