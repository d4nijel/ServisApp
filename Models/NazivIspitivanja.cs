using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Models
{
    public class NazivIspitivanja
    {
        public int NazivIspitivanjaId { get; set; }

        [Required]
        [StringLength(100)]
        public string Naziv { get; set; }

        [Required]
        [StringLength(50)]
        public string Oznaka { get; set; }

        [Column(TypeName = "tinyint")]
        public int PeriodVazenja { get; set; }

        public bool NazivIspitivanjaStatus { get; set; }

        #region Relationships
        public List<Ispitivanje> Ispitivanja { get; set; }
        #endregion
    }
}
