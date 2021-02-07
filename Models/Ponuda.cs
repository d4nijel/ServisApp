using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Models
{
    public class Ponuda
    {
        public int PonudaId { get; set; }

        [Required]
        [StringLength(50)]
        public string BrojPonude { get; set; }

        [Column(TypeName = "date")]
        public DateTime DatumIzdavanja { get; set; }

        [Column(TypeName = "money")]
        public decimal UkupanIznosBezPdv { get; set; }

        [Column(TypeName = "money")]
        public decimal UkupanIznosSaPdv { get; set; }

        [Required]
        public string PonudaPath { get; set; }

        public bool PonudaStatus { get; set; }

        #region Relationships
        public int KlijentId { get; set; }
        public Klijent Klijent { get; set; }
        public int KorisnikId { get; set; }
        public Korisnik Korisnik { get; set; }
        #endregion
    }
}
