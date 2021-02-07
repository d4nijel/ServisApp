using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Models
{
    public class KlijentskiRacun
    {
        public int KlijentskiRacunId { get; set; }

        [Required]
        [StringLength(50)]
        public string Ime { get; set; }

        [Required]
        [StringLength(50)]
        public string Prezime { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DatumRegistracije { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

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
        public byte[] KlijentskiRacunSlika { get; set; }

        public bool EmailNotifikacija { get; set; }

        [Column(TypeName = "tinyint")]
        public int BrojDanaPrijeIsteka { get; set; }

        public bool KlijentskiRacunStatus { get; set; }

        #region Relationships
        public int KlijentId { get; set; }
        public Klijent Klijent { get; set; }

        public List<Zahtjev> Zahtjevi { get; set; }

        public List<Poruka> Poruke { get; set; }

        public List<AutorizacijskiToken> AutorizacijskiTokeni { get; set; }
        #endregion
    }
}
