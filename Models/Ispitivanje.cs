using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Models
{
    public class Ispitivanje
    {
        public int IspitivanjeId { get; set; }

        [Column(TypeName = "date")]
        public DateTime DatumIspitivanja { get; set; }

        [Column(TypeName = "date")]
        public DateTime DatumNarednogIspitivanja { get; set; }

        [Required]
        [StringLength(50)]
        public string TipIspitivanja { get; set; }

        [StringLength(255)]
        public string Napomena { get; set; }

        #region Relationships
        public int RadniNalogId { get; set; }
        public RadniNalog RadniNalog { get; set; }

        public int NazivIspitivanjaId { get; set; }
        public NazivIspitivanja NazivIspitivanja { get; set; }

        public Izvjestaj Izvjestaj { get; set; }
        #endregion
    }
}
