using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Models
{
    public class Ugovor
    {
        public int UgovorId { get; set; }

        [Required]
        [StringLength(50)]
        public string BrojUgovora { get; set; }

        [Required]
        [StringLength(200)]
        public string Naziv { get; set; }

        [Column(TypeName = "date")]
        public DateTime DatumPotpisivanja { get; set; }

        [Column(TypeName = "date")]
        public DateTime DatumIsteka { get; set; }

        [Required]
        public string UgovorPath { get; set; }

        public bool UgovorStatus { get; set; }

        #region Relationships
        public int KlijentId { get; set; }
        public Klijent Klijent { get; set; }

        public int KorisnikId { get; set; }
        public Korisnik Korisnik { get; set; }
        #endregion
    }
}
