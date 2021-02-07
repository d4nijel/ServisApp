using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Areas.AdministratorModul.ViewModels
{
    public class KorisnikIndexVM
    {
        public class Row
        {
            public int KorisnikId { get; set; }
            public string ImePrezime { get; set; }
            public string KorisnickoIme { get; set; }
            public byte[] KorisnikSlika { get; set; }
            public string KorisnikSlikaPath { get; set; }
            public bool KorisnikStatus { get; set; }
            public bool DeleteBtn { get; set; }
        }
        public List<Row> Rows { get; set; }
    }
}
