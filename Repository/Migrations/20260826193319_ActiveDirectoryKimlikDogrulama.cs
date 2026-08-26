using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class ActiveDirectoryKimlikDogrulama : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "active_directory_kullanici_adi",
                table: "kullanicilar",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "giris_yontemi",
                table: "kullanicilar",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "active_directory_ayarlari",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    organizasyon_id = table.Column<int>(type: "integer", nullable: false),
                    aktif = table.Column<bool>(type: "boolean", nullable: false),
                    sunucu = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    port = table.Column<int>(type: "integer", nullable: false),
                    ssl_kullan = table.Column<bool>(type: "boolean", nullable: false),
                    start_tls_kullan = table.Column<bool>(type: "boolean", nullable: false),
                    sertifika_dogrulamasi_atla = table.Column<bool>(type: "boolean", nullable: false),
                    alan_adi = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    net_bios_adi = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    taban_dn = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    servis_hesabi = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    servis_hesabi_sifresi_korunmus = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    kullanici_arama_filtresi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    kullanici_adi_ozniteligi = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ad_soyad_ozniteligi = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    eposta_ozniteligi = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    zorunlu_grup_dn = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    zaman_asimi_sn = table.Column<int>(type: "integer", nullable: false),
                    profil_bilgilerini_guncelle = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_active_directory_ayarlari", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_active_directory_ayarlari_organizasyon_id",
                table: "active_directory_ayarlari",
                column: "organizasyon_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "active_directory_ayarlari");

            migrationBuilder.DropColumn(
                name: "active_directory_kullanici_adi",
                table: "kullanicilar");

            migrationBuilder.DropColumn(
                name: "giris_yontemi",
                table: "kullanicilar");
        }
    }
}
