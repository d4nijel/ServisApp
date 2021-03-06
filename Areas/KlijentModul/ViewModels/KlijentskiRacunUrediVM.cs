﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServisApp.Areas.KlijentModul.Controllers;
using ServisApp.Areas.OrganizatorModul.Controllers;
using ServisApp.Util.CustomValidation;
using System;
using System.ComponentModel.DataAnnotations;

namespace ServisApp.Areas.KlijentModul.ViewModels
{
    public class KlijentskiRacunUrediVM
    {
        [HiddenInput]
        public int KlijentskiRacunId { get; set; }

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
        [Remote(action: nameof(KlijentskiProfilController.ProvjeraEmailaKlijentskogRacuna), controller: "KlijentskiProfil", AdditionalFields = nameof(KlijentskiRacunId))]
        [Display(Name = "kontakt email")]
        public string Email { get; set; }

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
        public IFormFile KlijentskiRacunSlika { get; set; }

        [HiddenInput]
        public string KlijentskiRacunSlikaPath { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Display(Name = "email notifikacija")]
        public bool EmailNotifikacija { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Range(0, 31, ErrorMessageResourceName = "CustomRangeMinMax", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Display(Name = "broj dana prije isteka")]
        public int BrojDanaPrijeIsteka { get; set; }
    }
}
