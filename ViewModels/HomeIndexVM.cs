using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.ViewModels
{
    public class HomeIndexVM
    {
        public int BrojKlijenata { get; set; }
        public int BrojObjekata { get; set; }
        public int BrojRadnihNaloga { get; set; }
        public int BrojIspitivanja { get; set; }
        public int BrojIzvještaja { get; set; }
        public string Mjesec { get; set; }
        public string Godina { get; set; }
        public int BrojPonudaMjesec { get; set; }
        public int BrojUgovoraMjesec { get; set; }
        public int BrojRadnihNalogaMjesec { get; set; }
        public int BrojIspitivanjaMjesec { get; set; }
        public int BrojZahtjevaMjesec { get; set; }
        public int BrojIzvještajaMjesec { get; set; }

        public int BrojZahtjevaZaDodijeliti { get; set; }
        public int BrojZahtjeva { get; set; }
        public int BrojArhiviranihZahtjeva { get; set; }

        public int BrojPrihvacenihPonudaMjesec { get; set; }
        public decimal UkupanIznosBezPdvMjesec { get; set; }
        public decimal UkupanIznosSaPdvMjesec { get; set; }
        public int BrojPrihvacenihPonuda { get; set; }
        public decimal UkupanIznosBezPdv { get; set; }
        public decimal UkupanIznosSaPdv { get; set; }

        public bool IsAdministrator { get; set; }
        public bool IsOrganizator { get; set; }
        public bool IsInzinjer { get; set; }
        public bool IsMenadzment { get; set; }
        public bool IsKlijent { get; set; }

        public class KorisnikPodaci
        {
            public string KorisnikSlikaPath { get; set; }
            public string ImePrezimeKorisnika { get; set; }
        }

        public List<KorisnikPodaci> KorisniciPodaci { get; set; }
    }
}
