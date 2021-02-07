using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Areas.InzinjerModul.ViewModels
{
    public class ClanServisaIndexVM
    {
        public class Row
        {
            public int ClanServisaId { get; set; }
            public string ImePrezime { get; set; }
            public string BrojMobitela { get; set; }
            public string Zanimanje { get; set; }
            public bool ClanServisaStatus { get; set; }
            public bool DeleteBtn { get; set; }
        }
        public List<Row> Rows { get; set; }
    }
}

