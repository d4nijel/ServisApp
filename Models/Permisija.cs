using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Models
{
    public class Permisija
    {
        public int PermisijaId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DatumIzmjene { get; set; }

        public bool PermisijaStatus { get; set; }

        #region Relationships  
        public int KorisnikId { get; set; }
        public Korisnik Korisnik { get; set; }

        public int UlogaId { get; set; }
        public Uloga Uloga { get; set; }
        #endregion
    }
}
