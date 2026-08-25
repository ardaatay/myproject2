using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "anahtar_sorumlulari",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ad = table.Column<string>(type: "text", nullable: false),
                    durum = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_anahtar_sorumlulari", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "bagimli_varliklar",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ad = table.Column<string>(type: "text", nullable: false),
                    durum = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bagimli_varliklar", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "bilgi_siniflari",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ad = table.Column<string>(type: "text", nullable: false),
                    durum = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bilgi_siniflari", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "birimler",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ust_id = table.Column<int>(type: "integer", nullable: true),
                    ad = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    kod = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    yol = table.Column<string>(type: "character varying(900)", maxLength: 900, nullable: false),
                    seviye = table.Column<int>(type: "integer", nullable: false),
                    sira = table.Column<int>(type: "integer", nullable: false),
                    durum = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    deleted_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_birimler", x => x.id);
                    table.ForeignKey(
                        name: "fk_birimler_birimler_ust_id",
                        column: x => x.ust_id,
                        principalTable: "birimler",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "butunlukler",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ad = table.Column<string>(type: "text", nullable: false),
                    durum = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_butunlukler", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "destek_durumlari",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ad = table.Column<string>(type: "text", nullable: false),
                    durum = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_destek_durumlari", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "durumlar",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ad = table.Column<string>(type: "text", nullable: false),
                    durum = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_durumlar", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "erisilebilirlikler",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ad = table.Column<string>(type: "text", nullable: false),
                    durum = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_erisilebilirlikler", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "etkilenen_kisi_sayilari",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ad = table.Column<string>(type: "text", nullable: false),
                    durum = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_etkilenen_kisi_sayilari", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "gizlilikler",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ad = table.Column<string>(type: "text", nullable: false),
                    durum = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_gizlilikler", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "guvenlik_modu",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    durum = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_guvenlik_modu", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "kategoriler",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ad = table.Column<string>(type: "text", nullable: false),
                    ust_id = table.Column<int>(type: "integer", nullable: true),
                    durum = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_kategoriler", x => x.id);
                    table.ForeignKey(
                        name: "fk_kategoriler_kategoriler_ust_id",
                        column: x => x.ust_id,
                        principalTable: "kategoriler",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "konumlar",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ad = table.Column<string>(type: "text", nullable: false),
                    durum = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_konumlar", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "kriptoloji_turleri",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ad = table.Column<string>(type: "text", nullable: false),
                    durum = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_kriptoloji_turleri", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "kullanicilar",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "text", nullable: false),
                    birim_id = table.Column<int>(type: "integer", nullable: false),
                    birim_ad = table.Column<string>(type: "text", nullable: false),
                    durum = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_kullanicilar", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "kullanim_seviyeleri",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ad = table.Column<string>(type: "text", nullable: false),
                    durum = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_kullanim_seviyeleri", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "kurumlar",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    deleted_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ad = table.Column<string>(type: "text", nullable: false),
                    durum = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_kurumlar", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "kurumsal_sonuclar",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ad = table.Column<string>(type: "text", nullable: false),
                    durum = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_kurumsal_sonuclar", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "lisans_takip_sorumlulari",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ad = table.Column<string>(type: "text", nullable: false),
                    durum = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_lisans_takip_sorumlulari", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "logs",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    method_name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    class_name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    parameters = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    executing_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    return_value = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    error = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    username = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_logs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roller",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ad = table.Column<string>(type: "text", nullable: false),
                    durum = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roller", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sektorel_etkiler",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ad = table.Column<string>(type: "text", nullable: false),
                    durum = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sektorel_etkiler", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "toplumsal_sonuclar",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ad = table.Column<string>(type: "text", nullable: false),
                    durum = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_toplumsal_sonuclar", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "yedekleme_sorumlulari",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ad = table.Column<string>(type: "text", nullable: false),
                    durum = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_yedekleme_sorumlulari", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "yedekleme_tipleri",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ad = table.Column<string>(type: "text", nullable: false),
                    durum = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_yedekleme_tipleri", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "kullanici_birimler",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    kullanici_id = table.Column<int>(type: "integer", nullable: false),
                    birim_id = table.Column<int>(type: "integer", nullable: false),
                    birim_ad = table.Column<string>(type: "text", nullable: true),
                    durum = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_kullanici_birimler", x => x.id);
                    table.ForeignKey(
                        name: "fk_kullanici_birimler_kullanicilar_kullanici_id",
                        column: x => x.kullanici_id,
                        principalTable: "kullanicilar",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "kriptografi_envanterleri",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    varlik_sahibi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    varlik_sahibi_id = table.Column<int>(type: "integer", nullable: false),
                    varlik_sahibi_alt_departman = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    varlik_sahibi_alt_departman_id = table.Column<int>(type: "integer", nullable: true),
                    varlik_adi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    uretim_yeri = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    kullanim_amaci = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    olusturma_tarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    kullanim_suresi = table.Column<int>(type: "integer", nullable: true),
                    kullanim_suresi_tip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    anahtar_sorumlusu_id = table.Column<int>(type: "integer", nullable: true),
                    anahtar_saklama_alani = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    destek_alinan_tedarikci = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    donanim_yazilim = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    algoritma = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ortak_kriterler = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    kullanim_seviyesi_id = table.Column<int>(type: "integer", nullable: true),
                    kullanim_kabiliyetleri = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    notlar = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    aktif = table.Column<bool>(type: "boolean", nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    deleted_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    silinsin_mi = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_kriptografi_envanterleri", x => x.id);
                    table.ForeignKey(
                        name: "fk_kriptografi_envanterleri_anahtar_sorumlulari_anahtar_soruml",
                        column: x => x.anahtar_sorumlusu_id,
                        principalTable: "anahtar_sorumlulari",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_kriptografi_envanterleri_kullanim_seviyeleri_kullanim_seviy",
                        column: x => x.kullanim_seviyesi_id,
                        principalTable: "kullanim_seviyeleri",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "eposta_talepleri",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    kurum_id = table.Column<int>(type: "integer", nullable: false),
                    ucuncu_taraf = table.Column<string>(type: "text", nullable: true),
                    talep_edilen = table.Column<string>(type: "text", nullable: true),
                    talep_eden = table.Column<string>(type: "text", nullable: true),
                    talep_nedeni = table.Column<string>(type: "text", nullable: true),
                    talep_suresi = table.Column<string>(type: "text", nullable: true),
                    dosya_yolu = table.Column<string>(type: "text", nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    deleted_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ad = table.Column<string>(type: "text", nullable: false),
                    durum = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_eposta_talepleri", x => x.id);
                    table.ForeignKey(
                        name: "fk_eposta_talepleri_kurumlar_kurum_id",
                        column: x => x.kurum_id,
                        principalTable: "kurumlar",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "kullanici_roller",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    kullanici_id = table.Column<int>(type: "integer", nullable: false),
                    rol_id = table.Column<int>(type: "integer", nullable: false),
                    durum = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_kullanici_roller", x => x.id);
                    table.ForeignKey(
                        name: "fk_kullanici_roller_kullanicilar_kullanici_id",
                        column: x => x.kullanici_id,
                        principalTable: "kullanicilar",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_kullanici_roller_roller_rol_id",
                        column: x => x.rol_id,
                        principalTable: "roller",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "fiziksel_mekanlar",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    kategori_id = table.Column<int>(type: "integer", nullable: false),
                    alt_kategori_id = table.Column<int>(type: "integer", nullable: false),
                    varlik_adi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    kullanim_amaci = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    miktar = table.Column<int>(type: "integer", nullable: false),
                    durum_id = table.Column<int>(type: "integer", nullable: false),
                    konum = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    konum_id = table.Column<int>(type: "integer", nullable: false),
                    varlik_sahibi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    varlik_sahibi_id = table.Column<int>(type: "integer", nullable: true),
                    varlik_sahibi_alt_departman = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    varlik_sahibi_alt_departman_id = table.Column<int>(type: "integer", nullable: true),
                    operasyonel_sahibi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    operasyonel_sahibi_id = table.Column<int>(type: "integer", nullable: true),
                    bilgi_sinifi_id = table.Column<int>(type: "integer", nullable: true),
                    gizlilik_id = table.Column<int>(type: "integer", nullable: true),
                    butunluk_id = table.Column<int>(type: "integer", nullable: true),
                    erisilebilirlik_id = table.Column<int>(type: "integer", nullable: true),
                    etkilenen_kisi_sayisi_id = table.Column<int>(type: "integer", nullable: true),
                    toplumsal_sonuc_id = table.Column<int>(type: "integer", nullable: true),
                    kurumsal_sonuc_id = table.Column<int>(type: "integer", nullable: true),
                    sektorel_etki_id = table.Column<int>(type: "integer", nullable: true),
                    bagimli_varlik_id = table.Column<int>(type: "integer", nullable: true),
                    rpo = table.Column<int>(type: "integer", nullable: true),
                    rpo_tip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    rto = table.Column<int>(type: "integer", nullable: true),
                    rto_tip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    mtpd = table.Column<int>(type: "integer", nullable: true),
                    mtpd_tip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    kurtarma_planlari = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    kisisel_veri_barindirma = table.Column<bool>(type: "boolean", nullable: true),
                    basili_bilgi = table.Column<bool>(type: "boolean", nullable: true),
                    notlar = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    envantere_giris_tarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    envanter_guncelleme_tarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    envanterden_cikis_tarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    silinsin_mi = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_fiziksel_mekanlar", x => x.id);
                    table.ForeignKey(
                        name: "fk_fiziksel_mekanlar_bagimli_varliklar_bagimli_varlik_id",
                        column: x => x.bagimli_varlik_id,
                        principalTable: "bagimli_varliklar",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_fiziksel_mekanlar_bilgi_siniflari_bilgi_sinifi_id",
                        column: x => x.bilgi_sinifi_id,
                        principalTable: "bilgi_siniflari",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_fiziksel_mekanlar_butunlukler_butunluk_id",
                        column: x => x.butunluk_id,
                        principalTable: "butunlukler",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_fiziksel_mekanlar_durumlar_durum_id",
                        column: x => x.durum_id,
                        principalTable: "durumlar",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_fiziksel_mekanlar_erisilebilirlikler_erisilebilirlik_id",
                        column: x => x.erisilebilirlik_id,
                        principalTable: "erisilebilirlikler",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_fiziksel_mekanlar_etkilenen_kisi_sayilari_etkilenen_kisi_sa",
                        column: x => x.etkilenen_kisi_sayisi_id,
                        principalTable: "etkilenen_kisi_sayilari",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_fiziksel_mekanlar_gizlilikler_gizlilik_id",
                        column: x => x.gizlilik_id,
                        principalTable: "gizlilikler",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_fiziksel_mekanlar_kategoriler_alt_kategori_id",
                        column: x => x.alt_kategori_id,
                        principalTable: "kategoriler",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_fiziksel_mekanlar_kategoriler_kategori_id",
                        column: x => x.kategori_id,
                        principalTable: "kategoriler",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_fiziksel_mekanlar_kurumsal_sonuclar_kurumsal_sonuc_id",
                        column: x => x.kurumsal_sonuc_id,
                        principalTable: "kurumsal_sonuclar",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_fiziksel_mekanlar_sektorel_etkiler_sektorel_etki_id",
                        column: x => x.sektorel_etki_id,
                        principalTable: "sektorel_etkiler",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_fiziksel_mekanlar_toplumsal_sonuclar_toplumsal_sonuc_id",
                        column: x => x.toplumsal_sonuc_id,
                        principalTable: "toplumsal_sonuclar",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "personeller",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    kategori_id = table.Column<int>(type: "integer", nullable: false),
                    alt_kategori_id = table.Column<int>(type: "integer", nullable: false),
                    varlik_adi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    kullanim_amaci = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    miktar = table.Column<int>(type: "integer", nullable: false),
                    durum_id = table.Column<int>(type: "integer", nullable: false),
                    konum = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    konum_id = table.Column<int>(type: "integer", nullable: false),
                    varlik_sahibi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    varlik_sahibi_id = table.Column<int>(type: "integer", nullable: true),
                    varlik_sahibi_alt_departman = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    varlik_sahibi_alt_departman_id = table.Column<int>(type: "integer", nullable: true),
                    operasyonel_sahibi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    operasyonel_sahibi_id = table.Column<int>(type: "integer", nullable: true),
                    bilgi_sinifi_id = table.Column<int>(type: "integer", nullable: true),
                    gizlilik_id = table.Column<int>(type: "integer", nullable: true),
                    butunluk_id = table.Column<int>(type: "integer", nullable: true),
                    erisilebilirlik_id = table.Column<int>(type: "integer", nullable: true),
                    etkilenen_kisi_sayisi_id = table.Column<int>(type: "integer", nullable: true),
                    toplumsal_sonuc_id = table.Column<int>(type: "integer", nullable: true),
                    kurumsal_sonuc_id = table.Column<int>(type: "integer", nullable: true),
                    sektorel_etki_id = table.Column<int>(type: "integer", nullable: true),
                    bagimli_varlik_id = table.Column<int>(type: "integer", nullable: true),
                    rpo = table.Column<int>(type: "integer", nullable: true),
                    rpo_tip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    mtpd = table.Column<int>(type: "integer", nullable: true),
                    mtpd_tip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    kurtarma_planlari = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    vekalet_edilme_durumu = table.Column<bool>(type: "boolean", nullable: true),
                    notlar = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    envantere_giris_tarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    envanter_guncelleme_tarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    envanterden_cikis_tarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    silinsin_mi = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_personeller", x => x.id);
                    table.ForeignKey(
                        name: "fk_personeller_bagimli_varliklar_bagimli_varlik_id",
                        column: x => x.bagimli_varlik_id,
                        principalTable: "bagimli_varliklar",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_personeller_bilgi_siniflari_bilgi_sinifi_id",
                        column: x => x.bilgi_sinifi_id,
                        principalTable: "bilgi_siniflari",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_personeller_butunlukler_butunluk_id",
                        column: x => x.butunluk_id,
                        principalTable: "butunlukler",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_personeller_durumlar_durum_id",
                        column: x => x.durum_id,
                        principalTable: "durumlar",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_personeller_erisilebilirlikler_erisilebilirlik_id",
                        column: x => x.erisilebilirlik_id,
                        principalTable: "erisilebilirlikler",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_personeller_etkilenen_kisi_sayilari_etkilenen_kisi_sayisi_id",
                        column: x => x.etkilenen_kisi_sayisi_id,
                        principalTable: "etkilenen_kisi_sayilari",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_personeller_gizlilikler_gizlilik_id",
                        column: x => x.gizlilik_id,
                        principalTable: "gizlilikler",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_personeller_kategoriler_alt_kategori_id",
                        column: x => x.alt_kategori_id,
                        principalTable: "kategoriler",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_personeller_kategoriler_kategori_id",
                        column: x => x.kategori_id,
                        principalTable: "kategoriler",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_personeller_kurumsal_sonuclar_kurumsal_sonuc_id",
                        column: x => x.kurumsal_sonuc_id,
                        principalTable: "kurumsal_sonuclar",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_personeller_sektorel_etkiler_sektorel_etki_id",
                        column: x => x.sektorel_etki_id,
                        principalTable: "sektorel_etkiler",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_personeller_toplumsal_sonuclar_toplumsal_sonuc_id",
                        column: x => x.toplumsal_sonuc_id,
                        principalTable: "toplumsal_sonuclar",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "surecler",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    kategori_id = table.Column<int>(type: "integer", nullable: false),
                    alt_kategori_id = table.Column<int>(type: "integer", nullable: false),
                    varlik_adi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    kullanim_amaci = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    miktar = table.Column<int>(type: "integer", nullable: false),
                    durum_id = table.Column<int>(type: "integer", nullable: false),
                    konum = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    konum_id = table.Column<int>(type: "integer", nullable: false),
                    varlik_sahibi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    varlik_sahibi_id = table.Column<int>(type: "integer", nullable: true),
                    varlik_sahibi_alt_departman = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    varlik_sahibi_alt_departman_id = table.Column<int>(type: "integer", nullable: true),
                    operasyonel_sahibi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    operasyonel_sahibi_id = table.Column<int>(type: "integer", nullable: true),
                    bilgi_sinifi_id = table.Column<int>(type: "integer", nullable: true),
                    gizlilik_id = table.Column<int>(type: "integer", nullable: true),
                    butunluk_id = table.Column<int>(type: "integer", nullable: true),
                    erisilebilirlik_id = table.Column<int>(type: "integer", nullable: true),
                    etkilenen_kisi_sayisi_id = table.Column<int>(type: "integer", nullable: true),
                    toplumsal_sonuc_id = table.Column<int>(type: "integer", nullable: true),
                    kurumsal_sonuc_id = table.Column<int>(type: "integer", nullable: true),
                    sektorel_etki_id = table.Column<int>(type: "integer", nullable: true),
                    bagimli_varlik_id = table.Column<int>(type: "integer", nullable: true),
                    rpo = table.Column<int>(type: "integer", nullable: true),
                    rpo_tip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    rto = table.Column<int>(type: "integer", nullable: true),
                    rto_tip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    mtpd = table.Column<int>(type: "integer", nullable: true),
                    mtpd_tip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    kurtarma_planlari = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    notlar = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    envantere_giris_tarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    envanter_guncelleme_tarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    envanterden_cikis_tarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    silinsin_mi = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_surecler", x => x.id);
                    table.ForeignKey(
                        name: "fk_surecler_bagimli_varliklar_bagimli_varlik_id",
                        column: x => x.bagimli_varlik_id,
                        principalTable: "bagimli_varliklar",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_surecler_bilgi_siniflari_bilgi_sinifi_id",
                        column: x => x.bilgi_sinifi_id,
                        principalTable: "bilgi_siniflari",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_surecler_butunlukler_butunluk_id",
                        column: x => x.butunluk_id,
                        principalTable: "butunlukler",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_surecler_durumlar_durum_id",
                        column: x => x.durum_id,
                        principalTable: "durumlar",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_surecler_erisilebilirlikler_erisilebilirlik_id",
                        column: x => x.erisilebilirlik_id,
                        principalTable: "erisilebilirlikler",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_surecler_etkilenen_kisi_sayilari_etkilenen_kisi_sayisi_id",
                        column: x => x.etkilenen_kisi_sayisi_id,
                        principalTable: "etkilenen_kisi_sayilari",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_surecler_gizlilikler_gizlilik_id",
                        column: x => x.gizlilik_id,
                        principalTable: "gizlilikler",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_surecler_kategoriler_alt_kategori_id",
                        column: x => x.alt_kategori_id,
                        principalTable: "kategoriler",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_surecler_kategoriler_kategori_id",
                        column: x => x.kategori_id,
                        principalTable: "kategoriler",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_surecler_kurumsal_sonuclar_kurumsal_sonuc_id",
                        column: x => x.kurumsal_sonuc_id,
                        principalTable: "kurumsal_sonuclar",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_surecler_sektorel_etkiler_sektorel_etki_id",
                        column: x => x.sektorel_etki_id,
                        principalTable: "sektorel_etkiler",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_surecler_toplumsal_sonuclar_toplumsal_sonuc_id",
                        column: x => x.toplumsal_sonuc_id,
                        principalTable: "toplumsal_sonuclar",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "agve_sistemler",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    kategori_id = table.Column<int>(type: "integer", nullable: false),
                    alt_kategori_id = table.Column<int>(type: "integer", nullable: false),
                    varlik_adi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    kullanim_amaci = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    miktar = table.Column<int>(type: "integer", nullable: true),
                    durum_id = table.Column<int>(type: "integer", nullable: false),
                    konum = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    konum_id = table.Column<int>(type: "integer", nullable: false),
                    varlik_sahibi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    varlik_sahibi_id = table.Column<int>(type: "integer", nullable: true),
                    varlik_sahibi_alt_departman = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    varlik_sahibi_alt_departman_id = table.Column<int>(type: "integer", nullable: true),
                    operasyonel_sahibi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    operasyonel_sahibi_id = table.Column<int>(type: "integer", nullable: true),
                    bilgi_sinifi_id = table.Column<int>(type: "integer", nullable: true),
                    gizlilik_id = table.Column<int>(type: "integer", nullable: true),
                    butunluk_id = table.Column<int>(type: "integer", nullable: true),
                    erisilebilirlik_id = table.Column<int>(type: "integer", nullable: true),
                    etkilenen_kisi_sayisi_id = table.Column<int>(type: "integer", nullable: true),
                    toplumsal_sonuc_id = table.Column<int>(type: "integer", nullable: true),
                    kurumsal_sonuc_id = table.Column<int>(type: "integer", nullable: true),
                    sektorel_etki_id = table.Column<int>(type: "integer", nullable: true),
                    bagimli_varlik_id = table.Column<int>(type: "integer", nullable: true),
                    rpo = table.Column<int>(type: "integer", nullable: true),
                    rpo_tip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    rto = table.Column<int>(type: "integer", nullable: true),
                    rto_tip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    mtpd = table.Column<int>(type: "integer", nullable: true),
                    mtpd_tip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    kurtarma_planlari = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    yedekleme_tipi_id = table.Column<int>(type: "integer", nullable: true),
                    yedekleme_turu = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    yedekleme_sikligi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    yedeklerin_saklama_suresi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    yedekleme_alani = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    yedekten_donus_plani = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    yedekleme_sorumlusu_id = table.Column<int>(type: "integer", nullable: true),
                    kriptoloji = table.Column<bool>(type: "boolean", nullable: true),
                    kriptoloji_turu_id = table.Column<int>(type: "integer", nullable: true),
                    kullanilan_kriptoloji = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    anahtar_sorumlusu_id = table.Column<int>(type: "integer", nullable: true),
                    kisisel_veri_barindirma = table.Column<bool>(type: "boolean", nullable: true),
                    anlik_mesajlasma_kullanimi = table.Column<bool>(type: "boolean", nullable: true),
                    bulut_bilisim = table.Column<bool>(type: "boolean", nullable: true),
                    yeni_gelismelerve_tedarik = table.Column<bool>(type: "boolean", nullable: true),
                    kritik_altyapi_sistemi = table.Column<bool>(type: "boolean", nullable: true),
                    ip_adresi = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    isletim_sistemi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    lisans_takip_sorumlusu_id = table.Column<int>(type: "integer", nullable: true),
                    marka_model = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    seri_numarasi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    zimmet_sahibi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    notlar = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    envantere_giris_tarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    envanter_guncelleme_tarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    envanterden_cikis_tarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    silinsin_mi = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_agve_sistemler", x => x.id);
                    table.ForeignKey(
                        name: "fk_agve_sistemler_anahtar_sorumlulari_anahtar_sorumlusu_id",
                        column: x => x.anahtar_sorumlusu_id,
                        principalTable: "anahtar_sorumlulari",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_agve_sistemler_bagimli_varliklar_bagimli_varlik_id",
                        column: x => x.bagimli_varlik_id,
                        principalTable: "bagimli_varliklar",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_agve_sistemler_bilgi_siniflari_bilgi_sinifi_id",
                        column: x => x.bilgi_sinifi_id,
                        principalTable: "bilgi_siniflari",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_agve_sistemler_butunlukler_butunluk_id",
                        column: x => x.butunluk_id,
                        principalTable: "butunlukler",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_agve_sistemler_durumlar_durum_id",
                        column: x => x.durum_id,
                        principalTable: "durumlar",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_agve_sistemler_erisilebilirlikler_erisilebilirlik_id",
                        column: x => x.erisilebilirlik_id,
                        principalTable: "erisilebilirlikler",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_agve_sistemler_etkilenen_kisi_sayilari_etkilenen_kisi_sayis",
                        column: x => x.etkilenen_kisi_sayisi_id,
                        principalTable: "etkilenen_kisi_sayilari",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_agve_sistemler_gizlilikler_gizlilik_id",
                        column: x => x.gizlilik_id,
                        principalTable: "gizlilikler",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_agve_sistemler_kategoriler_alt_kategori_id",
                        column: x => x.alt_kategori_id,
                        principalTable: "kategoriler",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_agve_sistemler_kategoriler_kategori_id",
                        column: x => x.kategori_id,
                        principalTable: "kategoriler",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_agve_sistemler_kriptoloji_turleri_kriptoloji_turu_id",
                        column: x => x.kriptoloji_turu_id,
                        principalTable: "kriptoloji_turleri",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_agve_sistemler_kurumsal_sonuclar_kurumsal_sonuc_id",
                        column: x => x.kurumsal_sonuc_id,
                        principalTable: "kurumsal_sonuclar",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_agve_sistemler_lisans_takip_sorumlulari_lisans_takip_soruml",
                        column: x => x.lisans_takip_sorumlusu_id,
                        principalTable: "lisans_takip_sorumlulari",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_agve_sistemler_sektorel_etkiler_sektorel_etki_id",
                        column: x => x.sektorel_etki_id,
                        principalTable: "sektorel_etkiler",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_agve_sistemler_toplumsal_sonuclar_toplumsal_sonuc_id",
                        column: x => x.toplumsal_sonuc_id,
                        principalTable: "toplumsal_sonuclar",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_agve_sistemler_yedekleme_sorumlulari_yedekleme_sorumlusu_id",
                        column: x => x.yedekleme_sorumlusu_id,
                        principalTable: "yedekleme_sorumlulari",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_agve_sistemler_yedekleme_tipleri_yedekleme_tipi_id",
                        column: x => x.yedekleme_tipi_id,
                        principalTable: "yedekleme_tipleri",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "basili_bilgiler",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    kategori_id = table.Column<int>(type: "integer", nullable: false),
                    alt_kategori_id = table.Column<int>(type: "integer", nullable: false),
                    varlik_adi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    kullanim_amaci = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    miktar = table.Column<int>(type: "integer", nullable: false),
                    durum_id = table.Column<int>(type: "integer", nullable: false),
                    konum = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    konum_id = table.Column<int>(type: "integer", nullable: false),
                    varlik_sahibi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    varlik_sahibi_id = table.Column<int>(type: "integer", nullable: true),
                    varlik_sahibi_alt_departman = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    varlik_sahibi_alt_departman_id = table.Column<int>(type: "integer", nullable: true),
                    operasyonel_sahibi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    operasyonel_sahibi_id = table.Column<int>(type: "integer", nullable: true),
                    bilgi_sinifi_id = table.Column<int>(type: "integer", nullable: true),
                    gizlilik_id = table.Column<int>(type: "integer", nullable: true),
                    butunluk_id = table.Column<int>(type: "integer", nullable: true),
                    erisilebilirlik_id = table.Column<int>(type: "integer", nullable: true),
                    etkilenen_kisi_sayisi_id = table.Column<int>(type: "integer", nullable: true),
                    toplumsal_sonuc_id = table.Column<int>(type: "integer", nullable: true),
                    kurumsal_sonuc_id = table.Column<int>(type: "integer", nullable: true),
                    sektorel_etki_id = table.Column<int>(type: "integer", nullable: true),
                    bagimli_varlik_id = table.Column<int>(type: "integer", nullable: true),
                    rpo = table.Column<int>(type: "integer", nullable: true),
                    rpo_tip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    rto = table.Column<int>(type: "integer", nullable: true),
                    rto_tip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    mtpd = table.Column<int>(type: "integer", nullable: true),
                    mtpd_tip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    kurtarma_planlari = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    yedekleme_tipi_id = table.Column<int>(type: "integer", nullable: true),
                    yedekleme_turu = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    yedekleme_sikligi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    yedeklerin_saklama_suresi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    yedekleme_alani = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    yedekten_donus_plani = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    yedekleme_sorumlusu_id = table.Column<int>(type: "integer", nullable: true),
                    kisisel_veri_barindirma = table.Column<bool>(type: "boolean", nullable: true),
                    saklama_suresi = table.Column<int>(type: "integer", nullable: true),
                    saklama_suresi_tip = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    notlar = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    envantere_giris_tarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    envanter_guncelleme_tarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    envanterden_cikis_tarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    silinsin_mi = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_basili_bilgiler", x => x.id);
                    table.ForeignKey(
                        name: "fk_basili_bilgiler_bagimli_varliklar_bagimli_varlik_id",
                        column: x => x.bagimli_varlik_id,
                        principalTable: "bagimli_varliklar",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_basili_bilgiler_bilgi_siniflari_bilgi_sinifi_id",
                        column: x => x.bilgi_sinifi_id,
                        principalTable: "bilgi_siniflari",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_basili_bilgiler_butunlukler_butunluk_id",
                        column: x => x.butunluk_id,
                        principalTable: "butunlukler",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_basili_bilgiler_durumlar_durum_id",
                        column: x => x.durum_id,
                        principalTable: "durumlar",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_basili_bilgiler_erisilebilirlikler_erisilebilirlik_id",
                        column: x => x.erisilebilirlik_id,
                        principalTable: "erisilebilirlikler",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_basili_bilgiler_etkilenen_kisi_sayilari_etkilenen_kisi_sayi",
                        column: x => x.etkilenen_kisi_sayisi_id,
                        principalTable: "etkilenen_kisi_sayilari",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_basili_bilgiler_gizlilikler_gizlilik_id",
                        column: x => x.gizlilik_id,
                        principalTable: "gizlilikler",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_basili_bilgiler_kategoriler_alt_kategori_id",
                        column: x => x.alt_kategori_id,
                        principalTable: "kategoriler",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_basili_bilgiler_kategoriler_kategori_id",
                        column: x => x.kategori_id,
                        principalTable: "kategoriler",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_basili_bilgiler_kurumsal_sonuclar_kurumsal_sonuc_id",
                        column: x => x.kurumsal_sonuc_id,
                        principalTable: "kurumsal_sonuclar",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_basili_bilgiler_sektorel_etkiler_sektorel_etki_id",
                        column: x => x.sektorel_etki_id,
                        principalTable: "sektorel_etkiler",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_basili_bilgiler_toplumsal_sonuclar_toplumsal_sonuc_id",
                        column: x => x.toplumsal_sonuc_id,
                        principalTable: "toplumsal_sonuclar",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_basili_bilgiler_yedekleme_sorumlulari_yedekleme_sorumlusu_id",
                        column: x => x.yedekleme_sorumlusu_id,
                        principalTable: "yedekleme_sorumlulari",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_basili_bilgiler_yedekleme_tipleri_yedekleme_tipi_id",
                        column: x => x.yedekleme_tipi_id,
                        principalTable: "yedekleme_tipleri",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "elektronik_bilgiler",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    kategori_id = table.Column<int>(type: "integer", nullable: false),
                    alt_kategori_id = table.Column<int>(type: "integer", nullable: false),
                    varlik_adi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    kullanim_amaci = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    miktar = table.Column<int>(type: "integer", nullable: false),
                    durum_id = table.Column<int>(type: "integer", nullable: false),
                    konum = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    konum_id = table.Column<int>(type: "integer", nullable: true),
                    varlik_sahibi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    varlik_sahibi_id = table.Column<int>(type: "integer", nullable: true),
                    varlik_sahibi_alt_departman = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    varlik_sahibi_alt_departman_id = table.Column<int>(type: "integer", nullable: true),
                    operasyonel_sahibi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    operasyonel_sahibi_id = table.Column<int>(type: "integer", nullable: true),
                    bilgi_sinifi_id = table.Column<int>(type: "integer", nullable: true),
                    gizlilik_id = table.Column<int>(type: "integer", nullable: true),
                    butunluk_id = table.Column<int>(type: "integer", nullable: true),
                    erisilebilirlik_id = table.Column<int>(type: "integer", nullable: true),
                    etkilenen_kisi_sayisi_id = table.Column<int>(type: "integer", nullable: true),
                    toplumsal_sonuc_id = table.Column<int>(type: "integer", nullable: true),
                    kurumsal_sonuc_id = table.Column<int>(type: "integer", nullable: true),
                    sektorel_etki_id = table.Column<int>(type: "integer", nullable: true),
                    bagimli_varlik_id = table.Column<int>(type: "integer", nullable: true),
                    rpo = table.Column<int>(type: "integer", nullable: true),
                    rpo_tip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    rto = table.Column<int>(type: "integer", nullable: true),
                    rto_tip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    mtpd = table.Column<int>(type: "integer", nullable: true),
                    mtpd_tip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    kurtarma_planlari = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    yedekleme_tipi_id = table.Column<int>(type: "integer", nullable: true),
                    yedekleme_turu = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    yedekleme_sikligi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    yedeklerin_saklama_suresi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    yedekleme_alani = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    yedekten_donus_plani = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    yedekleme_sorumlusu_id = table.Column<int>(type: "integer", nullable: true),
                    kriptoloji = table.Column<bool>(type: "boolean", nullable: true),
                    kriptoloji_turu_id = table.Column<int>(type: "integer", nullable: true),
                    kullanilan_kriptoloji = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    anahtar_sorumlusu_id = table.Column<int>(type: "integer", nullable: true),
                    kisisel_veri_barindirma = table.Column<bool>(type: "boolean", nullable: true),
                    saklama_suresi = table.Column<int>(type: "integer", nullable: true),
                    saklama_suresi_tip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    notlar = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    envantere_giris_tarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    envanter_guncelleme_tarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    envanterden_cikis_tarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    silinsin_mi = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_elektronik_bilgiler", x => x.id);
                    table.ForeignKey(
                        name: "fk_elektronik_bilgiler_anahtar_sorumlulari_anahtar_sorumlusu_id",
                        column: x => x.anahtar_sorumlusu_id,
                        principalTable: "anahtar_sorumlulari",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_elektronik_bilgiler_bagimli_varliklar_bagimli_varlik_id",
                        column: x => x.bagimli_varlik_id,
                        principalTable: "bagimli_varliklar",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_elektronik_bilgiler_bilgi_siniflari_bilgi_sinifi_id",
                        column: x => x.bilgi_sinifi_id,
                        principalTable: "bilgi_siniflari",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_elektronik_bilgiler_butunlukler_butunluk_id",
                        column: x => x.butunluk_id,
                        principalTable: "butunlukler",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_elektronik_bilgiler_durumlar_durum_id",
                        column: x => x.durum_id,
                        principalTable: "durumlar",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_elektronik_bilgiler_erisilebilirlikler_erisilebilirlik_id",
                        column: x => x.erisilebilirlik_id,
                        principalTable: "erisilebilirlikler",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_elektronik_bilgiler_etkilenen_kisi_sayilari_etkilenen_kisi_",
                        column: x => x.etkilenen_kisi_sayisi_id,
                        principalTable: "etkilenen_kisi_sayilari",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_elektronik_bilgiler_gizlilikler_gizlilik_id",
                        column: x => x.gizlilik_id,
                        principalTable: "gizlilikler",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_elektronik_bilgiler_kategoriler_alt_kategori_id",
                        column: x => x.alt_kategori_id,
                        principalTable: "kategoriler",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_elektronik_bilgiler_kategoriler_kategori_id",
                        column: x => x.kategori_id,
                        principalTable: "kategoriler",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_elektronik_bilgiler_kriptoloji_turleri_kriptoloji_turu_id",
                        column: x => x.kriptoloji_turu_id,
                        principalTable: "kriptoloji_turleri",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_elektronik_bilgiler_kurumsal_sonuclar_kurumsal_sonuc_id",
                        column: x => x.kurumsal_sonuc_id,
                        principalTable: "kurumsal_sonuclar",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_elektronik_bilgiler_sektorel_etkiler_sektorel_etki_id",
                        column: x => x.sektorel_etki_id,
                        principalTable: "sektorel_etkiler",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_elektronik_bilgiler_toplumsal_sonuclar_toplumsal_sonuc_id",
                        column: x => x.toplumsal_sonuc_id,
                        principalTable: "toplumsal_sonuclar",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_elektronik_bilgiler_yedekleme_sorumlulari_yedekleme_sorumlu",
                        column: x => x.yedekleme_sorumlusu_id,
                        principalTable: "yedekleme_sorumlulari",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_elektronik_bilgiler_yedekleme_tipleri_yedekleme_tipi_id",
                        column: x => x.yedekleme_tipi_id,
                        principalTable: "yedekleme_tipleri",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "iot_cihazlari",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    kategori_id = table.Column<int>(type: "integer", nullable: false),
                    alt_kategori_id = table.Column<int>(type: "integer", nullable: false),
                    varlik_adi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    kullanim_amaci = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    miktar = table.Column<int>(type: "integer", nullable: false),
                    durum_id = table.Column<int>(type: "integer", nullable: false),
                    konum = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    konum_id = table.Column<int>(type: "integer", nullable: false),
                    varlik_sahibi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    varlik_sahibi_id = table.Column<int>(type: "integer", nullable: true),
                    varlik_sahibi_alt_departman = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    varlik_sahibi_alt_departman_id = table.Column<int>(type: "integer", nullable: true),
                    operasyonel_sahibi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    operasyonel_sahibi_id = table.Column<int>(type: "integer", nullable: true),
                    bilgi_sinifi_id = table.Column<int>(type: "integer", nullable: true),
                    gizlilik_id = table.Column<int>(type: "integer", nullable: true),
                    butunluk_id = table.Column<int>(type: "integer", nullable: true),
                    erisilebilirlik_id = table.Column<int>(type: "integer", nullable: true),
                    etkilenen_kisi_sayisi_id = table.Column<int>(type: "integer", nullable: true),
                    toplumsal_sonuc_id = table.Column<int>(type: "integer", nullable: true),
                    kurumsal_sonuc_id = table.Column<int>(type: "integer", nullable: true),
                    sektorel_etki_id = table.Column<int>(type: "integer", nullable: true),
                    bagimli_varlik_id = table.Column<int>(type: "integer", nullable: true),
                    rpo = table.Column<int>(type: "integer", nullable: true),
                    rpo_tip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    rto = table.Column<int>(type: "integer", nullable: true),
                    rto_tip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    mtpd = table.Column<int>(type: "integer", nullable: true),
                    mtpd_tip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    kurtarma_planlari = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    yedekleme_tipi_id = table.Column<int>(type: "integer", nullable: true),
                    yedekleme_turu = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    yedekleme_sikligi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    yedeklerin_saklama_suresi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    yedekleme_alani = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    yedekten_donus_plani = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    yedekleme_sorumlusu_id = table.Column<int>(type: "integer", nullable: true),
                    kriptoloji = table.Column<bool>(type: "boolean", nullable: true),
                    kriptoloji_turu_id = table.Column<int>(type: "integer", nullable: true),
                    kullanilan_kriptoloji = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    anahtar_sorumlusu_id = table.Column<int>(type: "integer", nullable: true),
                    kisisel_veri_barindirma = table.Column<bool>(type: "boolean", nullable: true),
                    anlik_mesajlasma_kullanimi = table.Column<bool>(type: "boolean", nullable: true),
                    bulut_bilisim = table.Column<bool>(type: "boolean", nullable: true),
                    yeni_gelistirmelerve_tedarik = table.Column<bool>(type: "boolean", nullable: true),
                    kritik_altyapi_sistemi = table.Column<bool>(type: "boolean", nullable: true),
                    ip_adresi = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    isletim_sistemi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    lisans_takip_sorumlusu_id = table.Column<int>(type: "integer", nullable: true),
                    marka_model = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    seri_numarasi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    zimmet_sahibi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    notlar = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    envantere_giris_tarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    envanter_guncelleme_tarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    envanterden_cikis_tarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    silinsin_mi = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_iot_cihazlari", x => x.id);
                    table.ForeignKey(
                        name: "fk_iot_cihazlari_anahtar_sorumlulari_anahtar_sorumlusu_id",
                        column: x => x.anahtar_sorumlusu_id,
                        principalTable: "anahtar_sorumlulari",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_iot_cihazlari_bagimli_varliklar_bagimli_varlik_id",
                        column: x => x.bagimli_varlik_id,
                        principalTable: "bagimli_varliklar",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_iot_cihazlari_bilgi_siniflari_bilgi_sinifi_id",
                        column: x => x.bilgi_sinifi_id,
                        principalTable: "bilgi_siniflari",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_iot_cihazlari_butunlukler_butunluk_id",
                        column: x => x.butunluk_id,
                        principalTable: "butunlukler",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_iot_cihazlari_durumlar_durum_id",
                        column: x => x.durum_id,
                        principalTable: "durumlar",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_iot_cihazlari_erisilebilirlikler_erisilebilirlik_id",
                        column: x => x.erisilebilirlik_id,
                        principalTable: "erisilebilirlikler",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_iot_cihazlari_etkilenen_kisi_sayilari_etkilenen_kisi_sayisi",
                        column: x => x.etkilenen_kisi_sayisi_id,
                        principalTable: "etkilenen_kisi_sayilari",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_iot_cihazlari_gizlilikler_gizlilik_id",
                        column: x => x.gizlilik_id,
                        principalTable: "gizlilikler",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_iot_cihazlari_kategoriler_alt_kategori_id",
                        column: x => x.alt_kategori_id,
                        principalTable: "kategoriler",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_iot_cihazlari_kategoriler_kategori_id",
                        column: x => x.kategori_id,
                        principalTable: "kategoriler",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_iot_cihazlari_kriptoloji_turleri_kriptoloji_turu_id",
                        column: x => x.kriptoloji_turu_id,
                        principalTable: "kriptoloji_turleri",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_iot_cihazlari_kurumsal_sonuclar_kurumsal_sonuc_id",
                        column: x => x.kurumsal_sonuc_id,
                        principalTable: "kurumsal_sonuclar",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_iot_cihazlari_lisans_takip_sorumlulari_lisans_takip_sorumlu",
                        column: x => x.lisans_takip_sorumlusu_id,
                        principalTable: "lisans_takip_sorumlulari",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_iot_cihazlari_sektorel_etkiler_sektorel_etki_id",
                        column: x => x.sektorel_etki_id,
                        principalTable: "sektorel_etkiler",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_iot_cihazlari_toplumsal_sonuclar_toplumsal_sonuc_id",
                        column: x => x.toplumsal_sonuc_id,
                        principalTable: "toplumsal_sonuclar",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_iot_cihazlari_yedekleme_sorumlulari_yedekleme_sorumlusu_id",
                        column: x => x.yedekleme_sorumlusu_id,
                        principalTable: "yedekleme_sorumlulari",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_iot_cihazlari_yedekleme_tipleri_yedekleme_tipi_id",
                        column: x => x.yedekleme_tipi_id,
                        principalTable: "yedekleme_tipleri",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "tasinabilir_cihaz_ve_ortamlar",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    kategori_id = table.Column<int>(type: "integer", nullable: false),
                    alt_kategori_id = table.Column<int>(type: "integer", nullable: false),
                    varlik_adi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    kullanim_amaci = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    miktar = table.Column<int>(type: "integer", nullable: false),
                    durum_id = table.Column<int>(type: "integer", nullable: false),
                    konum = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    konum_id = table.Column<int>(type: "integer", nullable: false),
                    varlik_sahibi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    varlik_sahibi_id = table.Column<int>(type: "integer", nullable: true),
                    varlik_sahibi_alt_departman = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    varlik_sahibi_alt_departman_id = table.Column<int>(type: "integer", nullable: true),
                    operasyonel_sahibi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    operasyonel_sahibi_id = table.Column<int>(type: "integer", nullable: true),
                    bilgi_sinifi_id = table.Column<int>(type: "integer", nullable: true),
                    gizlilik_id = table.Column<int>(type: "integer", nullable: true),
                    butunluk_id = table.Column<int>(type: "integer", nullable: true),
                    erisilebilirlik_id = table.Column<int>(type: "integer", nullable: true),
                    etkilenen_kisi_sayisi_id = table.Column<int>(type: "integer", nullable: true),
                    toplumsal_sonuc_id = table.Column<int>(type: "integer", nullable: true),
                    kurumsal_sonuc_id = table.Column<int>(type: "integer", nullable: true),
                    sektorel_etki_id = table.Column<int>(type: "integer", nullable: true),
                    bagimli_varlik_id = table.Column<int>(type: "integer", nullable: true),
                    rpo = table.Column<int>(type: "integer", nullable: true),
                    rpo_tip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    rto = table.Column<int>(type: "integer", nullable: true),
                    rto_tip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    mtpd = table.Column<int>(type: "integer", nullable: true),
                    mtpd_tip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    kurtarma_planlari = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    yedekleme_tipi_id = table.Column<int>(type: "integer", nullable: true),
                    yedekleme_turu = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    yedekleme_sikligi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    yedeklerin_saklama_suresi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    yedekleme_alani = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    yedekten_donus_plani = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    yedekleme_sorumlusu_id = table.Column<int>(type: "integer", nullable: true),
                    kriptoloji = table.Column<bool>(type: "boolean", nullable: true),
                    kriptoloji_turu_id = table.Column<int>(type: "integer", nullable: true),
                    kullanilan_kriptoloji = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    anahtar_sorumlusu_id = table.Column<int>(type: "integer", nullable: true),
                    kisisel_veri_barindirma = table.Column<bool>(type: "boolean", nullable: true),
                    anlik_mesajlasma_kullanimi = table.Column<bool>(type: "boolean", nullable: true),
                    bulut_bilisim = table.Column<bool>(type: "boolean", nullable: true),
                    yeni_gelismelerve_tedarik = table.Column<bool>(type: "boolean", nullable: true),
                    kritik_altyapi_sistemi = table.Column<bool>(type: "boolean", nullable: true),
                    ip_adresi = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    isletim_sistemi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    lisans_takip_sorumlusu_id = table.Column<int>(type: "integer", nullable: true),
                    marka_model = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    seri_numarasi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    zimmet_sahibi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    notlar = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    envantere_giris_tarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    envanter_guncelleme_tarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    envanterden_cikis_tarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    silinsin_mi = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tasinabilir_cihaz_ve_ortamlar", x => x.id);
                    table.ForeignKey(
                        name: "fk_tasinabilir_cihaz_ve_ortamlar_anahtar_sorumlulari_anahtar_s",
                        column: x => x.anahtar_sorumlusu_id,
                        principalTable: "anahtar_sorumlulari",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_tasinabilir_cihaz_ve_ortamlar_bagimli_varliklar_bagimli_var",
                        column: x => x.bagimli_varlik_id,
                        principalTable: "bagimli_varliklar",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_tasinabilir_cihaz_ve_ortamlar_bilgi_siniflari_bilgi_sinifi_",
                        column: x => x.bilgi_sinifi_id,
                        principalTable: "bilgi_siniflari",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_tasinabilir_cihaz_ve_ortamlar_butunlukler_butunluk_id",
                        column: x => x.butunluk_id,
                        principalTable: "butunlukler",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_tasinabilir_cihaz_ve_ortamlar_durumlar_durum_id",
                        column: x => x.durum_id,
                        principalTable: "durumlar",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_tasinabilir_cihaz_ve_ortamlar_erisilebilirlikler_erisilebil",
                        column: x => x.erisilebilirlik_id,
                        principalTable: "erisilebilirlikler",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_tasinabilir_cihaz_ve_ortamlar_etkilenen_kisi_sayilari_etkil",
                        column: x => x.etkilenen_kisi_sayisi_id,
                        principalTable: "etkilenen_kisi_sayilari",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_tasinabilir_cihaz_ve_ortamlar_gizlilikler_gizlilik_id",
                        column: x => x.gizlilik_id,
                        principalTable: "gizlilikler",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_tasinabilir_cihaz_ve_ortamlar_kategoriler_alt_kategori_id",
                        column: x => x.alt_kategori_id,
                        principalTable: "kategoriler",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_tasinabilir_cihaz_ve_ortamlar_kategoriler_kategori_id",
                        column: x => x.kategori_id,
                        principalTable: "kategoriler",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_tasinabilir_cihaz_ve_ortamlar_kriptoloji_turleri_kriptoloji",
                        column: x => x.kriptoloji_turu_id,
                        principalTable: "kriptoloji_turleri",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_tasinabilir_cihaz_ve_ortamlar_kurumsal_sonuclar_kurumsal_so",
                        column: x => x.kurumsal_sonuc_id,
                        principalTable: "kurumsal_sonuclar",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_tasinabilir_cihaz_ve_ortamlar_lisans_takip_sorumlulari_lisa",
                        column: x => x.lisans_takip_sorumlusu_id,
                        principalTable: "lisans_takip_sorumlulari",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_tasinabilir_cihaz_ve_ortamlar_sektorel_etkiler_sektorel_etk",
                        column: x => x.sektorel_etki_id,
                        principalTable: "sektorel_etkiler",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_tasinabilir_cihaz_ve_ortamlar_toplumsal_sonuclar_toplumsal_",
                        column: x => x.toplumsal_sonuc_id,
                        principalTable: "toplumsal_sonuclar",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_tasinabilir_cihaz_ve_ortamlar_yedekleme_sorumlulari_yedekle",
                        column: x => x.yedekleme_sorumlusu_id,
                        principalTable: "yedekleme_sorumlulari",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_tasinabilir_cihaz_ve_ortamlar_yedekleme_tipleri_yedekleme_t",
                        column: x => x.yedekleme_tipi_id,
                        principalTable: "yedekleme_tipleri",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "uygulamalar",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    kategori_id = table.Column<int>(type: "integer", nullable: false),
                    alt_kategori_id = table.Column<int>(type: "integer", nullable: false),
                    varlik_adi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    kullanim_amaci = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    miktar = table.Column<int>(type: "integer", nullable: false),
                    durum_id = table.Column<int>(type: "integer", nullable: false),
                    konum = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    konum_id = table.Column<int>(type: "integer", nullable: false),
                    varlik_sahibi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    varlik_sahibi_id = table.Column<int>(type: "integer", nullable: true),
                    varlik_sahibi_alt_departman = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    varlik_sahibi_alt_departman_id = table.Column<int>(type: "integer", nullable: true),
                    operasyonel_sahibi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    operasyonel_sahibi_id = table.Column<int>(type: "integer", nullable: true),
                    bilgi_sinifi_id = table.Column<int>(type: "integer", nullable: true),
                    gizlilik_id = table.Column<int>(type: "integer", nullable: true),
                    butunluk_id = table.Column<int>(type: "integer", nullable: true),
                    erisilebilirlik_id = table.Column<int>(type: "integer", nullable: true),
                    etkilenen_kisi_sayisi_id = table.Column<int>(type: "integer", nullable: true),
                    toplumsal_sonuc_id = table.Column<int>(type: "integer", nullable: true),
                    kurumsal_sonuc_id = table.Column<int>(type: "integer", nullable: true),
                    sektorel_etki_id = table.Column<int>(type: "integer", nullable: true),
                    bagimli_varlik_id = table.Column<int>(type: "integer", nullable: true),
                    rpo = table.Column<int>(type: "integer", nullable: true),
                    rpo_tip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    rto = table.Column<int>(type: "integer", nullable: true),
                    rto_tip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    mtpd = table.Column<int>(type: "integer", nullable: true),
                    mtpd_tip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    kurtarma_planlari = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    yedekleme_tipi_id = table.Column<int>(type: "integer", nullable: true),
                    yedekleme_turu = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    yedekleme_sikligi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    yedeklerin_saklama_suresi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    yedekleme_alani = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    yedekten_donus_plani = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    yedekleme_sorumlusu_id = table.Column<int>(type: "integer", nullable: true),
                    kriptoloji = table.Column<bool>(type: "boolean", nullable: true),
                    kriptoloji_turu_id = table.Column<int>(type: "integer", nullable: true),
                    kullanilan_kriptoloji = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    anahtar_sorumlusu_id = table.Column<int>(type: "integer", nullable: true),
                    kisisel_veri_barindirma = table.Column<bool>(type: "boolean", nullable: true),
                    anlik_mesajlasma_kullanimi = table.Column<bool>(type: "boolean", nullable: true),
                    bulut_bilisim = table.Column<bool>(type: "boolean", nullable: true),
                    yeni_gelistirmelerve_tedarik = table.Column<bool>(type: "boolean", nullable: true),
                    kritik_altyapi_sistemi = table.Column<bool>(type: "boolean", nullable: true),
                    ip_adresi = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    url_adresi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    yazilim_surumu = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    yazilim_yayimcisi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    edinim_tarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    lisans_adedi = table.Column<int>(type: "integer", nullable: true),
                    lisans_takip_sorumlusu_id = table.Column<int>(type: "integer", nullable: true),
                    destek_durumu_id = table.Column<int>(type: "integer", nullable: true),
                    destek_alinan_tedarikci = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    bakim_suresi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    bakim_kapsami = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    yazilimin_yuklendigi_donanimlar = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    veritabanive_surumu = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    veritabani_versiyonu = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    notlar = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    envantere_giris_tarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    envanter_guncelleme_tarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    envanterden_cikis_tarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    silinsin_mi = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_uygulamalar", x => x.id);
                    table.ForeignKey(
                        name: "fk_uygulamalar_anahtar_sorumlulari_anahtar_sorumlusu_id",
                        column: x => x.anahtar_sorumlusu_id,
                        principalTable: "anahtar_sorumlulari",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_uygulamalar_bagimli_varliklar_bagimli_varlik_id",
                        column: x => x.bagimli_varlik_id,
                        principalTable: "bagimli_varliklar",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_uygulamalar_bilgi_siniflari_bilgi_sinifi_id",
                        column: x => x.bilgi_sinifi_id,
                        principalTable: "bilgi_siniflari",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_uygulamalar_butunlukler_butunluk_id",
                        column: x => x.butunluk_id,
                        principalTable: "butunlukler",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_uygulamalar_destek_durumlari_destek_durumu_id",
                        column: x => x.destek_durumu_id,
                        principalTable: "destek_durumlari",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_uygulamalar_durumlar_durum_id",
                        column: x => x.durum_id,
                        principalTable: "durumlar",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_uygulamalar_erisilebilirlikler_erisilebilirlik_id",
                        column: x => x.erisilebilirlik_id,
                        principalTable: "erisilebilirlikler",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_uygulamalar_etkilenen_kisi_sayilari_etkilenen_kisi_sayisi_id",
                        column: x => x.etkilenen_kisi_sayisi_id,
                        principalTable: "etkilenen_kisi_sayilari",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_uygulamalar_gizlilikler_gizlilik_id",
                        column: x => x.gizlilik_id,
                        principalTable: "gizlilikler",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_uygulamalar_kategoriler_alt_kategori_id",
                        column: x => x.alt_kategori_id,
                        principalTable: "kategoriler",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_uygulamalar_kategoriler_kategori_id",
                        column: x => x.kategori_id,
                        principalTable: "kategoriler",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_uygulamalar_kriptoloji_turleri_kriptoloji_turu_id",
                        column: x => x.kriptoloji_turu_id,
                        principalTable: "kriptoloji_turleri",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_uygulamalar_kurumsal_sonuclar_kurumsal_sonuc_id",
                        column: x => x.kurumsal_sonuc_id,
                        principalTable: "kurumsal_sonuclar",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_uygulamalar_lisans_takip_sorumlulari_lisans_takip_sorumlusu",
                        column: x => x.lisans_takip_sorumlusu_id,
                        principalTable: "lisans_takip_sorumlulari",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_uygulamalar_sektorel_etkiler_sektorel_etki_id",
                        column: x => x.sektorel_etki_id,
                        principalTable: "sektorel_etkiler",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_uygulamalar_toplumsal_sonuclar_toplumsal_sonuc_id",
                        column: x => x.toplumsal_sonuc_id,
                        principalTable: "toplumsal_sonuclar",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_uygulamalar_yedekleme_sorumlulari_yedekleme_sorumlusu_id",
                        column: x => x.yedekleme_sorumlusu_id,
                        principalTable: "yedekleme_sorumlulari",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_uygulamalar_yedekleme_tipleri_yedekleme_tipi_id",
                        column: x => x.yedekleme_tipi_id,
                        principalTable: "yedekleme_tipleri",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "veritabanlari",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    kategori_id = table.Column<int>(type: "integer", nullable: false),
                    alt_kategori_id = table.Column<int>(type: "integer", nullable: false),
                    varlik_adi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    kullanim_amaci = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    miktar = table.Column<int>(type: "integer", nullable: false),
                    durum_id = table.Column<int>(type: "integer", nullable: false),
                    konum = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    konum_id = table.Column<int>(type: "integer", nullable: true),
                    varlik_sahibi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    varlik_sahibi_id = table.Column<int>(type: "integer", nullable: true),
                    varlik_sahibi_alt_departman = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    varlik_sahibi_alt_departman_id = table.Column<int>(type: "integer", nullable: true),
                    operasyonel_sahibi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    operasyonel_sahibi_id = table.Column<int>(type: "integer", nullable: true),
                    bilgi_sinifi_id = table.Column<int>(type: "integer", nullable: true),
                    gizlilik_id = table.Column<int>(type: "integer", nullable: true),
                    butunluk_id = table.Column<int>(type: "integer", nullable: true),
                    erisilebilirlik_id = table.Column<int>(type: "integer", nullable: true),
                    etkilenen_kisi_sayisi_id = table.Column<int>(type: "integer", nullable: true),
                    toplumsal_sonuc_id = table.Column<int>(type: "integer", nullable: true),
                    kurumsal_sonuc_id = table.Column<int>(type: "integer", nullable: true),
                    sektorel_etki_id = table.Column<int>(type: "integer", nullable: true),
                    bagimli_varlik_id = table.Column<int>(type: "integer", nullable: true),
                    rpo = table.Column<int>(type: "integer", nullable: true),
                    rpo_tip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    rto = table.Column<int>(type: "integer", nullable: true),
                    rto_tip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    mtpd = table.Column<int>(type: "integer", nullable: true),
                    mtpd_tip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    kurtarma_planlari = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    yedekleme_tipi_id = table.Column<int>(type: "integer", nullable: true),
                    yedekleme_turu = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    yedekleme_sikligi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    yedeklerin_saklama_suresi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    yedekleme_alani = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    yedekten_donus_plani = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    yedekleme_sorumlusu_id = table.Column<int>(type: "integer", nullable: true),
                    kriptoloji = table.Column<bool>(type: "boolean", nullable: true),
                    kriptoloji_turu_id = table.Column<int>(type: "integer", nullable: true),
                    kullanilan_kriptoloji = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    anahtar_sorumlusu_id = table.Column<int>(type: "integer", nullable: true),
                    kisisel_veri_barindirma = table.Column<bool>(type: "boolean", nullable: false),
                    bulut_bilisim = table.Column<bool>(type: "boolean", nullable: false),
                    yeni_gelismelerve_tedarik = table.Column<bool>(type: "boolean", nullable: false),
                    kritik_altyapi_sistemi = table.Column<bool>(type: "boolean", nullable: false),
                    notlar = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    envantere_giris_tarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    envanter_guncelleme_tarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    envanterden_cikis_tarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    silinsin_mi = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_veritabanlari", x => x.id);
                    table.ForeignKey(
                        name: "fk_veritabanlari_anahtar_sorumlulari_anahtar_sorumlusu_id",
                        column: x => x.anahtar_sorumlusu_id,
                        principalTable: "anahtar_sorumlulari",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_veritabanlari_bagimli_varliklar_bagimli_varlik_id",
                        column: x => x.bagimli_varlik_id,
                        principalTable: "bagimli_varliklar",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_veritabanlari_bilgi_siniflari_bilgi_sinifi_id",
                        column: x => x.bilgi_sinifi_id,
                        principalTable: "bilgi_siniflari",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_veritabanlari_butunlukler_butunluk_id",
                        column: x => x.butunluk_id,
                        principalTable: "butunlukler",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_veritabanlari_durumlar_durum_id",
                        column: x => x.durum_id,
                        principalTable: "durumlar",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_veritabanlari_erisilebilirlikler_erisilebilirlik_id",
                        column: x => x.erisilebilirlik_id,
                        principalTable: "erisilebilirlikler",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_veritabanlari_etkilenen_kisi_sayilari_etkilenen_kisi_sayisi",
                        column: x => x.etkilenen_kisi_sayisi_id,
                        principalTable: "etkilenen_kisi_sayilari",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_veritabanlari_gizlilikler_gizlilik_id",
                        column: x => x.gizlilik_id,
                        principalTable: "gizlilikler",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_veritabanlari_kategoriler_alt_kategori_id",
                        column: x => x.alt_kategori_id,
                        principalTable: "kategoriler",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_veritabanlari_kategoriler_kategori_id",
                        column: x => x.kategori_id,
                        principalTable: "kategoriler",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_veritabanlari_kurumsal_sonuclar_kurumsal_sonuc_id",
                        column: x => x.kurumsal_sonuc_id,
                        principalTable: "kurumsal_sonuclar",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_veritabanlari_sektorel_etkiler_sektorel_etki_id",
                        column: x => x.sektorel_etki_id,
                        principalTable: "sektorel_etkiler",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_veritabanlari_toplumsal_sonuclar_toplumsal_sonuc_id",
                        column: x => x.toplumsal_sonuc_id,
                        principalTable: "toplumsal_sonuclar",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_veritabanlari_yedekleme_sorumlulari_yedekleme_sorumlusu_id",
                        column: x => x.yedekleme_sorumlusu_id,
                        principalTable: "yedekleme_sorumlulari",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_veritabanlari_yedekleme_tipleri_yedekleme_tipi_id",
                        column: x => x.yedekleme_tipi_id,
                        principalTable: "yedekleme_tipleri",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_agve_sistemler_alt_kategori_id",
                table: "agve_sistemler",
                column: "alt_kategori_id");

            migrationBuilder.CreateIndex(
                name: "ix_agve_sistemler_anahtar_sorumlusu_id",
                table: "agve_sistemler",
                column: "anahtar_sorumlusu_id");

            migrationBuilder.CreateIndex(
                name: "ix_agve_sistemler_bagimli_varlik_id",
                table: "agve_sistemler",
                column: "bagimli_varlik_id");

            migrationBuilder.CreateIndex(
                name: "ix_agve_sistemler_bilgi_sinifi_id",
                table: "agve_sistemler",
                column: "bilgi_sinifi_id");

            migrationBuilder.CreateIndex(
                name: "ix_agve_sistemler_butunluk_id",
                table: "agve_sistemler",
                column: "butunluk_id");

            migrationBuilder.CreateIndex(
                name: "ix_agve_sistemler_durum_id",
                table: "agve_sistemler",
                column: "durum_id");

            migrationBuilder.CreateIndex(
                name: "ix_agve_sistemler_erisilebilirlik_id",
                table: "agve_sistemler",
                column: "erisilebilirlik_id");

            migrationBuilder.CreateIndex(
                name: "ix_agve_sistemler_etkilenen_kisi_sayisi_id",
                table: "agve_sistemler",
                column: "etkilenen_kisi_sayisi_id");

            migrationBuilder.CreateIndex(
                name: "ix_agve_sistemler_gizlilik_id",
                table: "agve_sistemler",
                column: "gizlilik_id");

            migrationBuilder.CreateIndex(
                name: "ix_agve_sistemler_kategori_id",
                table: "agve_sistemler",
                column: "kategori_id");

            migrationBuilder.CreateIndex(
                name: "ix_agve_sistemler_kriptoloji_turu_id",
                table: "agve_sistemler",
                column: "kriptoloji_turu_id");

            migrationBuilder.CreateIndex(
                name: "ix_agve_sistemler_kurumsal_sonuc_id",
                table: "agve_sistemler",
                column: "kurumsal_sonuc_id");

            migrationBuilder.CreateIndex(
                name: "ix_agve_sistemler_lisans_takip_sorumlusu_id",
                table: "agve_sistemler",
                column: "lisans_takip_sorumlusu_id");

            migrationBuilder.CreateIndex(
                name: "ix_agve_sistemler_sektorel_etki_id",
                table: "agve_sistemler",
                column: "sektorel_etki_id");

            migrationBuilder.CreateIndex(
                name: "ix_agve_sistemler_toplumsal_sonuc_id",
                table: "agve_sistemler",
                column: "toplumsal_sonuc_id");

            migrationBuilder.CreateIndex(
                name: "ix_agve_sistemler_yedekleme_sorumlusu_id",
                table: "agve_sistemler",
                column: "yedekleme_sorumlusu_id");

            migrationBuilder.CreateIndex(
                name: "ix_agve_sistemler_yedekleme_tipi_id",
                table: "agve_sistemler",
                column: "yedekleme_tipi_id");

            migrationBuilder.CreateIndex(
                name: "ix_basili_bilgiler_alt_kategori_id",
                table: "basili_bilgiler",
                column: "alt_kategori_id");

            migrationBuilder.CreateIndex(
                name: "ix_basili_bilgiler_bagimli_varlik_id",
                table: "basili_bilgiler",
                column: "bagimli_varlik_id");

            migrationBuilder.CreateIndex(
                name: "ix_basili_bilgiler_bilgi_sinifi_id",
                table: "basili_bilgiler",
                column: "bilgi_sinifi_id");

            migrationBuilder.CreateIndex(
                name: "ix_basili_bilgiler_butunluk_id",
                table: "basili_bilgiler",
                column: "butunluk_id");

            migrationBuilder.CreateIndex(
                name: "ix_basili_bilgiler_durum_id",
                table: "basili_bilgiler",
                column: "durum_id");

            migrationBuilder.CreateIndex(
                name: "ix_basili_bilgiler_erisilebilirlik_id",
                table: "basili_bilgiler",
                column: "erisilebilirlik_id");

            migrationBuilder.CreateIndex(
                name: "ix_basili_bilgiler_etkilenen_kisi_sayisi_id",
                table: "basili_bilgiler",
                column: "etkilenen_kisi_sayisi_id");

            migrationBuilder.CreateIndex(
                name: "ix_basili_bilgiler_gizlilik_id",
                table: "basili_bilgiler",
                column: "gizlilik_id");

            migrationBuilder.CreateIndex(
                name: "ix_basili_bilgiler_kategori_id",
                table: "basili_bilgiler",
                column: "kategori_id");

            migrationBuilder.CreateIndex(
                name: "ix_basili_bilgiler_kurumsal_sonuc_id",
                table: "basili_bilgiler",
                column: "kurumsal_sonuc_id");

            migrationBuilder.CreateIndex(
                name: "ix_basili_bilgiler_sektorel_etki_id",
                table: "basili_bilgiler",
                column: "sektorel_etki_id");

            migrationBuilder.CreateIndex(
                name: "ix_basili_bilgiler_toplumsal_sonuc_id",
                table: "basili_bilgiler",
                column: "toplumsal_sonuc_id");

            migrationBuilder.CreateIndex(
                name: "ix_basili_bilgiler_yedekleme_sorumlusu_id",
                table: "basili_bilgiler",
                column: "yedekleme_sorumlusu_id");

            migrationBuilder.CreateIndex(
                name: "ix_basili_bilgiler_yedekleme_tipi_id",
                table: "basili_bilgiler",
                column: "yedekleme_tipi_id");

            migrationBuilder.CreateIndex(
                name: "ix_birimler_ust_id",
                table: "birimler",
                column: "ust_id");

            migrationBuilder.CreateIndex(
                name: "ix_birimler_yol",
                table: "birimler",
                column: "yol");

            migrationBuilder.CreateIndex(
                name: "ix_elektronik_bilgiler_alt_kategori_id",
                table: "elektronik_bilgiler",
                column: "alt_kategori_id");

            migrationBuilder.CreateIndex(
                name: "ix_elektronik_bilgiler_anahtar_sorumlusu_id",
                table: "elektronik_bilgiler",
                column: "anahtar_sorumlusu_id");

            migrationBuilder.CreateIndex(
                name: "ix_elektronik_bilgiler_bagimli_varlik_id",
                table: "elektronik_bilgiler",
                column: "bagimli_varlik_id");

            migrationBuilder.CreateIndex(
                name: "ix_elektronik_bilgiler_bilgi_sinifi_id",
                table: "elektronik_bilgiler",
                column: "bilgi_sinifi_id");

            migrationBuilder.CreateIndex(
                name: "ix_elektronik_bilgiler_butunluk_id",
                table: "elektronik_bilgiler",
                column: "butunluk_id");

            migrationBuilder.CreateIndex(
                name: "ix_elektronik_bilgiler_durum_id",
                table: "elektronik_bilgiler",
                column: "durum_id");

            migrationBuilder.CreateIndex(
                name: "ix_elektronik_bilgiler_erisilebilirlik_id",
                table: "elektronik_bilgiler",
                column: "erisilebilirlik_id");

            migrationBuilder.CreateIndex(
                name: "ix_elektronik_bilgiler_etkilenen_kisi_sayisi_id",
                table: "elektronik_bilgiler",
                column: "etkilenen_kisi_sayisi_id");

            migrationBuilder.CreateIndex(
                name: "ix_elektronik_bilgiler_gizlilik_id",
                table: "elektronik_bilgiler",
                column: "gizlilik_id");

            migrationBuilder.CreateIndex(
                name: "ix_elektronik_bilgiler_kategori_id",
                table: "elektronik_bilgiler",
                column: "kategori_id");

            migrationBuilder.CreateIndex(
                name: "ix_elektronik_bilgiler_kriptoloji_turu_id",
                table: "elektronik_bilgiler",
                column: "kriptoloji_turu_id");

            migrationBuilder.CreateIndex(
                name: "ix_elektronik_bilgiler_kurumsal_sonuc_id",
                table: "elektronik_bilgiler",
                column: "kurumsal_sonuc_id");

            migrationBuilder.CreateIndex(
                name: "ix_elektronik_bilgiler_sektorel_etki_id",
                table: "elektronik_bilgiler",
                column: "sektorel_etki_id");

            migrationBuilder.CreateIndex(
                name: "ix_elektronik_bilgiler_toplumsal_sonuc_id",
                table: "elektronik_bilgiler",
                column: "toplumsal_sonuc_id");

            migrationBuilder.CreateIndex(
                name: "ix_elektronik_bilgiler_yedekleme_sorumlusu_id",
                table: "elektronik_bilgiler",
                column: "yedekleme_sorumlusu_id");

            migrationBuilder.CreateIndex(
                name: "ix_elektronik_bilgiler_yedekleme_tipi_id",
                table: "elektronik_bilgiler",
                column: "yedekleme_tipi_id");

            migrationBuilder.CreateIndex(
                name: "ix_eposta_talepleri_kurum_id",
                table: "eposta_talepleri",
                column: "kurum_id");

            migrationBuilder.CreateIndex(
                name: "ix_fiziksel_mekanlar_alt_kategori_id",
                table: "fiziksel_mekanlar",
                column: "alt_kategori_id");

            migrationBuilder.CreateIndex(
                name: "ix_fiziksel_mekanlar_bagimli_varlik_id",
                table: "fiziksel_mekanlar",
                column: "bagimli_varlik_id");

            migrationBuilder.CreateIndex(
                name: "ix_fiziksel_mekanlar_bilgi_sinifi_id",
                table: "fiziksel_mekanlar",
                column: "bilgi_sinifi_id");

            migrationBuilder.CreateIndex(
                name: "ix_fiziksel_mekanlar_butunluk_id",
                table: "fiziksel_mekanlar",
                column: "butunluk_id");

            migrationBuilder.CreateIndex(
                name: "ix_fiziksel_mekanlar_durum_id",
                table: "fiziksel_mekanlar",
                column: "durum_id");

            migrationBuilder.CreateIndex(
                name: "ix_fiziksel_mekanlar_erisilebilirlik_id",
                table: "fiziksel_mekanlar",
                column: "erisilebilirlik_id");

            migrationBuilder.CreateIndex(
                name: "ix_fiziksel_mekanlar_etkilenen_kisi_sayisi_id",
                table: "fiziksel_mekanlar",
                column: "etkilenen_kisi_sayisi_id");

            migrationBuilder.CreateIndex(
                name: "ix_fiziksel_mekanlar_gizlilik_id",
                table: "fiziksel_mekanlar",
                column: "gizlilik_id");

            migrationBuilder.CreateIndex(
                name: "ix_fiziksel_mekanlar_kategori_id",
                table: "fiziksel_mekanlar",
                column: "kategori_id");

            migrationBuilder.CreateIndex(
                name: "ix_fiziksel_mekanlar_kurumsal_sonuc_id",
                table: "fiziksel_mekanlar",
                column: "kurumsal_sonuc_id");

            migrationBuilder.CreateIndex(
                name: "ix_fiziksel_mekanlar_sektorel_etki_id",
                table: "fiziksel_mekanlar",
                column: "sektorel_etki_id");

            migrationBuilder.CreateIndex(
                name: "ix_fiziksel_mekanlar_toplumsal_sonuc_id",
                table: "fiziksel_mekanlar",
                column: "toplumsal_sonuc_id");

            migrationBuilder.CreateIndex(
                name: "ix_iot_cihazlari_alt_kategori_id",
                table: "iot_cihazlari",
                column: "alt_kategori_id");

            migrationBuilder.CreateIndex(
                name: "ix_iot_cihazlari_anahtar_sorumlusu_id",
                table: "iot_cihazlari",
                column: "anahtar_sorumlusu_id");

            migrationBuilder.CreateIndex(
                name: "ix_iot_cihazlari_bagimli_varlik_id",
                table: "iot_cihazlari",
                column: "bagimli_varlik_id");

            migrationBuilder.CreateIndex(
                name: "ix_iot_cihazlari_bilgi_sinifi_id",
                table: "iot_cihazlari",
                column: "bilgi_sinifi_id");

            migrationBuilder.CreateIndex(
                name: "ix_iot_cihazlari_butunluk_id",
                table: "iot_cihazlari",
                column: "butunluk_id");

            migrationBuilder.CreateIndex(
                name: "ix_iot_cihazlari_durum_id",
                table: "iot_cihazlari",
                column: "durum_id");

            migrationBuilder.CreateIndex(
                name: "ix_iot_cihazlari_erisilebilirlik_id",
                table: "iot_cihazlari",
                column: "erisilebilirlik_id");

            migrationBuilder.CreateIndex(
                name: "ix_iot_cihazlari_etkilenen_kisi_sayisi_id",
                table: "iot_cihazlari",
                column: "etkilenen_kisi_sayisi_id");

            migrationBuilder.CreateIndex(
                name: "ix_iot_cihazlari_gizlilik_id",
                table: "iot_cihazlari",
                column: "gizlilik_id");

            migrationBuilder.CreateIndex(
                name: "ix_iot_cihazlari_kategori_id",
                table: "iot_cihazlari",
                column: "kategori_id");

            migrationBuilder.CreateIndex(
                name: "ix_iot_cihazlari_kriptoloji_turu_id",
                table: "iot_cihazlari",
                column: "kriptoloji_turu_id");

            migrationBuilder.CreateIndex(
                name: "ix_iot_cihazlari_kurumsal_sonuc_id",
                table: "iot_cihazlari",
                column: "kurumsal_sonuc_id");

            migrationBuilder.CreateIndex(
                name: "ix_iot_cihazlari_lisans_takip_sorumlusu_id",
                table: "iot_cihazlari",
                column: "lisans_takip_sorumlusu_id");

            migrationBuilder.CreateIndex(
                name: "ix_iot_cihazlari_sektorel_etki_id",
                table: "iot_cihazlari",
                column: "sektorel_etki_id");

            migrationBuilder.CreateIndex(
                name: "ix_iot_cihazlari_toplumsal_sonuc_id",
                table: "iot_cihazlari",
                column: "toplumsal_sonuc_id");

            migrationBuilder.CreateIndex(
                name: "ix_iot_cihazlari_yedekleme_sorumlusu_id",
                table: "iot_cihazlari",
                column: "yedekleme_sorumlusu_id");

            migrationBuilder.CreateIndex(
                name: "ix_iot_cihazlari_yedekleme_tipi_id",
                table: "iot_cihazlari",
                column: "yedekleme_tipi_id");

            migrationBuilder.CreateIndex(
                name: "ix_kategoriler_ust_id",
                table: "kategoriler",
                column: "ust_id");

            migrationBuilder.CreateIndex(
                name: "ix_kriptografi_envanterleri_anahtar_sorumlusu_id",
                table: "kriptografi_envanterleri",
                column: "anahtar_sorumlusu_id");

            migrationBuilder.CreateIndex(
                name: "ix_kriptografi_envanterleri_kullanim_seviyesi_id",
                table: "kriptografi_envanterleri",
                column: "kullanim_seviyesi_id");

            migrationBuilder.CreateIndex(
                name: "ix_kullanici_birimler_kullanici_id",
                table: "kullanici_birimler",
                column: "kullanici_id");

            migrationBuilder.CreateIndex(
                name: "ix_kullanici_roller_kullanici_id",
                table: "kullanici_roller",
                column: "kullanici_id");

            migrationBuilder.CreateIndex(
                name: "ix_kullanici_roller_rol_id",
                table: "kullanici_roller",
                column: "rol_id");

            migrationBuilder.CreateIndex(
                name: "ix_personeller_alt_kategori_id",
                table: "personeller",
                column: "alt_kategori_id");

            migrationBuilder.CreateIndex(
                name: "ix_personeller_bagimli_varlik_id",
                table: "personeller",
                column: "bagimli_varlik_id");

            migrationBuilder.CreateIndex(
                name: "ix_personeller_bilgi_sinifi_id",
                table: "personeller",
                column: "bilgi_sinifi_id");

            migrationBuilder.CreateIndex(
                name: "ix_personeller_butunluk_id",
                table: "personeller",
                column: "butunluk_id");

            migrationBuilder.CreateIndex(
                name: "ix_personeller_durum_id",
                table: "personeller",
                column: "durum_id");

            migrationBuilder.CreateIndex(
                name: "ix_personeller_erisilebilirlik_id",
                table: "personeller",
                column: "erisilebilirlik_id");

            migrationBuilder.CreateIndex(
                name: "ix_personeller_etkilenen_kisi_sayisi_id",
                table: "personeller",
                column: "etkilenen_kisi_sayisi_id");

            migrationBuilder.CreateIndex(
                name: "ix_personeller_gizlilik_id",
                table: "personeller",
                column: "gizlilik_id");

            migrationBuilder.CreateIndex(
                name: "ix_personeller_kategori_id",
                table: "personeller",
                column: "kategori_id");

            migrationBuilder.CreateIndex(
                name: "ix_personeller_kurumsal_sonuc_id",
                table: "personeller",
                column: "kurumsal_sonuc_id");

            migrationBuilder.CreateIndex(
                name: "ix_personeller_sektorel_etki_id",
                table: "personeller",
                column: "sektorel_etki_id");

            migrationBuilder.CreateIndex(
                name: "ix_personeller_toplumsal_sonuc_id",
                table: "personeller",
                column: "toplumsal_sonuc_id");

            migrationBuilder.CreateIndex(
                name: "ix_surecler_alt_kategori_id",
                table: "surecler",
                column: "alt_kategori_id");

            migrationBuilder.CreateIndex(
                name: "ix_surecler_bagimli_varlik_id",
                table: "surecler",
                column: "bagimli_varlik_id");

            migrationBuilder.CreateIndex(
                name: "ix_surecler_bilgi_sinifi_id",
                table: "surecler",
                column: "bilgi_sinifi_id");

            migrationBuilder.CreateIndex(
                name: "ix_surecler_butunluk_id",
                table: "surecler",
                column: "butunluk_id");

            migrationBuilder.CreateIndex(
                name: "ix_surecler_durum_id",
                table: "surecler",
                column: "durum_id");

            migrationBuilder.CreateIndex(
                name: "ix_surecler_erisilebilirlik_id",
                table: "surecler",
                column: "erisilebilirlik_id");

            migrationBuilder.CreateIndex(
                name: "ix_surecler_etkilenen_kisi_sayisi_id",
                table: "surecler",
                column: "etkilenen_kisi_sayisi_id");

            migrationBuilder.CreateIndex(
                name: "ix_surecler_gizlilik_id",
                table: "surecler",
                column: "gizlilik_id");

            migrationBuilder.CreateIndex(
                name: "ix_surecler_kategori_id",
                table: "surecler",
                column: "kategori_id");

            migrationBuilder.CreateIndex(
                name: "ix_surecler_kurumsal_sonuc_id",
                table: "surecler",
                column: "kurumsal_sonuc_id");

            migrationBuilder.CreateIndex(
                name: "ix_surecler_sektorel_etki_id",
                table: "surecler",
                column: "sektorel_etki_id");

            migrationBuilder.CreateIndex(
                name: "ix_surecler_toplumsal_sonuc_id",
                table: "surecler",
                column: "toplumsal_sonuc_id");

            migrationBuilder.CreateIndex(
                name: "ix_tasinabilir_cihaz_ve_ortamlar_alt_kategori_id",
                table: "tasinabilir_cihaz_ve_ortamlar",
                column: "alt_kategori_id");

            migrationBuilder.CreateIndex(
                name: "ix_tasinabilir_cihaz_ve_ortamlar_anahtar_sorumlusu_id",
                table: "tasinabilir_cihaz_ve_ortamlar",
                column: "anahtar_sorumlusu_id");

            migrationBuilder.CreateIndex(
                name: "ix_tasinabilir_cihaz_ve_ortamlar_bagimli_varlik_id",
                table: "tasinabilir_cihaz_ve_ortamlar",
                column: "bagimli_varlik_id");

            migrationBuilder.CreateIndex(
                name: "ix_tasinabilir_cihaz_ve_ortamlar_bilgi_sinifi_id",
                table: "tasinabilir_cihaz_ve_ortamlar",
                column: "bilgi_sinifi_id");

            migrationBuilder.CreateIndex(
                name: "ix_tasinabilir_cihaz_ve_ortamlar_butunluk_id",
                table: "tasinabilir_cihaz_ve_ortamlar",
                column: "butunluk_id");

            migrationBuilder.CreateIndex(
                name: "ix_tasinabilir_cihaz_ve_ortamlar_durum_id",
                table: "tasinabilir_cihaz_ve_ortamlar",
                column: "durum_id");

            migrationBuilder.CreateIndex(
                name: "ix_tasinabilir_cihaz_ve_ortamlar_erisilebilirlik_id",
                table: "tasinabilir_cihaz_ve_ortamlar",
                column: "erisilebilirlik_id");

            migrationBuilder.CreateIndex(
                name: "ix_tasinabilir_cihaz_ve_ortamlar_etkilenen_kisi_sayisi_id",
                table: "tasinabilir_cihaz_ve_ortamlar",
                column: "etkilenen_kisi_sayisi_id");

            migrationBuilder.CreateIndex(
                name: "ix_tasinabilir_cihaz_ve_ortamlar_gizlilik_id",
                table: "tasinabilir_cihaz_ve_ortamlar",
                column: "gizlilik_id");

            migrationBuilder.CreateIndex(
                name: "ix_tasinabilir_cihaz_ve_ortamlar_kategori_id",
                table: "tasinabilir_cihaz_ve_ortamlar",
                column: "kategori_id");

            migrationBuilder.CreateIndex(
                name: "ix_tasinabilir_cihaz_ve_ortamlar_kriptoloji_turu_id",
                table: "tasinabilir_cihaz_ve_ortamlar",
                column: "kriptoloji_turu_id");

            migrationBuilder.CreateIndex(
                name: "ix_tasinabilir_cihaz_ve_ortamlar_kurumsal_sonuc_id",
                table: "tasinabilir_cihaz_ve_ortamlar",
                column: "kurumsal_sonuc_id");

            migrationBuilder.CreateIndex(
                name: "ix_tasinabilir_cihaz_ve_ortamlar_lisans_takip_sorumlusu_id",
                table: "tasinabilir_cihaz_ve_ortamlar",
                column: "lisans_takip_sorumlusu_id");

            migrationBuilder.CreateIndex(
                name: "ix_tasinabilir_cihaz_ve_ortamlar_sektorel_etki_id",
                table: "tasinabilir_cihaz_ve_ortamlar",
                column: "sektorel_etki_id");

            migrationBuilder.CreateIndex(
                name: "ix_tasinabilir_cihaz_ve_ortamlar_toplumsal_sonuc_id",
                table: "tasinabilir_cihaz_ve_ortamlar",
                column: "toplumsal_sonuc_id");

            migrationBuilder.CreateIndex(
                name: "ix_tasinabilir_cihaz_ve_ortamlar_yedekleme_sorumlusu_id",
                table: "tasinabilir_cihaz_ve_ortamlar",
                column: "yedekleme_sorumlusu_id");

            migrationBuilder.CreateIndex(
                name: "ix_tasinabilir_cihaz_ve_ortamlar_yedekleme_tipi_id",
                table: "tasinabilir_cihaz_ve_ortamlar",
                column: "yedekleme_tipi_id");

            migrationBuilder.CreateIndex(
                name: "ix_uygulamalar_alt_kategori_id",
                table: "uygulamalar",
                column: "alt_kategori_id");

            migrationBuilder.CreateIndex(
                name: "ix_uygulamalar_anahtar_sorumlusu_id",
                table: "uygulamalar",
                column: "anahtar_sorumlusu_id");

            migrationBuilder.CreateIndex(
                name: "ix_uygulamalar_bagimli_varlik_id",
                table: "uygulamalar",
                column: "bagimli_varlik_id");

            migrationBuilder.CreateIndex(
                name: "ix_uygulamalar_bilgi_sinifi_id",
                table: "uygulamalar",
                column: "bilgi_sinifi_id");

            migrationBuilder.CreateIndex(
                name: "ix_uygulamalar_butunluk_id",
                table: "uygulamalar",
                column: "butunluk_id");

            migrationBuilder.CreateIndex(
                name: "ix_uygulamalar_destek_durumu_id",
                table: "uygulamalar",
                column: "destek_durumu_id");

            migrationBuilder.CreateIndex(
                name: "ix_uygulamalar_durum_id",
                table: "uygulamalar",
                column: "durum_id");

            migrationBuilder.CreateIndex(
                name: "ix_uygulamalar_erisilebilirlik_id",
                table: "uygulamalar",
                column: "erisilebilirlik_id");

            migrationBuilder.CreateIndex(
                name: "ix_uygulamalar_etkilenen_kisi_sayisi_id",
                table: "uygulamalar",
                column: "etkilenen_kisi_sayisi_id");

            migrationBuilder.CreateIndex(
                name: "ix_uygulamalar_gizlilik_id",
                table: "uygulamalar",
                column: "gizlilik_id");

            migrationBuilder.CreateIndex(
                name: "ix_uygulamalar_kategori_id",
                table: "uygulamalar",
                column: "kategori_id");

            migrationBuilder.CreateIndex(
                name: "ix_uygulamalar_kriptoloji_turu_id",
                table: "uygulamalar",
                column: "kriptoloji_turu_id");

            migrationBuilder.CreateIndex(
                name: "ix_uygulamalar_kurumsal_sonuc_id",
                table: "uygulamalar",
                column: "kurumsal_sonuc_id");

            migrationBuilder.CreateIndex(
                name: "ix_uygulamalar_lisans_takip_sorumlusu_id",
                table: "uygulamalar",
                column: "lisans_takip_sorumlusu_id");

            migrationBuilder.CreateIndex(
                name: "ix_uygulamalar_sektorel_etki_id",
                table: "uygulamalar",
                column: "sektorel_etki_id");

            migrationBuilder.CreateIndex(
                name: "ix_uygulamalar_toplumsal_sonuc_id",
                table: "uygulamalar",
                column: "toplumsal_sonuc_id");

            migrationBuilder.CreateIndex(
                name: "ix_uygulamalar_yedekleme_sorumlusu_id",
                table: "uygulamalar",
                column: "yedekleme_sorumlusu_id");

            migrationBuilder.CreateIndex(
                name: "ix_uygulamalar_yedekleme_tipi_id",
                table: "uygulamalar",
                column: "yedekleme_tipi_id");

            migrationBuilder.CreateIndex(
                name: "ix_veritabanlari_alt_kategori_id",
                table: "veritabanlari",
                column: "alt_kategori_id");

            migrationBuilder.CreateIndex(
                name: "ix_veritabanlari_anahtar_sorumlusu_id",
                table: "veritabanlari",
                column: "anahtar_sorumlusu_id");

            migrationBuilder.CreateIndex(
                name: "ix_veritabanlari_bagimli_varlik_id",
                table: "veritabanlari",
                column: "bagimli_varlik_id");

            migrationBuilder.CreateIndex(
                name: "ix_veritabanlari_bilgi_sinifi_id",
                table: "veritabanlari",
                column: "bilgi_sinifi_id");

            migrationBuilder.CreateIndex(
                name: "ix_veritabanlari_butunluk_id",
                table: "veritabanlari",
                column: "butunluk_id");

            migrationBuilder.CreateIndex(
                name: "ix_veritabanlari_durum_id",
                table: "veritabanlari",
                column: "durum_id");

            migrationBuilder.CreateIndex(
                name: "ix_veritabanlari_erisilebilirlik_id",
                table: "veritabanlari",
                column: "erisilebilirlik_id");

            migrationBuilder.CreateIndex(
                name: "ix_veritabanlari_etkilenen_kisi_sayisi_id",
                table: "veritabanlari",
                column: "etkilenen_kisi_sayisi_id");

            migrationBuilder.CreateIndex(
                name: "ix_veritabanlari_gizlilik_id",
                table: "veritabanlari",
                column: "gizlilik_id");

            migrationBuilder.CreateIndex(
                name: "ix_veritabanlari_kategori_id",
                table: "veritabanlari",
                column: "kategori_id");

            migrationBuilder.CreateIndex(
                name: "ix_veritabanlari_kurumsal_sonuc_id",
                table: "veritabanlari",
                column: "kurumsal_sonuc_id");

            migrationBuilder.CreateIndex(
                name: "ix_veritabanlari_sektorel_etki_id",
                table: "veritabanlari",
                column: "sektorel_etki_id");

            migrationBuilder.CreateIndex(
                name: "ix_veritabanlari_toplumsal_sonuc_id",
                table: "veritabanlari",
                column: "toplumsal_sonuc_id");

            migrationBuilder.CreateIndex(
                name: "ix_veritabanlari_yedekleme_sorumlusu_id",
                table: "veritabanlari",
                column: "yedekleme_sorumlusu_id");

            migrationBuilder.CreateIndex(
                name: "ix_veritabanlari_yedekleme_tipi_id",
                table: "veritabanlari",
                column: "yedekleme_tipi_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "agve_sistemler");

            migrationBuilder.DropTable(
                name: "basili_bilgiler");

            migrationBuilder.DropTable(
                name: "birimler");

            migrationBuilder.DropTable(
                name: "elektronik_bilgiler");

            migrationBuilder.DropTable(
                name: "eposta_talepleri");

            migrationBuilder.DropTable(
                name: "fiziksel_mekanlar");

            migrationBuilder.DropTable(
                name: "guvenlik_modu");

            migrationBuilder.DropTable(
                name: "iot_cihazlari");

            migrationBuilder.DropTable(
                name: "konumlar");

            migrationBuilder.DropTable(
                name: "kriptografi_envanterleri");

            migrationBuilder.DropTable(
                name: "kullanici_birimler");

            migrationBuilder.DropTable(
                name: "kullanici_roller");

            migrationBuilder.DropTable(
                name: "logs");

            migrationBuilder.DropTable(
                name: "personeller");

            migrationBuilder.DropTable(
                name: "surecler");

            migrationBuilder.DropTable(
                name: "tasinabilir_cihaz_ve_ortamlar");

            migrationBuilder.DropTable(
                name: "uygulamalar");

            migrationBuilder.DropTable(
                name: "veritabanlari");

            migrationBuilder.DropTable(
                name: "kurumlar");

            migrationBuilder.DropTable(
                name: "kullanim_seviyeleri");

            migrationBuilder.DropTable(
                name: "kullanicilar");

            migrationBuilder.DropTable(
                name: "roller");

            migrationBuilder.DropTable(
                name: "destek_durumlari");

            migrationBuilder.DropTable(
                name: "kriptoloji_turleri");

            migrationBuilder.DropTable(
                name: "lisans_takip_sorumlulari");

            migrationBuilder.DropTable(
                name: "anahtar_sorumlulari");

            migrationBuilder.DropTable(
                name: "bagimli_varliklar");

            migrationBuilder.DropTable(
                name: "bilgi_siniflari");

            migrationBuilder.DropTable(
                name: "butunlukler");

            migrationBuilder.DropTable(
                name: "durumlar");

            migrationBuilder.DropTable(
                name: "erisilebilirlikler");

            migrationBuilder.DropTable(
                name: "etkilenen_kisi_sayilari");

            migrationBuilder.DropTable(
                name: "gizlilikler");

            migrationBuilder.DropTable(
                name: "kategoriler");

            migrationBuilder.DropTable(
                name: "kurumsal_sonuclar");

            migrationBuilder.DropTable(
                name: "sektorel_etkiler");

            migrationBuilder.DropTable(
                name: "toplumsal_sonuclar");

            migrationBuilder.DropTable(
                name: "yedekleme_sorumlulari");

            migrationBuilder.DropTable(
                name: "yedekleme_tipleri");
        }
    }
}
