using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Models
{
    public class RadniNalog
    {
        public int RadniNalogId { get; set; }

        [Required]
        [StringLength(50)]
        public string BrojRadnogNaloga { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DatumPocetkaRadova { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DatumZavrsetkaRadova { get; set; }

        [Required]
        public string RadniNalogPath { get; set; }

        #region Relationships
        public int ObjekatId { get; set; }
        public Objekat Objekat { get; set; }

        public List<Ispitivanje> Ispitivanja { get; set; }

        public List<ObavljeniPosao> ObavljeniPoslovi { get; set; }
        #endregion
    }
}
