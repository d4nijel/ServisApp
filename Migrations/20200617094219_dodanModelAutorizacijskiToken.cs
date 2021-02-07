using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ServisApp.Migrations
{
    public partial class dodanModelAutorizacijskiToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AutorizacijskiTokeni",
                columns: table => new
                {
                    AutorizacijskiTokenId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Vrijednost = table.Column<string>(nullable: true),
                    VrijemeEvidentiranja = table.Column<DateTime>(nullable: false),
                    IpAdresa = table.Column<string>(nullable: true),
                    KorisnikId = table.Column<int>(nullable: true),
                    KlijentskiRacunId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutorizacijskiTokeni", x => x.AutorizacijskiTokenId);
                    table.ForeignKey(
                        name: "FK_AutorizacijskiTokeni_KlijentskiRacuni_KlijentskiRacunId",
                        column: x => x.KlijentskiRacunId,
                        principalTable: "KlijentskiRacuni",
                        principalColumn: "KlijentskiRacunId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AutorizacijskiTokeni_Korisnici_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnici",
                        principalColumn: "KorisnikId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AutorizacijskiTokeni_KlijentskiRacunId",
                table: "AutorizacijskiTokeni",
                column: "KlijentskiRacunId");

            migrationBuilder.CreateIndex(
                name: "IX_AutorizacijskiTokeni_KorisnikId",
                table: "AutorizacijskiTokeni",
                column: "KorisnikId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AutorizacijskiTokeni");
        }
    }
}
