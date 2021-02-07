using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Areas.InzinjerModul.ViewModels
{
    public class RadniNalogIndexVM
    {
        public class Row
        {
            public int RadniNalogId { get; set; }
            public string BrojRadnogNaloga { get; set; }
            public string DatumPocetkaRadova { get; set; }
            public string DatumZavrsetkaRadova { get; set; }
            public string UkupnoSatiRada { get; set; }
            public string NazivKlijenta { get; set; }
            public string NazivObjekta { get; set; }
            public int BrojIspitivanja { get; set; }
            public int NedostajeIzvjestaja { get; set; }
            public bool DeleteBtn { get; set; }
        }
        public List<Row> Rows { get; set; }
    }
}
