using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Areas.OrganizatorModul.ViewModels
{
    public class ObjekatIndexVM
    {
        public class Row
        {
            public int ObjekatId { get; set; }
            public string Naziv { get; set; }
            public string Lokacija { get; set; }
            public string Klijent { get; set; }
            public int BrojRadnihNaloga { get; set; }
            public bool ObjekatStatus { get; set; }
            public bool DeleteBtn { get; set; }

            public string KontaktOsoba { get; set; }
            public string KontaktMobitel { get; set; }
        }
        public List<Row> Rows { get; set; }
    }
}
