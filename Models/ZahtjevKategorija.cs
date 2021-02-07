using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Models
{
    public class ZahtjevKategorija
    {
        public int ZahtjevKategorijaId { get; set; }

        [Required]
        [StringLength(50)]
        public string Naziv { get; set; }

        [Required]
        [StringLength(255)]
        public string Opis { get; set; }

        #region Relationships
        public List<Zahtjev> Zahtjevi { get; set; }
        #endregion
    }
}
