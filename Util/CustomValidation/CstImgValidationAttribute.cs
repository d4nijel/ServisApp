using Microsoft.AspNetCore.Http;
using ServisApp.Areas.AdministratorModul.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Util.CustomValidation
{
    public class CstImgValidationAttribute : ValidationAttribute
    {
        public CstImgValidationAttribute(int velicina, string ekstenzija)
        {
            Velicina = velicina;
            EkstenzijaList = ekstenzija.Split(',').ToList();
            Ekstenzija = String.Join(", ", EkstenzijaList.ToArray());
        }

        public int Velicina { get; }
        public List<string> EkstenzijaList { get; }
        public string Ekstenzija { get; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var fileExtension = Path.GetExtension(file.FileName);
                var size = file.Length;
                bool test = false;

                foreach (var item in EkstenzijaList)
                {
                    if (fileExtension.ToUpper() == ("." + item).ToUpper())
                    {
                        test = true;
                        break;
                    }
                }

                if (test == false)
                {
                    string GetErrorMessage() => $"Slika mora biti ekstenzije {Ekstenzija}";
                    return new ValidationResult(GetErrorMessage());
                }

                if (size > Velicina * 1024)
                {
                    string GetErrorMessage() => $"Slika je prevelika. Maksimalna veličina slike je {Velicina} KB.";
                    return new ValidationResult(GetErrorMessage());
                }
            }
            return ValidationResult.Success;
        }
    }
}
