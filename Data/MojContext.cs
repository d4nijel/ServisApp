using Microsoft.EntityFrameworkCore;
using ServisApp.Models;

namespace ServisApp.Data
{
    public class MojContext : DbContext
    {
        public MojContext(DbContextOptions<MojContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Objekat>().HasOne(o => o.Klijent).WithMany(k => k.Objekti).HasForeignKey(f => f.KlijentId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Klijent>().HasOne(k => k.Mjesto).WithMany(m => m.Klijenti).HasForeignKey(f => f.MjestoId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Objekat>().HasOne(o => o.Mjesto).WithMany(m => m.Objekti).HasForeignKey(f => f.MjestoId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Mjesto>().HasOne(m => m.Opcina).WithMany(o => o.Mjesta).HasForeignKey(f => f.OpcinaId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Opcina>().HasOne(o => o.Kanton).WithMany(k => k.Opcine).HasForeignKey(f => f.KantonId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<RadniNalog>().HasOne(r => r.Objekat).WithMany(o => o.RadniNalozi).HasForeignKey(f => f.ObjekatId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Ispitivanje>().HasOne(i => i.RadniNalog).WithMany(r => r.Ispitivanja).HasForeignKey(f => f.RadniNalogId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Ispitivanje>().HasOne(i => i.NazivIspitivanja).WithMany(n => n.Ispitivanja).HasForeignKey(f => f.NazivIspitivanjaId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Ugovor>().HasOne(u => u.Klijent).WithMany(k => k.Ugovori).HasForeignKey(f => f.KlijentId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Ponuda>().HasOne(p => p.Klijent).WithMany(k => k.Ponuda).HasForeignKey(f => f.KlijentId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Zahtjev>().HasOne(z => z.KlijentskiRacun).WithMany(kr => kr.Zahtjevi).HasForeignKey(f => f.KlijentskiRacunId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Poruka>().HasOne(p => p.KlijentskiRacun).WithMany(kr => kr.Poruke).HasForeignKey(f => f.KlijentskiRacunId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Poruka>().HasOne(p => p.Korisnici).WithMany(k => k.Poruke).HasForeignKey(f => f.KorisnikId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Poruka>().HasOne(p => p.Zahtjev).WithMany(z => z.Poruke).HasForeignKey(f => f.ZahtjevId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Zahtjev>().HasOne(z => z.ZahtjevKategorija).WithMany(zk => zk.Zahtjevi).HasForeignKey(f => f.ZahtjevKategorijaId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Zahtjev>().HasOne(z => z.ZahtjevStatus).WithMany(zs => zs.Zahtjevi).HasForeignKey(f => f.ZahtjevStatusId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Zahtjev>().HasOne(z => z.Korisnik).WithMany(k => k.Zahtjevi).HasForeignKey(f => f.KorisnikId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Permisija>().HasOne(p => p.Korisnik).WithMany(p => p.Permisije).HasForeignKey(f => f.KorisnikId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Permisija>().HasOne(p => p.Uloga).WithMany(p => p.Permisije).HasForeignKey(f => f.UlogaId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Ugovor>().HasOne(u => u.Korisnik).WithMany(k => k.Ugovori).HasForeignKey(f => f.KorisnikId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Ponuda>().HasOne(p => p.Korisnik).WithMany(k => k.Ponude).HasForeignKey(f => f.KorisnikId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Dokument>().HasOne(d => d.Korisnik).WithMany(k => k.Dokumenti).HasForeignKey(f => f.KorisnikId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Izvjestaj>().HasOne(i => i.Korisnik).WithMany(k => k.Izvjestaj).HasForeignKey(f => f.KorisnikId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<AutorizacijskiToken>().HasOne(a => a.Korisnik).WithMany(k => k.AutorizacijskiTokeni).HasForeignKey(f => f.KorisnikId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<AutorizacijskiToken>().HasOne(a => a.KlijentskiRacun).WithMany(kr => kr.AutorizacijskiTokeni).HasForeignKey(f => f.KlijentskiRacunId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ObavljeniPosao>().HasKey(op => new { op.ClanServisaId, op.RadniNalogId });
            modelBuilder.Entity<ObavljeniPosao>().HasOne(op => op.ClanServisa).WithMany(cs => cs.ObavljeniPoslovi).HasForeignKey(f => f.ClanServisaId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ObavljeniPosao>().HasOne(op => op.RadniNalog).WithMany(rn => rn.ObavljeniPoslovi).HasForeignKey(f => f.RadniNalogId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ispitivanje>().HasOne(i => i.Izvjestaj).WithOne(i => i.Ispitivanje).HasForeignKey<Izvjestaj>(i => i.IspitivanjeId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Klijent>().HasOne(k => k.KlijentskiRacun).WithOne(kr => kr.Klijent).HasForeignKey<KlijentskiRacun>(i => i.KlijentId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Izvjestaj>().HasIndex(i => i.BrojIzvjestaja).IsUnique();
            modelBuilder.Entity<Kanton>().HasIndex(k => k.Naziv).IsUnique();
            modelBuilder.Entity<Kanton>().HasIndex(k => k.SkraceniNaziv).IsUnique();
            modelBuilder.Entity<Klijent>().HasIndex(k => k.IdBroj).IsUnique();
            modelBuilder.Entity<Klijent>().HasIndex(k => k.KontaktEmail).IsUnique();
            modelBuilder.Entity<KlijentskiRacun>().HasIndex(kr => kr.Email).IsUnique();
            modelBuilder.Entity<KlijentskiRacun>().HasIndex(kr => kr.KorisnickoIme).IsUnique();
            modelBuilder.Entity<Korisnik>().HasIndex(k => k.KorisnickoIme).IsUnique();
            modelBuilder.Entity<NazivIspitivanja>().HasIndex(ni => ni.Oznaka).IsUnique();
            modelBuilder.Entity<Objekat>().HasIndex(o => o.KontaktEmail).IsUnique();
            modelBuilder.Entity<Ponuda>().HasIndex(p => p.BrojPonude).IsUnique();
            modelBuilder.Entity<RadniNalog>().HasIndex(r => r.BrojRadnogNaloga).IsUnique();
            modelBuilder.Entity<Ugovor>().HasIndex(u => u.BrojUgovora).IsUnique();
            modelBuilder.Entity<Uloga>().HasIndex(u => u.Naziv).IsUnique();
        }

        #region DbSets
        public DbSet<AutorizacijskiToken> AutorizacijskiTokeni { get; set; }
        public DbSet<ClanServisa> ClanoviServisa { get; set; }
        public DbSet<Dokument> Dokumenti { get; set; }
        public DbSet<Ispitivanje> Ispitivanja { get; set; }
        public DbSet<Izvjestaj> Izvjestaji { get; set; }
        public DbSet<Kanton> Kantoni { get; set; }
        public DbSet<Klijent> Klijenti { get; set; }
        public DbSet<KlijentskiRacun> KlijentskiRacuni { get; set; }
        public DbSet<Korisnik> Korisnici { get; set; }
        public DbSet<Mjesto> Mjesta { get; set; }
        public DbSet<NazivIspitivanja> NaziviIspitivanja { get; set; }
        public DbSet<ObavljeniPosao> ObavljeniPoslovi { get; set; }
        public DbSet<Objekat> Objekti { get; set; }
        public DbSet<Opcina> Opcine { get; set; }
        public DbSet<Permisija> Permisije { get; set; }
        public DbSet<Ponuda> Ponude { get; set; }
        public DbSet<Poruka> Poruke { get; set; }
        public DbSet<RadniNalog> RadniNalozi { get; set; }
        public DbSet<Ugovor> Ugovori { get; set; }
        public DbSet<Uloga> Uloge { get; set; }
        public DbSet<Zahtjev> Zahtjevi { get; set; }
        public DbSet<ZahtjevKategorija> ZahtjeviKategorije { get; set; }
        public DbSet<ZahtjevStatus> ZahtjeviStatusi { get; set; }
        #endregion
    }
}
