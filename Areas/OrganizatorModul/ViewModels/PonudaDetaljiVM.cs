using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Areas.OrganizatorModul.ViewModels
{
    public class PonudaDetaljiVM
    {
        [HiddenInput]
        public int PonudaId { get; set; }
        public string BrojPonude { get; set; }
        public string DatumIzdavanja { get; set; }
        public decimal UkupanIznosBezPdv { get; set; }      
        public decimal PDV { get; set; }
        public string PonudaPath { get; set; }
        public bool PonudaStatus { get; set; }
        public string NazivKlijenta { get; set; }
        public string Korisnik { get; set; }
        public decimal UkupanIznosSaPdv { get; set; }
    }
}
