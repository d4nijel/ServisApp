using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Areas.AdministratorModul.ViewModels
{
    public class OpcinaIndexVM
    {
        public class Row
        {
            public int OpcinaId { get; set; }
            public string Naziv { get; set; }
            public string Kanton { get; set; }
            public int BrojMjesta { get; set; }
            public bool DeleteBtn { get; set; }
        }
        public List<Row> Rows { get; set; }
    }
}
