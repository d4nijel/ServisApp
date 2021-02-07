using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Models
{
    public class Mjesto
    {
        public int MjestoId { get; set; }

        [Required]
        [StringLength(100)]
        public string Naziv { get; set; }

        #region Relationships
        public List<Klijent> Klijenti { get; set; }

        public List<Objekat> Objekti { get; set; }

        public int OpcinaId { get; set; }
        public Opcina Opcina { get; set; }
        #endregion
    }
}
