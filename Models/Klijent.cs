using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Models
{
    public class Klijent
    {
        public int KlijentId { get; set; }

        [Required]
        [StringLength(200)]
        public string Naziv { get; set; }

        [Required]
        [StringLength(100)]
        public string SkraceniNaziv { get; set; }

        [Required]
        [StringLength(15)]
        public string IdBroj { get; set; }

        [Required]
        [StringLength(100)]
        public string Ulica { get; set; }

        [Required]
        [StringLength(100)]
        public string KontaktOsoba { get; set; }

        [Required]
        [StringLength(20)]
        public string KontaktBrojFiksni { get; set; }

        [Required]
        [StringLength(20)]
        public string KontaktBrojMobitel { get; set; }

        [Required]
        [StringLength(100)]
        public string KontaktEmail { get; set; }

        public bool KlijentStatus { get; set; }

        #region Relationships
        public List<Objekat> Objekti { get; set; }

        public int MjestoId { get; set; }
        public Mjesto Mjesto { get; set; }

        public List<Ugovor> Ugovori { get; set; }

        public List<Ponuda> Ponuda { get; set; }

        public KlijentskiRacun KlijentskiRacun { get; set; }
        #endregion
    }
}
