using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Models
{
    public class ClanServisa
    {
        public int ClanServisaId { get; set; }

        [Required]
        [StringLength(50)]
        public string Ime { get; set; }

        [Required]
        [StringLength(50)]
        public string Prezime { get; set; }

        [StringLength(20)]
        public string BrojMobitela { get; set; }

        [Required]
        [StringLength(100)]
        public string Zanimanje { get; set; }

        public bool ClanServisaStatus { get; set; }

        #region Relationships
        public List<ObavljeniPosao> ObavljeniPoslovi { get; set; }
        #endregion
    }
}
