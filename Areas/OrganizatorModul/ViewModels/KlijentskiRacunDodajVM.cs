using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServisApp.Areas.OrganizatorModul.Controllers;
using ServisApp.Util.CustomValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Areas.OrganizatorModul.ViewModels
{
    public class KlijentskiRacunDodajVM
    {
        [HiddenInput]
        public int KlijentId { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [StringLength(50, ErrorMessageResourceName = "CustomStringLengthMax", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Display(Name = "ime")]
        public string Ime { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [StringLength(50, ErrorMessageResourceName = "CustomStringLengthMax", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Display(Name = "prezime")]
        public string Prezime { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [StringLength(100, ErrorMessageResourceName = "CustomStringLengthMax", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [EmailAddress(ErrorMessageResourceName = "CustomEmail", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Remote(action: nameof(KlijentskiRacunController.ProvjeraEmailaKlijentskogRacuna), controller: "KlijentskiRacun")]
        [Display(Name = "kontakt email")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [StringLength(50, ErrorMessageResourceName = "CustomStringLengthMax", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Remote(action: nameof(KlijentskiRacunController.ProvjeraKorisnickogImenaKlijentskogRacuna), controller: "KlijentskiRacun")]
        [Display(Name = "korisničko ime")]
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

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [DataType(DataType.Upload)]
        [CstImgValidation(60, "jpg,jpeg,png")]
        [Display(Name = "sliku korisnika")]
        public IFormFile KlijentskiRacunSlika { get; set; }

        [HiddenInput]
        public string Klijent { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Display(Name = "email notifikacija")]
        public bool EmailNotifikacija { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Range(0, 31, ErrorMessageResourceName = "CustomRangeMinMax", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Display(Name = "broj dana prije isteka")]
        public int BrojDanaPrijeIsteka { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Display(Name = "status klijentskog računa")]
        public bool KlijentskiRacunStatus { get; set; }
    }
}
