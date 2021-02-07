using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Areas.MenadzmentModul.ViewModels
{
    public class DokumentIndexVM
    {
        public class Row
        {
            public int DokumentId { get; set; }
            public string Naziv { get; set; }
            public string TipDokumenta { get; set; }
            public string DatumIzdavanja { get; set; }
            public string SluzbeniList { get; set; }
            public string DokumentPath { get; set; }
            public bool DokumentStatus { get; set; }
            public string Korisnik { get; set; }
        }
        public List<Row> Rows { get; set; }
    }
}
