using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.Models
{
    public class ObavljeniPosao
    {
        #region Relationships
        public int ClanServisaId { get; set; }
        public ClanServisa ClanServisa { get; set; }
        public int RadniNalogId { get; set; }
        public RadniNalog RadniNalog { get; set; }
        #endregion
    }
}
