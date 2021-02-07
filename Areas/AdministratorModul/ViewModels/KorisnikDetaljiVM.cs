using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServisApp.Util.CustomValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Areas.AdministratorModul.ViewModels
{
    public class KorisnikDetaljiVM
    {
        [HiddenInput]
        public int KorisnikId { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [StringLength(50, ErrorMessageResourceName = "CustomStringLengthMax", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Display(Name = "ime korisnika")]
        public string Ime { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [StringLength(50, ErrorMessageResourceName = "CustomStringLengthMax", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Display(Name = "prezime korisnika")]
        public string Prezime { get; set; }

        [HiddenInput]
        public string KorisnickoIme { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [StringLength(50, ErrorMessageResourceName = "CustomStringLengthMaxMin", ErrorMessageResourceType = typeof(Util.CustomErrorMessages), MinimumLength = 8)]
        [DataType(DataType.Password)]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$",
            ErrorMessageResourceName = "CustomREPassword", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Display(Name = "lozinku")]
        public string Lozinka { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [StringLength(50, ErrorMessageResourceName = "CustomStringLengthMaxMin", ErrorMessageResourceType = typeof(Util.CustomErrorMessages), MinimumLength = 8)]
        [DataType(DataType.Password)]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$",
            ErrorMessageResourceName = "CustomREPassword", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Compare(nameof(Lozinka), ErrorMessage = "Lozinke se ne podudaraju")]
        [Display(Name = "potvrdu lozinke")]
        public string LozinkaPotvrda { get; set; }

        [DataType(DataType.Upload)]
        [CstImgValidation(60, "jpg,jpeg,png")]
        [Display(Name = "sliku korisnika")]
        public IFormFile KorisnikSlika { get; set; }

        [Display(Name = "status korisnika")]
        public bool KorisnikStatus { get; set; }

        //----------------------------------------------------------------------------------//
        public List<SelectListItem> Uloge { get; set; }

        [HiddenInput]
        public string KorisnikSlikaPath { get; set; }
    }
}
