using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class YerelKimlikDogrulama : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "username",
                table: "kullanicilar",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "birim_ad",
                table: "kullanicilar",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "ad_soyad",
                table: "kullanicilar",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "basarisiz_giris_sayisi",
                table: "kullanicilar",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "eposta",
                table: "kullanicilar",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "kilit_bitis_tarihi",
                table: "kullanicilar",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "password_hash",
                table: "kullanicilar",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "security_stamp",
                table: "kullanicilar",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "sifre_degistirmeli_mi",
                table: "kullanicilar",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "son_giris_tarihi",
                table: "kullanicilar",
                type: "timestamp without time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ad_soyad",
                table: "kullanicilar");

            migrationBuilder.DropColumn(
                name: "basarisiz_giris_sayisi",
                table: "kullanicilar");

            migrationBuilder.DropColumn(
                name: "eposta",
                table: "kullanicilar");

            migrationBuilder.DropColumn(
                name: "kilit_bitis_tarihi",
                table: "kullanicilar");

            migrationBuilder.DropColumn(
                name: "password_hash",
                table: "kullanicilar");

            migrationBuilder.DropColumn(
                name: "security_stamp",
                table: "kullanicilar");

            migrationBuilder.DropColumn(
                name: "sifre_degistirmeli_mi",
                table: "kullanicilar");

            migrationBuilder.DropColumn(
                name: "son_giris_tarihi",
                table: "kullanicilar");

            migrationBuilder.AlterColumn<string>(
                name: "username",
                table: "kullanicilar",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "birim_ad",
                table: "kullanicilar",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);
        }
    }
}
