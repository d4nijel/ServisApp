using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServisApp.Areas.MenadzmentModul.Controllers;
using ServisApp.Util;
using ServisApp.Util.CustomValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Areas.MenadzmentModul.ViewModels
{
    public class UgovorDodajVM
    {
        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [StringLength(50, ErrorMessageResourceName = "CustomStringLengthMax", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Remote(action: nameof(UgovorController.ProvjeraBrojaUgovora), controller: "Ugovor")]
        [RegularExpression("^((000[1-9]|00[1-9]\\d|0[1-9]\\d\\d|[1-9]\\d\\d\\d))(\\-)(0[1-9]|1[012])(\\-)([0-9]{2})$", ErrorMessageResourceName = "CustomREUgovor", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Display(Name = "broj ugovora")]
        public string BrojUgovora { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [StringLength(200, ErrorMessageResourceName = "CustomStringLengthMax", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Display(Name = "naziv ugovora")]
        public string Naziv { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [DataType(DataType.Date)]
        [Display(Name = "datum potpisivanja ugovora")]
        public DateTime DatumPotpisivanja { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [DataType(DataType.Date)]
        [CstDatumVrijemeUgovor]
        [Display(Name = "datum isteka ugovora")]
        public DateTime DatumIsteka { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [DataType(DataType.Upload)]
        [CstPDFValidation(2)]
        [Display(Name = "ugovor")]
        public IFormFile Ugovor { get; set; }

        public List<SelectListItem> Klijenti { get; set; }
        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceName = "CustomRegularExpressionSelectList", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Display(Name = "klijenta")]
        public int KlijentId { get; set; }
    }
}
