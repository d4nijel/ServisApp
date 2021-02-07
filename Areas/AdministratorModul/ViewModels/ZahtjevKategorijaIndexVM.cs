using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Areas.AdministratorModul.ViewModels
{
    public class ZahtjevKategorijaIndexVM
    {
        public class Row
        {
            public int ZahtjevKategorijaId { get; set; }
            public string Naziv { get; set; }
            public string Opis { get; set; }
            public bool DeleteBtn { get; set; }
        }
        public List<Row> Rows { get; set; }
    }
}
