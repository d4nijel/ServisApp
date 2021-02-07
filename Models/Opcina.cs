using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Models
{
    public class Opcina
    {
        public int OpcinaId { get; set; }

        [Required]
        [StringLength(100)]
        public string Naziv { get; set; }

        #region Relationships
        public List<Mjesto> Mjesta { get; set; }

        public int KantonId { get; set; }
        public Kanton Kanton { get; set; }
        #endregion
    }
}
