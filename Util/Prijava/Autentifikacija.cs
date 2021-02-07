using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ServisApp.Data;
using ServisApp.ViewModels;
using System;
using ServisApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ServisApp.Util.Prijava
{
    public static class Autentifikacija
    {
        private const string logiraniKorisnik = "logirani_korisnik";

        public static void SetLogiraniKorisnik(this HttpContext context, AutentifikacijaVM korisnik, bool snimiUCookie = false)
        {
            if (snimiUCookie)
            {
                MojContext db = context.RequestServices.GetService<MojContext>();

                string stariToken = context.Request.GetCookieJson<string>(logiraniKorisnik);

                if (stariToken != null)
                {
                    AutorizacijskiToken obrisati = db.AutorizacijskiTokeni.FirstOrDefault(x => x.Vrijednost == stariToken);

                    if (obrisati != null)
                    {
                        db.AutorizacijskiTokeni.Remove(obrisati);
                        db.SaveChanges();
                    }
                }

                if (korisnik != null)
                {
                    string token = Guid.NewGuid().ToString();

                    AutorizacijskiToken at = new AutorizacijskiToken()
                    {
                        Vrijednost = token,
                        VrijemeEvidentiranja = DateTime.Now,
                        IpAdresa = context.Connection.RemoteIpAddress.ToString()
                    };

                    if (korisnik.IsKlijent)
                    {
                        at.KlijentskiRacunId = korisnik.KlijentskiRacunId;
                    }
                    else
                    {
                        at.KorisnikId = korisnik.KorisnikId;
                    }

                    db.AutorizacijskiTokeni.Add(at);
                    db.SaveChanges();

                    context.Response.SetCookieJson(logiraniKorisnik, token);
                }
            }
            else
            {
                context.Session.Set(logiraniKorisnik, korisnik);
                context.Response.SetCookieJson(logiraniKorisnik, null);
            }
        }

        public static string GetTrenutniToken(this HttpContext context)
        {
            return context.Request.GetCookieJson<string>(logiraniKorisnik);
        }

        public static AutentifikacijaVM GetLogiraniKorisnik(this HttpContext context)
        {
            AutentifikacijaVM korisnik = context.Session.Get<AutentifikacijaVM>(logiraniKorisnik);

            if (korisnik == null)
            {
                MojContext db = context.RequestServices.GetService<MojContext>();

                string token = context.Request.GetCookieJson<string>(logiraniKorisnik);

                if (token == null)
                    return null;

                AutorizacijskiToken at = db.AutorizacijskiTokeni.Where(w => w.Vrijednost == token).Include(i => i.KlijentskiRacun).Include(j => j.Korisnik).SingleOrDefault();

                korisnik = new AutentifikacijaVM();

                if (at != null)
                {
                    if (at.KlijentskiRacun != null)
                    {
                        korisnik.ImePrezime = at.KlijentskiRacun.Ime + " " + at.KlijentskiRacun.Prezime;
                        korisnik.KlijentskiRacunId = at.KlijentskiRacunId.Value;
                        korisnik.KorisnickoIme = at.KlijentskiRacun.KorisnickoIme;
                        korisnik.IsKlijent = true;

                        var x = ImageHelper.GetImageType(at.KlijentskiRacun.KlijentskiRacunSlika);
                        korisnik.KlijentskiRacunSlikaPath = string.Format("data:image/" + x + ";base64,{0}", Convert.ToBase64String(at.KlijentskiRacun.KlijentskiRacunSlika));
                    }
                    else
                    {
                        korisnik.ImePrezime = at.Korisnik.Ime + " " + at.Korisnik.Prezime;
                        korisnik.KorisnikId = at.KorisnikId.Value;
                        korisnik.KorisnickoIme = at.Korisnik.KorisnickoIme;

                        var ulogeList = db.Permisije.Where(w => w.KorisnikId == at.KorisnikId && w.PermisijaStatus == true).Select(s => s.UlogaId).ToList();

                        if (ulogeList != null)
                        {
                            foreach (var item in ulogeList)
                            {
                                //provjeravamo da li je logirani korisnik Administrator, Organizator, Inzinjer, Menadžment
                                switch (item)
                                {
                                    case 1:
                                        korisnik.IsAdministrator = true;
                                        break;
                                    case 2:
                                        korisnik.IsOrganizator = true;
                                        break;
                                    case 3:
                                        korisnik.IsInzinjer = true;
                                        break;
                                    case 4:
                                        korisnik.IsMenadzment = true;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }

                        var x = ImageHelper.GetImageType(at.Korisnik.KorisnikSlika);
                        korisnik.KorisnikSlikaPath = string.Format("data:image/" + x + ";base64,{0}", Convert.ToBase64String(at.Korisnik.KorisnikSlika));
                    }
                }
            }
            return korisnik;
        }
    }
}
