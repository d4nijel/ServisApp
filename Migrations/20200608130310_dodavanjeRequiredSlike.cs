using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ServisApp.Migrations
{
    public partial class dodavanjeRequiredSlike : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "KorisnikSlika",
                table: "Korisnici",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "KlijentskiRacunSlika",
                table: "KlijentskiRacuni",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "KorisnikSlika",
                table: "Korisnici",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte[]));

            migrationBuilder.AlterColumn<byte[]>(
                name: "KlijentskiRacunSlika",
                table: "KlijentskiRacuni",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte[]));
        }
    }
}
