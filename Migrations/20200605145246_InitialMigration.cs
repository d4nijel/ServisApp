using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ServisApp.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClanoviServisa",
                columns: table => new
                {
                    ClanServisaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(maxLength: 50, nullable: false),
                    Prezime = table.Column<string>(maxLength: 50, nullable: false),
                    BrojMobitela = table.Column<string>(maxLength: 20, nullable: true),
                    Zanimanje = table.Column<string>(maxLength: 100, nullable: false),
                    ClanServisaStatus = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClanoviServisa", x => x.ClanServisaId);
                });

            migrationBuilder.CreateTable(
                name: "Kantoni",
                columns: table => new
                {
                    KantonId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(maxLength: 50, nullable: false),
                    SkraceniNaziv = table.Column<string>(maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kantoni", x => x.KantonId);
                });

            migrationBuilder.CreateTable(
                name: "Korisnici",
                columns: table => new
                {
                    KorisnikId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(maxLength: 50, nullable: false),
                    Prezime = table.Column<string>(maxLength: 50, nullable: false),
                    KorisnickoIme = table.Column<string>(maxLength: 100, nullable: false),
                    LozinkaHash = table.Column<string>(maxLength: 100, nullable: false),
                    LozinkaSalt = table.Column<string>(maxLength: 100, nullable: false),
                    KorisnikSlika = table.Column<byte[]>(nullable: true),
                    KorisnikStatus = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Korisnici", x => x.KorisnikId);
                });

            migrationBuilder.CreateTable(
                name: "NaziviIspitivanja",
                columns: table => new
                {
                    NazivIspitivanjaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(maxLength: 100, nullable: false),
                    Oznaka = table.Column<string>(maxLength: 50, nullable: false),
                    PeriodVazenja = table.Column<byte>(type: "tinyint", nullable: false),
                    NazivIspitivanjaStatus = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NaziviIspitivanja", x => x.NazivIspitivanjaId);
                });

            migrationBuilder.CreateTable(
                name: "Uloge",
                columns: table => new
                {
                    UlogaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(maxLength: 50, nullable: false),
                    Opis = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uloge", x => x.UlogaId);
                });

            migrationBuilder.CreateTable(
                name: "ZahtjeviKategorije",
                columns: table => new
                {
                    ZahtjevKategorijaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(maxLength: 50, nullable: false),
                    Opis = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZahtjeviKategorije", x => x.ZahtjevKategorijaId);
                });

            migrationBuilder.CreateTable(
                name: "ZahtjeviStatusi",
                columns: table => new
                {
                    ZahtjevStatusId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(maxLength: 50, nullable: false),
                    Opis = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZahtjeviStatusi", x => x.ZahtjevStatusId);
                });

            migrationBuilder.CreateTable(
                name: "Opcine",
                columns: table => new
                {
                    OpcinaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(maxLength: 100, nullable: false),
                    KantonId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Opcine", x => x.OpcinaId);
                    table.ForeignKey(
                        name: "FK_Opcine_Kantoni_KantonId",
                        column: x => x.KantonId,
                        principalTable: "Kantoni",
                        principalColumn: "KantonId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Dokumenti",
                columns: table => new
                {
                    DokumentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(maxLength: 400, nullable: false),
                    TipDokumenta = table.Column<string>(maxLength: 50, nullable: false),
                    DatumIzdavanja = table.Column<DateTime>(type: "date", nullable: false),
                    SluzbeniList = table.Column<string>(maxLength: 100, nullable: true),
                    DokumentPath = table.Column<string>(nullable: false),
                    DokumentStatus = table.Column<bool>(nullable: false),
                    KorisnikId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dokumenti", x => x.DokumentId);
                    table.ForeignKey(
                        name: "FK_Dokumenti_Korisnici_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnici",
                        principalColumn: "KorisnikId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Permisije",
                columns: table => new
                {
                    PermisijaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatumIzmjene = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PermisijaStatus = table.Column<bool>(nullable: false),
                    KorisnikId = table.Column<int>(nullable: false),
                    UlogaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permisije", x => x.PermisijaId);
                    table.ForeignKey(
                        name: "FK_Permisije_Korisnici_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnici",
                        principalColumn: "KorisnikId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Permisije_Uloge_UlogaId",
                        column: x => x.UlogaId,
                        principalTable: "Uloge",
                        principalColumn: "UlogaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Mjesta",
                columns: table => new
                {
                    MjestoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(maxLength: 100, nullable: false),
                    OpcinaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mjesta", x => x.MjestoId);
                    table.ForeignKey(
                        name: "FK_Mjesta_Opcine_OpcinaId",
                        column: x => x.OpcinaId,
                        principalTable: "Opcine",
                        principalColumn: "OpcinaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Klijenti",
                columns: table => new
                {
                    KlijentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(maxLength: 200, nullable: false),
                    SkraceniNaziv = table.Column<string>(maxLength: 100, nullable: false),
                    IdBroj = table.Column<string>(maxLength: 15, nullable: false),
                    Ulica = table.Column<string>(maxLength: 100, nullable: false),
                    KontaktOsoba = table.Column<string>(maxLength: 100, nullable: false),
                    KontaktBrojFiksni = table.Column<string>(maxLength: 20, nullable: false),
                    KontaktBrojMobitel = table.Column<string>(maxLength: 20, nullable: false),
                    KontaktEmail = table.Column<string>(maxLength: 100, nullable: false),
                    KlijentStatus = table.Column<bool>(nullable: false),
                    MjestoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Klijenti", x => x.KlijentId);
                    table.ForeignKey(
                        name: "FK_Klijenti_Mjesta_MjestoId",
                        column: x => x.MjestoId,
                        principalTable: "Mjesta",
                        principalColumn: "MjestoId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "KlijentskiRacuni",
                columns: table => new
                {
                    KlijentskiRacunId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(maxLength: 50, nullable: false),
                    Prezime = table.Column<string>(maxLength: 50, nullable: false),
                    DatumRegistracije = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(maxLength: 100, nullable: false),
                    KorisnickoIme = table.Column<string>(maxLength: 100, nullable: false),
                    LozinkaHash = table.Column<string>(maxLength: 100, nullable: false),
                    LozinkaSalt = table.Column<string>(maxLength: 100, nullable: false),
                    KlijentskiRacunSlika = table.Column<byte[]>(nullable: true),
                    EmailNotifikacija = table.Column<bool>(nullable: false),
                    BrojDanaPrijeIsteka = table.Column<byte>(type: "tinyint", nullable: false),
                    KlijentskiRacunStatus = table.Column<bool>(nullable: false),
                    KlijentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KlijentskiRacuni", x => x.KlijentskiRacunId);
                    table.ForeignKey(
                        name: "FK_KlijentskiRacuni_Klijenti_KlijentId",
                        column: x => x.KlijentId,
                        principalTable: "Klijenti",
                        principalColumn: "KlijentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Objekti",
                columns: table => new
                {
                    ObjekatId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(maxLength: 100, nullable: false),
                    Ulica = table.Column<string>(maxLength: 100, nullable: false),
                    KontaktOsoba = table.Column<string>(maxLength: 100, nullable: false),
                    KontaktBrojFiksni = table.Column<string>(maxLength: 20, nullable: false),
                    KontaktBrojMobitel = table.Column<string>(maxLength: 20, nullable: false),
                    KontaktEmail = table.Column<string>(maxLength: 100, nullable: false),
                    ObjekatStatus = table.Column<bool>(nullable: false),
                    KlijentId = table.Column<int>(nullable: false),
                    MjestoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Objekti", x => x.ObjekatId);
                    table.ForeignKey(
                        name: "FK_Objekti_Klijenti_KlijentId",
                        column: x => x.KlijentId,
                        principalTable: "Klijenti",
                        principalColumn: "KlijentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Objekti_Mjesta_MjestoId",
                        column: x => x.MjestoId,
                        principalTable: "Mjesta",
                        principalColumn: "MjestoId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ponude",
                columns: table => new
                {
                    PonudaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrojPonude = table.Column<string>(maxLength: 50, nullable: false),
                    DatumIzdavanja = table.Column<DateTime>(type: "date", nullable: false),
                    UkupanIznosBezPdv = table.Column<decimal>(type: "money", nullable: false),
                    UkupanIznosSaPdv = table.Column<decimal>(type: "money", nullable: false),
                    PonudaPath = table.Column<string>(nullable: false),
                    PonudaStatus = table.Column<bool>(nullable: false),
                    KlijentId = table.Column<int>(nullable: false),
                    KorisnikId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ponude", x => x.PonudaId);
                    table.ForeignKey(
                        name: "FK_Ponude_Klijenti_KlijentId",
                        column: x => x.KlijentId,
                        principalTable: "Klijenti",
                        principalColumn: "KlijentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ponude_Korisnici_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnici",
                        principalColumn: "KorisnikId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ugovori",
                columns: table => new
                {
                    UgovorId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrojUgovora = table.Column<string>(maxLength: 50, nullable: false),
                    Naziv = table.Column<string>(maxLength: 200, nullable: false),
                    DatumPotpisivanja = table.Column<DateTime>(type: "date", nullable: false),
                    DatumIsteka = table.Column<DateTime>(type: "date", nullable: false),
                    UgovorPath = table.Column<string>(nullable: false),
                    UgovorStatus = table.Column<bool>(nullable: false),
                    KlijentId = table.Column<int>(nullable: false),
                    KorisnikId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ugovori", x => x.UgovorId);
                    table.ForeignKey(
                        name: "FK_Ugovori_Klijenti_KlijentId",
                        column: x => x.KlijentId,
                        principalTable: "Klijenti",
                        principalColumn: "KlijentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ugovori_Korisnici_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnici",
                        principalColumn: "KorisnikId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Zahtjevi",
                columns: table => new
                {
                    ZahtjevId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naslov = table.Column<string>(maxLength: 100, nullable: false),
                    Opis = table.Column<string>(maxLength: 1000, nullable: false),
                    DatumKreiranja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KlijentskiRacunId = table.Column<int>(nullable: false),
                    ZahtjevKategorijaId = table.Column<int>(nullable: false),
                    ZahtjevStatusId = table.Column<int>(nullable: false),
                    KorisnikId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zahtjevi", x => x.ZahtjevId);
                    table.ForeignKey(
                        name: "FK_Zahtjevi_KlijentskiRacuni_KlijentskiRacunId",
                        column: x => x.KlijentskiRacunId,
                        principalTable: "KlijentskiRacuni",
                        principalColumn: "KlijentskiRacunId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Zahtjevi_Korisnici_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnici",
                        principalColumn: "KorisnikId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Zahtjevi_ZahtjeviKategorije_ZahtjevKategorijaId",
                        column: x => x.ZahtjevKategorijaId,
                        principalTable: "ZahtjeviKategorije",
                        principalColumn: "ZahtjevKategorijaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Zahtjevi_ZahtjeviStatusi_ZahtjevStatusId",
                        column: x => x.ZahtjevStatusId,
                        principalTable: "ZahtjeviStatusi",
                        principalColumn: "ZahtjevStatusId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RadniNalozi",
                columns: table => new
                {
                    RadniNalogId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrojRadnogNaloga = table.Column<string>(maxLength: 50, nullable: false),
                    DatumPocetkaRadova = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DatumZavrsetkaRadova = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RadniNalogPath = table.Column<string>(nullable: false),
                    ObjekatId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RadniNalozi", x => x.RadniNalogId);
                    table.ForeignKey(
                        name: "FK_RadniNalozi_Objekti_ObjekatId",
                        column: x => x.ObjekatId,
                        principalTable: "Objekti",
                        principalColumn: "ObjekatId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Poruke",
                columns: table => new
                {
                    PorukaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sadrzaj = table.Column<string>(maxLength: 1000, nullable: false),
                    DatumKreiranja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KlijentskiRacunId = table.Column<int>(nullable: true),
                    KorisnikId = table.Column<int>(nullable: true),
                    ZahtjevId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Poruke", x => x.PorukaId);
                    table.ForeignKey(
                        name: "FK_Poruke_KlijentskiRacuni_KlijentskiRacunId",
                        column: x => x.KlijentskiRacunId,
                        principalTable: "KlijentskiRacuni",
                        principalColumn: "KlijentskiRacunId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Poruke_Korisnici_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnici",
                        principalColumn: "KorisnikId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Poruke_Zahtjevi_ZahtjevId",
                        column: x => x.ZahtjevId,
                        principalTable: "Zahtjevi",
                        principalColumn: "ZahtjevId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ispitivanja",
                columns: table => new
                {
                    IspitivanjeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatumIspitivanja = table.Column<DateTime>(type: "date", nullable: false),
                    DatumNarednogIspitivanja = table.Column<DateTime>(type: "date", nullable: false),
                    TipIspitivanja = table.Column<string>(maxLength: 50, nullable: false),
                    Napomena = table.Column<string>(maxLength: 255, nullable: true),
                    RadniNalogId = table.Column<int>(nullable: false),
                    NazivIspitivanjaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ispitivanja", x => x.IspitivanjeId);
                    table.ForeignKey(
                        name: "FK_Ispitivanja_NaziviIspitivanja_NazivIspitivanjaId",
                        column: x => x.NazivIspitivanjaId,
                        principalTable: "NaziviIspitivanja",
                        principalColumn: "NazivIspitivanjaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ispitivanja_RadniNalozi_RadniNalogId",
                        column: x => x.RadniNalogId,
                        principalTable: "RadniNalozi",
                        principalColumn: "RadniNalogId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ObavljeniPoslovi",
                columns: table => new
                {
                    ClanServisaId = table.Column<int>(nullable: false),
                    RadniNalogId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObavljeniPoslovi", x => new { x.ClanServisaId, x.RadniNalogId });
                    table.ForeignKey(
                        name: "FK_ObavljeniPoslovi_ClanoviServisa_ClanServisaId",
                        column: x => x.ClanServisaId,
                        principalTable: "ClanoviServisa",
                        principalColumn: "ClanServisaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ObavljeniPoslovi_RadniNalozi_RadniNalogId",
                        column: x => x.RadniNalogId,
                        principalTable: "RadniNalozi",
                        principalColumn: "RadniNalogId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Izvjestaji",
                columns: table => new
                {
                    IzvjestajId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrojIzvjestaja = table.Column<string>(maxLength: 50, nullable: false),
                    DatumKreiranja = table.Column<DateTime>(type: "date", nullable: false),
                    IzvjestajPath = table.Column<string>(nullable: false),
                    IzvjestajStatus = table.Column<bool>(nullable: false),
                    IspitivanjeId = table.Column<int>(nullable: false),
                    KorisnikId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Izvjestaji", x => x.IzvjestajId);
                    table.ForeignKey(
                        name: "FK_Izvjestaji_Ispitivanja_IspitivanjeId",
                        column: x => x.IspitivanjeId,
                        principalTable: "Ispitivanja",
                        principalColumn: "IspitivanjeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Izvjestaji_Korisnici_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnici",
                        principalColumn: "KorisnikId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dokumenti_KorisnikId",
                table: "Dokumenti",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Ispitivanja_NazivIspitivanjaId",
                table: "Ispitivanja",
                column: "NazivIspitivanjaId");

            migrationBuilder.CreateIndex(
                name: "IX_Ispitivanja_RadniNalogId",
                table: "Ispitivanja",
                column: "RadniNalogId");

            migrationBuilder.CreateIndex(
                name: "IX_Izvjestaji_BrojIzvjestaja",
                table: "Izvjestaji",
                column: "BrojIzvjestaja",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Izvjestaji_IspitivanjeId",
                table: "Izvjestaji",
                column: "IspitivanjeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Izvjestaji_KorisnikId",
                table: "Izvjestaji",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Kantoni_Naziv",
                table: "Kantoni",
                column: "Naziv",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Kantoni_SkraceniNaziv",
                table: "Kantoni",
                column: "SkraceniNaziv",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Klijenti_IdBroj",
                table: "Klijenti",
                column: "IdBroj",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Klijenti_KontaktEmail",
                table: "Klijenti",
                column: "KontaktEmail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Klijenti_MjestoId",
                table: "Klijenti",
                column: "MjestoId");

            migrationBuilder.CreateIndex(
                name: "IX_KlijentskiRacuni_Email",
                table: "KlijentskiRacuni",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KlijentskiRacuni_KlijentId",
                table: "KlijentskiRacuni",
                column: "KlijentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KlijentskiRacuni_KorisnickoIme",
                table: "KlijentskiRacuni",
                column: "KorisnickoIme",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Korisnici_KorisnickoIme",
                table: "Korisnici",
                column: "KorisnickoIme",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Mjesta_OpcinaId",
                table: "Mjesta",
                column: "OpcinaId");

            migrationBuilder.CreateIndex(
                name: "IX_NaziviIspitivanja_Oznaka",
                table: "NaziviIspitivanja",
                column: "Oznaka",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ObavljeniPoslovi_RadniNalogId",
                table: "ObavljeniPoslovi",
                column: "RadniNalogId");

            migrationBuilder.CreateIndex(
                name: "IX_Objekti_KlijentId",
                table: "Objekti",
                column: "KlijentId");

            migrationBuilder.CreateIndex(
                name: "IX_Objekti_KontaktEmail",
                table: "Objekti",
                column: "KontaktEmail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Objekti_MjestoId",
                table: "Objekti",
                column: "MjestoId");

            migrationBuilder.CreateIndex(
                name: "IX_Opcine_KantonId",
                table: "Opcine",
                column: "KantonId");

            migrationBuilder.CreateIndex(
                name: "IX_Permisije_KorisnikId",
                table: "Permisije",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Permisije_UlogaId",
                table: "Permisije",
                column: "UlogaId");

            migrationBuilder.CreateIndex(
                name: "IX_Ponude_BrojPonude",
                table: "Ponude",
                column: "BrojPonude",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ponude_KlijentId",
                table: "Ponude",
                column: "KlijentId");

            migrationBuilder.CreateIndex(
                name: "IX_Ponude_KorisnikId",
                table: "Ponude",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Poruke_KlijentskiRacunId",
                table: "Poruke",
                column: "KlijentskiRacunId");

            migrationBuilder.CreateIndex(
                name: "IX_Poruke_KorisnikId",
                table: "Poruke",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Poruke_ZahtjevId",
                table: "Poruke",
                column: "ZahtjevId");

            migrationBuilder.CreateIndex(
                name: "IX_RadniNalozi_BrojRadnogNaloga",
                table: "RadniNalozi",
                column: "BrojRadnogNaloga",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RadniNalozi_ObjekatId",
                table: "RadniNalozi",
                column: "ObjekatId");

            migrationBuilder.CreateIndex(
                name: "IX_Ugovori_BrojUgovora",
                table: "Ugovori",
                column: "BrojUgovora",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ugovori_KlijentId",
                table: "Ugovori",
                column: "KlijentId");

            migrationBuilder.CreateIndex(
                name: "IX_Ugovori_KorisnikId",
                table: "Ugovori",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Uloge_Naziv",
                table: "Uloge",
                column: "Naziv",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Zahtjevi_KlijentskiRacunId",
                table: "Zahtjevi",
                column: "KlijentskiRacunId");

            migrationBuilder.CreateIndex(
                name: "IX_Zahtjevi_KorisnikId",
                table: "Zahtjevi",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Zahtjevi_ZahtjevKategorijaId",
                table: "Zahtjevi",
                column: "ZahtjevKategorijaId");

            migrationBuilder.CreateIndex(
                name: "IX_Zahtjevi_ZahtjevStatusId",
                table: "Zahtjevi",
                column: "ZahtjevStatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dokumenti");

            migrationBuilder.DropTable(
                name: "Izvjestaji");

            migrationBuilder.DropTable(
                name: "ObavljeniPoslovi");

            migrationBuilder.DropTable(
                name: "Permisije");

            migrationBuilder.DropTable(
                name: "Ponude");

            migrationBuilder.DropTable(
                name: "Poruke");

            migrationBuilder.DropTable(
                name: "Ugovori");

            migrationBuilder.DropTable(
                name: "Ispitivanja");

            migrationBuilder.DropTable(
                name: "ClanoviServisa");

            migrationBuilder.DropTable(
                name: "Uloge");

            migrationBuilder.DropTable(
                name: "Zahtjevi");

            migrationBuilder.DropTable(
                name: "NaziviIspitivanja");

            migrationBuilder.DropTable(
                name: "RadniNalozi");

            migrationBuilder.DropTable(
                name: "KlijentskiRacuni");

            migrationBuilder.DropTable(
                name: "Korisnici");

            migrationBuilder.DropTable(
                name: "ZahtjeviKategorije");

            migrationBuilder.DropTable(
                name: "ZahtjeviStatusi");

            migrationBuilder.DropTable(
                name: "Objekti");

            migrationBuilder.DropTable(
                name: "Klijenti");

            migrationBuilder.DropTable(
                name: "Mjesta");

            migrationBuilder.DropTable(
                name: "Opcine");

            migrationBuilder.DropTable(
                name: "Kantoni");
        }
    }
}
