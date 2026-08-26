using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class IslemVeHataLoglari : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "basarili",
                table: "logs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "hata_kodu",
                table: "logs",
                type: "character varying(13)",
                maxLength: 13,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ip_adresi",
                table: "logs",
                type: "character varying(45)",
                maxLength: 45,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "sure_ms",
                table: "logs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "yol",
                table: "logs",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            // Kolon eklenirken tüm eski kayıtlar "başarısız" görünürdü. Geçmiş
            // loglarda başarı ölçütü hata alanının boş olmasıdır.
            migrationBuilder.Sql("UPDATE logs SET basarili = (error IS NULL);");

            migrationBuilder.CreateTable(
                name: "hata_loglari",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    organizasyon_id = table.Column<int>(type: "integer", nullable: false),
                    kod = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: false),
                    olusma_tarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    tur = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    mesaj = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    kullanici_mesaji = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    ayrinti = table.Column<string>(type: "text", nullable: true),
                    durum_kodu = table.Column<int>(type: "integer", nullable: false),
                    yol = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    http_yontemi = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    kullanici = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ip_adresi = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    istek_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    cozuldu = table.Column<bool>(type: "boolean", nullable: false),
                    cozum_notu = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    cozulme_tarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    cozen_kullanici = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_hata_loglari", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_logs_hata_kodu",
                table: "logs",
                column: "hata_kodu");

            migrationBuilder.CreateIndex(
                name: "ix_logs_organizasyon_id_executing_time",
                table: "logs",
                columns: new[] { "organizasyon_id", "executing_time" });

            migrationBuilder.CreateIndex(
                name: "ix_hata_loglari_cozuldu",
                table: "hata_loglari",
                column: "cozuldu");

            migrationBuilder.CreateIndex(
                name: "ix_hata_loglari_kod",
                table: "hata_loglari",
                column: "kod",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_hata_loglari_organizasyon_id_olusma_tarihi",
                table: "hata_loglari",
                columns: new[] { "organizasyon_id", "olusma_tarihi" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "hata_loglari");

            migrationBuilder.DropIndex(
                name: "ix_logs_hata_kodu",
                table: "logs");

            migrationBuilder.DropIndex(
                name: "ix_logs_organizasyon_id_executing_time",
                table: "logs");

            migrationBuilder.DropColumn(
                name: "basarili",
                table: "logs");

            migrationBuilder.DropColumn(
                name: "hata_kodu",
                table: "logs");

            migrationBuilder.DropColumn(
                name: "ip_adresi",
                table: "logs");

            migrationBuilder.DropColumn(
                name: "sure_ms",
                table: "logs");

            migrationBuilder.DropColumn(
                name: "yol",
                table: "logs");
        }
    }
}
