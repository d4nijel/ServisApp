using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Models
{
    public class Objekat
    {
        public int ObjekatId { get; set; }

        [Required]
        [StringLength(100)]
        public string Naziv { get; set; }

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

        public bool ObjekatStatus { get; set; }

        #region Relationships
        public int KlijentId { get; set; }
        public Klijent Klijent { get; set; }

        public int MjestoId { get; set; }
        public Mjesto Mjesto { get; set; }

        public List<RadniNalog> RadniNalozi { get; set; }
        #endregion
    }
}
