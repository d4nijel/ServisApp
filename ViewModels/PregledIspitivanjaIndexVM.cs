using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.ViewModels
{
    public class PregledIspitivanjaIndexVM
    {
        public class Row
        {
            public int ObjekatId { get; set; }
            public string Klijent { get; set; }
            public string Objekat { get; set; }
            public string Kanton { get; set; }
            public string Opcina { get; set; }
            public string Mjesto { get; set; }
            public List<int> BrojDanaDoIstekaUsluge { get; set; }
        }
        public List<Row> Rows { get; set; }
        public Dictionary<int, string> Usluge { get; set; }

        public bool IsKlijent { get; set; }
        public string Klijent { get; set; }
        public bool IsUsluga { get; set; }
        public string Usluga { get; set; }
    }
}
