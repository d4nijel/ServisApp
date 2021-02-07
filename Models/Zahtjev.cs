using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Models
{
    public class Zahtjev
    {
        public int ZahtjevId { get; set; }

        [Required]
        [StringLength(100)]
        public string Naslov { get; set; }

        [Required]
        [StringLength(1000)]
        public string Opis { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DatumKreiranja { get; set; }

        #region Relationships
        public int KlijentskiRacunId { get; set; }
        public KlijentskiRacun KlijentskiRacun { get; set; }

        public List<Poruka> Poruke { get; set; }

        public int ZahtjevKategorijaId { get; set; }
        public ZahtjevKategorija ZahtjevKategorija { get; set; }

        public int ZahtjevStatusId { get; set; }
        public ZahtjevStatus ZahtjevStatus { get; set; }

        public int? KorisnikId { get; set; }
        public Korisnik Korisnik { get; set; }
        #endregion
    }
}
