using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServisApp.Areas.OrganizatorModul.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Areas.OrganizatorModul.ViewModels
{
    public class KlijentDodajVM
    {
        [HiddenInput]
        public int KlijentId { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [StringLength(200, ErrorMessageResourceName = "CustomStringLengthMax", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Display(Name = "puni naziv klijenta")]
        public string Naziv { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [StringLength(100, ErrorMessageResourceName = "CustomStringLengthMax", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Display(Name = "skraćeni naziv klijenta")]
        public string SkraceniNaziv { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [StringLength(15, ErrorMessageResourceName = "CustomStringLengthMax", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Remote(action: nameof(KlijentController.ProvjeraIdbrojaKlijenta), controller: "Klijent", AdditionalFields = nameof(KlijentId))]
        [RegularExpression("[0-9]{12}", ErrorMessageResourceName = "CustomREIdBroj", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Display(Name = "ID broj")]
        public string IdBroj { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [StringLength(100, ErrorMessageResourceName = "CustomStringLengthMax", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Display(Name = "ulicu")]
        public string Ulica { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [StringLength(100, ErrorMessageResourceName = "CustomStringLengthMax", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Display(Name = "ime i prezime kontakt osobe")]
        public string KontaktOsoba { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [StringLength(20, ErrorMessageResourceName = "CustomStringLengthMax", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("^(\\+[0-9]+)(\\s)([0-9]{2})(\\s)([0-9]{3})(\\s)([0-9]{3})$", ErrorMessageResourceName = "CustomREPhoneNumber", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Display(Name = "broj fiksnog telefona")]
        public string KontaktBrojFiksni { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [StringLength(20, ErrorMessageResourceName = "CustomStringLengthMax", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("^(\\+[0-9]+)(\\s)([0-9]{2})(\\s)([0-9]{3})(\\s)([0-9]{3,4})$", ErrorMessageResourceName = "CustomREMobileNumber", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Display(Name = "broj mobitela")]
        public string KontaktBrojMobitel { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [StringLength(100, ErrorMessageResourceName = "CustomStringLengthMax", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [EmailAddress(ErrorMessageResourceName = "CustomEmail", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Remote(action: nameof(KlijentController.ProvjeraEmailaKlijenta), controller: "Klijent", AdditionalFields = nameof(KlijentId))]
        [Display(Name = "kontakt email")]
        public string KontaktEmail { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Display(Name = "status klijenta")]
        public bool KlijentStatus { get; set; }

        public List<SelectListItem> Mjesto { get; set; }
        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceName = "CustomRegularExpressionSelectList", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Display(Name = "lokaciju sjedišta")]
        public int MjestoId { get; set; }
    }
}
