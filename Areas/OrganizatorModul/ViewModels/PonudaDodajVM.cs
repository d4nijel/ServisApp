using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServisApp.Areas.OrganizatorModul.Controllers;
using ServisApp.Util;
using ServisApp.Util.CustomValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Areas.OrganizatorModul.ViewModels
{
    public class PonudaDodajVM
    {
        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [StringLength(50, ErrorMessageResourceName = "CustomStringLengthMax", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Remote(action: nameof(PonudaController.ProvjeraBrojaPonude), controller: "Ponuda")]
        [RegularExpression("^((000[1-9]|00[1-9]\\d|0[1-9]\\d\\d|[1-9]\\d\\d\\d))(\\-)(0[1-9]|1[012])(\\-)([0-9]{2})$", ErrorMessageResourceName = "CustomREPonuda", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Display(Name = "broj ponude")]
        public string BrojPonude { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [DataType(DataType.Date)]
        [Display(Name = "datum izdavanja ponude")]
        public DateTime DatumIzdavanja { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Range(1, 10000, ErrorMessageResourceName = "CustomRangeMinMax", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "ukupan iznos bez PDV")]
        public decimal UkupanIznosBezPdv { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Display(Name = "PDV")]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(1, 100, ErrorMessageResourceName = "CustomRangeMinMax", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        public decimal PDV { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [DataType(DataType.Upload)]
        [CstPDFValidation(2)]
        [Display(Name = "ponudu")]
        public IFormFile Ponuda { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Display(Name = "status ponude")]
        public bool PonudaStatus { get; set; }

        public List<SelectListItem> Klijenti { get; set; }
        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceName = "CustomRegularExpressionSelectList", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Display(Name = "klijenta")]
        public int KlijentId { get; set; }
    }
}
