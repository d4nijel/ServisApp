using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Areas.AdministratorModul.ViewModels
{
    public class MjestoIndexVM
    {
        public class Row
        {
            public int MjestoId { get; set; }
            public string Naziv { get; set; }
            public string Opcina { get; set; }
            public string Kanton { get; set; }
            public bool DeleteBtn { get; set; }
        }
        public List<Row> Rows { get; set; }
    }
}
