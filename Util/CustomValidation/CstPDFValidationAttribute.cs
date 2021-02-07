using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Util.CustomValidation
{
    public class CstPDFValidationAttribute : ValidationAttribute
    {
        public CstPDFValidationAttribute(int velicina)
        {
            Velicina = velicina;
            Ekstenzija = ".pdf";
        }

        public int Velicina { get; }
        public string Ekstenzija { get; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var x = value as IFormFile;

                var fileExtension = Path.GetExtension(x.FileName);
                var size = x.Length;

                if (fileExtension.ToUpper() != Ekstenzija.ToUpper())
                {
                    string GetErrorMessage() => $"Dokument mora biti ekstenzije {Ekstenzija}";
                    return new ValidationResult(GetErrorMessage());
                }

                if (size > Velicina * 1024 * 1024)
                {
                    string GetErrorMessage() => $"Dokument je preveliki. Maksimalna veličina dokumenta je {Velicina} MB.";
                    return new ValidationResult(GetErrorMessage());
                }
            }
            return ValidationResult.Success;
        }
    }
}
