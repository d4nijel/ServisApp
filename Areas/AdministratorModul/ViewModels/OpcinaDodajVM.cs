using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServisApp.Areas.AdministratorModul.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Areas.AdministratorModul.ViewModels
{
    public class OpcinaDodajVM
    {
        [HiddenInput]
        public int OpcinaId { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [StringLength(100, ErrorMessageResourceName = "CustomStringLengthMax", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Remote(action: nameof(OpcinaController.ProvjeraNazivaOpcine), controller: "Opcina", AdditionalFields = nameof(KantonId) + "," + nameof(OpcinaId))]
        [Display(Name = "naziv općine")]
        public string Naziv { get; set; }

        public List<SelectListItem> Kantoni { get; set; }

        [Required(ErrorMessageResourceName = "CustomRequired", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceName = "CustomRegularExpressionSelectList", ErrorMessageResourceType = typeof(Util.CustomErrorMessages))]
        [Display(Name = "kanton")]
        public int KantonId { get; set; }
    }
}
