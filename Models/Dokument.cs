using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Models
{
    public class Dokument
    {
        public int DokumentId { get; set; }

        [Required]
        [StringLength(400)]
        public string Naziv { get; set; }

        [Required]
        [StringLength(50)]
        public string TipDokumenta { get; set; }

        [Column(TypeName = "date")]
        public DateTime DatumIzdavanja { get; set; }

        [StringLength(100)]
        public string SluzbeniList { get; set; }

        [Required]
        public string DokumentPath { get; set; }

        public bool DokumentStatus { get; set; }

        #region Relationships
        public int KorisnikId { get; set; }
        public Korisnik Korisnik { get; set; }
        #endregion
    }
}
