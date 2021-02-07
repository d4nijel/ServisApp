using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Areas.MenadzmentModul.ViewModels
{
    public class UgovorIndexVM
    {
        public class Row
        {
            public int UgovorId { get; set; }
            public string BrojUgovora { get; set; }
            public string Naziv { get; set; }
            public string Klijent { get; set; }
            public string DatumPotpisivanja { get; set; }
            public bool UgovorStatus { get; set; }
        }
        public List<Row> Rows { get; set; }
    }
}
