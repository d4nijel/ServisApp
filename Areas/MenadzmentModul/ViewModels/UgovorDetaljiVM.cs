using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Areas.MenadzmentModul.ViewModels
{
    public class UgovorDetaljiVM
    {
        public string BrojUgovora { get; set; }

        public string Naziv { get; set; }

        public string DatumPotpisivanja { get; set; }

        public string DatumIsteka { get; set; }

        public string UgovorPath { get; set; }

        public bool UgovorStatus { get; set; }

        public string Klijent { get; set; }
    }
}
