using ServisApp.Areas.InzinjerModul.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Util.CustomValidation
{
    public class DatumZavrsetkaVeceOdDatumPocetkaAttribute : ValidationAttribute
    {
        public string GetErrorMessage() => $"Datum i vrijeme završetka radova mora biti veće od datuma i vremena početka radova";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var radniNalog = (RadniNalogDodajVM)validationContext.ObjectInstance;

                var datumPocetkaRadova = radniNalog.DatumPocetkaRadova;
                var datumZavrsetkaRadova = radniNalog.DatumZavrsetkaRadova;

                if (datumZavrsetkaRadova <= datumPocetkaRadova)
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }
            return ValidationResult.Success;
        }
    }
}
