using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Models
{
    public class AutorizacijskiToken
    {
        public int AutorizacijskiTokenId { get; set; }
        public string Vrijednost { get; set; }
        public DateTime VrijemeEvidentiranja { get; set; }
        public string IpAdresa { get; set; }

        #region Relationships  
        public int? KorisnikId { get; set; }
        public Korisnik Korisnik { get; set; }

        public int? KlijentskiRacunId { get; set; }
        public KlijentskiRacun KlijentskiRacun { get; set; }
        #endregion
    }
}
