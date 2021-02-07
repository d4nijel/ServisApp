using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Models
{
    public class Kanton
    {
        public int KantonId { get; set; }

        [Required]
        [StringLength(50)]
        public string Naziv { get; set; }

        [Required]
        [StringLength(5)]
        public string SkraceniNaziv { get; set; }

        #region Relationships
        public List<Opcina> Opcine { get; set; }
        #endregion
    }
}
