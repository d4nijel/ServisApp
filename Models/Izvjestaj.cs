using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Models
{
    public class Izvjestaj
    {
        public int IzvjestajId { get; set; }

        [Required]
        [StringLength(50)]
        public string BrojIzvjestaja { get; set; }

        [Column(TypeName = "date")]
        public DateTime DatumKreiranja { get; set; }

        [Required]
        public string IzvjestajPath { get; set; }

        public bool IzvjestajStatus { get; set; }

        #region Relationships
        public int IspitivanjeId { get; set; }
        public Ispitivanje Ispitivanje { get; set; }

        public int KorisnikId { get; set; }
        public Korisnik Korisnik { get; set; }
        #endregion
    }
}
