using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Areas.OrganizatorModul.ViewModels
{
    public class PonudaIndexVM
    {
        public class Row
        {
            public int PonudaId { get; set; }
            public string BrojPonude { get; set; }
            public string DatumIzdavanja { get; set; }
            public string NazivKlijenta { get; set; }
            public bool PonudaStatus { get; set; }
            public decimal UkupanIznosSaPDV { get; set; }
        }
        public List<Row> Rows { get; set; }
    }
}
