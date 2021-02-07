using Microsoft.AspNetCore.Mvc;
using ServisApp.Areas.KlijentModul.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Areas.KlijentModul.ViewModels
{
    public class KlijentUrediVM
    {
        [HiddenInput]
        public int KlijentId { get; set; }

        [HiddenInput]
        public string Naziv { get; set; }

        [HiddenInput]
        public string IdBroj { get; set; }

        [HiddenInput]
        public string Ulica { get; set; }

        [HiddenInput]
        public string Sjediste { get; set; }

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
        [Remote(action: nameof(KontaktController.ProvjeraEmailaKlijenta), controller: "Kontakt", AdditionalFields = nameof(KlijentId))]
        [Display(Name = "kontakt email")]
        public string KontaktEmail { get; set; }
    }
}
