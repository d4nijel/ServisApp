using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServisApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Areas.KlijentModul.ViewModels
{
    public class ZahtjevDetaljiVM
    {
        [HiddenInput]
        public int ZahtjevId { get; set; }

        public string Naslov { get; set; }
        public string Opis { get; set; }
        public string DatumKreiranja { get; set; }
        public string KlijentskiRacun { get; set; }
        public string ZahtjevKategorija { get; set; }
        public int StatusZahtjevaId { get; set; }
        public string StatusZahtjevaNaziv { get; set; }
        public List<SelectListItem> Korisnici { get; set; }
        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceName = "CustomRegularExpressionSelectList", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Display(Name = "korisnika")]
        public int KorisnikId { get; set; }
        public string Korisnik { get; set; }
        public List<PorukeDetaljiVM> ListaPoruka { get; set; }
        public bool IsKorisnik { get; set; }
        public bool Zakljucaj { get; set; }
        public bool IsArhiva { get; set; }
        public string Klijent { get; set; }
    }
}
