using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Areas.AdministratorModul.ViewModels
{
    public class NazivIspitivanjaIndexVM
    {
        public class Row
        {
            public int NazivIspitivanjaId { get; set; }
            public string Naziv { get; set; }
            public string Oznaka { get; set; }
            public int PeriodVazenja { get; set; }
            public bool NazivIspitivanjaStatus { get; set; }
            public bool DeleteBtn { get; set; }
        }
        public List<Row> Rows { get; set; }
    }
}
