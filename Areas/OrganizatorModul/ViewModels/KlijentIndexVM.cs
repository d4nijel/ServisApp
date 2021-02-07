using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Areas.OrganizatorModul.ViewModels
{
    public class KlijentIndexVM
    {
        public class Row
        {
            public int KlijentId { get; set; }
            public string Naziv { get; set; }
            public string IdBroj { get; set; }
            public string Lokacija { get; set; }
            public int BrojObjekata { get; set; }
            public bool KlijentStatus { get; set; }
            public bool PostojanjeProfila { get; set; }
            public bool KlijentskiRacunStatus { get; set; }
            public int KlijentskiRacunId { get; set; }
            public bool DeleteBtn { get; set; }
            public bool ProfilDeleteBtn { get; set; }
        }
        public List<Row> Rows { get; set; }
    }
}
