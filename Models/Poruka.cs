using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Models
{
    public class Poruka
    {
        public int PorukaId { get; set; }

        [Required]
        [StringLength(1000)]
        public string Sadrzaj { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DatumKreiranja { get; set; }

        #region Relationships
        public int? KlijentskiRacunId { get; set; }
        public KlijentskiRacun KlijentskiRacun { get; set; }
        public int? KorisnikId { get; set; }
        public Korisnik Korisnici { get; set; }
        public int ZahtjevId { get; set; }
        public Zahtjev Zahtjev { get; set; }
        #endregion
    }
}
