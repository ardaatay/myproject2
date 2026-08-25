using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class CokKiracililik : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "organizasyon_id",
                table: "veritabanlari",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "organizasyon_id",
                table: "uygulamalar",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "organizasyon_id",
                table: "tasinabilir_cihaz_ve_ortamlar",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "organizasyon_id",
                table: "surecler",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "organizasyon_id",
                table: "personeller",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "organizasyon_id",
                table: "logs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "organizasyon_id",
                table: "kullanicilar",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "organizasyon_id",
                table: "kullanici_roller",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "organizasyon_id",
                table: "kullanici_birimler",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "organizasyon_id",
                table: "kriptografi_envanterleri",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "organizasyon_id",
                table: "iot_cihazlari",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "organizasyon_id",
                table: "guvenlik_modu",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "organizasyon_id",
                table: "fiziksel_mekanlar",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "organizasyon_id",
                table: "eposta_talepleri",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "organizasyon_id",
                table: "elektronik_bilgiler",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "organizasyon_id",
                table: "birimler",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "organizasyon_id",
                table: "basili_bilgiler",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "organizasyon_id",
                table: "agve_sistemler",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "organizasyonlar",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ad = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    kod = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    logo_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ayarlar = table.Column<string>(type: "text", nullable: true),
                    durum = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    deleted_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_organizasyonlar", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "organizasyonlar");

            migrationBuilder.DropColumn(
                name: "organizasyon_id",
                table: "veritabanlari");

            migrationBuilder.DropColumn(
                name: "organizasyon_id",
                table: "uygulamalar");

            migrationBuilder.DropColumn(
                name: "organizasyon_id",
                table: "tasinabilir_cihaz_ve_ortamlar");

            migrationBuilder.DropColumn(
                name: "organizasyon_id",
                table: "surecler");

            migrationBuilder.DropColumn(
                name: "organizasyon_id",
                table: "personeller");

            migrationBuilder.DropColumn(
                name: "organizasyon_id",
                table: "logs");

            migrationBuilder.DropColumn(
                name: "organizasyon_id",
                table: "kullanicilar");

            migrationBuilder.DropColumn(
                name: "organizasyon_id",
                table: "kullanici_roller");

            migrationBuilder.DropColumn(
                name: "organizasyon_id",
                table: "kullanici_birimler");

            migrationBuilder.DropColumn(
                name: "organizasyon_id",
                table: "kriptografi_envanterleri");

            migrationBuilder.DropColumn(
                name: "organizasyon_id",
                table: "iot_cihazlari");

            migrationBuilder.DropColumn(
                name: "organizasyon_id",
                table: "guvenlik_modu");

            migrationBuilder.DropColumn(
                name: "organizasyon_id",
                table: "fiziksel_mekanlar");

            migrationBuilder.DropColumn(
                name: "organizasyon_id",
                table: "eposta_talepleri");

            migrationBuilder.DropColumn(
                name: "organizasyon_id",
                table: "elektronik_bilgiler");

            migrationBuilder.DropColumn(
                name: "organizasyon_id",
                table: "birimler");

            migrationBuilder.DropColumn(
                name: "organizasyon_id",
                table: "basili_bilgiler");

            migrationBuilder.DropColumn(
                name: "organizasyon_id",
                table: "agve_sistemler");
        }
    }
}
