using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Models
{
    public class Uloga
    {
        public int UlogaId { get; set; }

        [Required]
        [StringLength(50)]
        public string Naziv { get; set; }

        [Required]
        [StringLength(255)]
        public string Opis { get; set; }

        #region Relationships
        public List<Permisija> Permisije { get; set; }
        #endregion
    }
}
