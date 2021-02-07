using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.ViewModels
{
    public class KreirajIzvjestajIndexVM
    {
        public List<SelectListItem> Klijenti { get; set; }
        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Display(Name = "klijenta")]
        public int KlijentId { get; set; }

        public List<SelectListItem> NaziviIspitivanja { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Display(Name = "naziv ispitivanja")]
        public int NazivIspitivanjaId { get; set; }

        public List<SelectListItem> Opcine { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Display(Name = "općinu")]
        public int OpcinaId { get; set; }

        public List<SelectListItem> Kantoni { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Display(Name = "kanton")]
        public int KantonId { get; set; }

        public string KlijentNaziv { get; set; }
        public bool IsKlijent { get; set; }
    }
}
