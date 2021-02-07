using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Areas.InzinjerModul.ViewModels
{
    public class ClanServisaDetaljiVM
    {
        [HiddenInput]
        public int ClanServisaId { get; set; }

        [HiddenInput]
        public string Ime { get; set; }

        [HiddenInput]
        public string Prezime { get; set; }

        [StringLength(20, ErrorMessageResourceName = "CustomStringLengthMax", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("^(\\+[0-9]+)(\\s)([0-9]{2})(\\s)([0-9]{3})(\\s)([0-9]{3,4})$",
            ErrorMessageResourceName = "CustomREMobileNumber", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Display(Name = "broj mobitela")]
        public string BrojMobitela { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [StringLength(100, ErrorMessageResourceName = "CustomStringLengthMax", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Display(Name = "zanimanje")]
        public string Zanimanje { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Display(Name = "status clana servisa")]
        public bool ClanServisaStatus { get; set; }
    }
}
