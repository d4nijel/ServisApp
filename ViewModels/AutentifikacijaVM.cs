using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.ViewModels
{
    public class AutentifikacijaVM
    {
        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [StringLength(100, ErrorMessageResourceName = "CustomStringLengthMax", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Display(Name = "korisničko ime")]
        public string KorisnickoIme { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [StringLength(50, ErrorMessageResourceName = "CustomStringLengthMax", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [DataType(DataType.Password)]
        [Display(Name = "lozinku")]
        public string Lozinka { get; set; }

        public bool ZapamtiLozinku { get; set; }

        //osn.inf.
        public string ImePrezime { get; set; }
        public string KorisnikSlikaPath { get; set; }
        public string KlijentskiRacunSlikaPath { get; set; }
        public int KorisnikId { get; set; }
        public int KlijentskiRacunId { get; set; }

        //uloge
        public bool IsAdministrator { get; set; }
        public bool IsOrganizator { get; set; }
        public bool IsInzinjer { get; set; }
        public bool IsMenadzment { get; set; }
        public bool IsKlijent { get; set; }
    }
}
