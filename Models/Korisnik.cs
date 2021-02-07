using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Models
{
    public class Korisnik
    {
        public int KorisnikId { get; set; }

        [Required]
        [StringLength(50)]
        public string Ime { get; set; }

        [Required]
        [StringLength(50)]
        public string Prezime { get; set; }

        [Required]
        [StringLength(100)]
        public string KorisnickoIme { get; set; }

        [Required]
        [StringLength(100)]
        public string LozinkaHash { get; set; }

        [Required]
        [StringLength(100)]
        public string LozinkaSalt { get; set; }

        [Required]
        public byte[] KorisnikSlika { get; set; }

        public bool KorisnikStatus { get; set; }

        #region Relationships
        public List<Poruka> Poruke { get; set; }

        public List<Zahtjev> Zahtjevi { get; set; }

        public List<Permisija> Permisije { get; set; }

        public List<Ugovor> Ugovori { get; set; }

        public List<Ponuda> Ponude { get; set; }

        public List<Dokument> Dokumenti { get; set; }

        public List<Izvjestaj> Izvjestaj { get; set; }

        public List<AutorizacijskiToken> AutorizacijskiTokeni { get; set; }
        #endregion
    }
}
