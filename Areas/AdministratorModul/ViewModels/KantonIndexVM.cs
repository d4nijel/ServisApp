using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Areas.AdministratorModul.ViewModels
{
    public class KantonIndexVM
    {
        public class Row
        {
            public int KantonId { get; set; }
            public string Naziv { get; set; }
            public string SkraceniNaziv { get; set; }
            public int BrojOpcina { get; set; }
            public bool DeleteBtn { get; set; }
        }
        public List<Row> Rows { get; set; }
    }
}
