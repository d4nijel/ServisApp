using ServisApp.Areas.MenadzmentModul.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Util.CustomValidation
{
    public class CstDatumVrijemeUgovorAttribute : ValidationAttribute
    {
        public string GetErrorMessage() => $"Datum isteka ugovora ne može biti stariji ili jednak datumu potpisivanja ugovora";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var ugovor = (UgovorDodajVM)validationContext.ObjectInstance;
                var datumPotpisivanja = ugovor.DatumPotpisivanja;
                var datumIsteka = ugovor.DatumIsteka;

                if (datumIsteka <= datumPotpisivanja)
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }
    }
}
