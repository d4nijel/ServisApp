using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Areas.InzinjerModul.ViewModels
{
    public class IspitivanjeIndexVM
    {
        public class Row
        {
            public int IspitivanjeId { get; set; }
            public string NazivIspitivanja { get; set; }
            public string DatumIspitivanja { get; set; }
            public string DatumNarednogIspitivanja { get; set; }
            public int BroDanaDoNarednogIspitivanja { get; set; }
            public string TipIspitivanja { get; set; }
            public bool PostojanjeIzvjestaja { get; set; }
            public int IzvjestajId { get; set; }
            public bool DeleteBtn { get; set; }
        }
        public List<Row> Rows { get; set; }
        public int RadniNalogId { get; set; }
    }
}
