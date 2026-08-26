using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class BirimNestedSet : Migration
    {
        /// <summary>
        /// Mevcut komşuluk listesinden (ust_id) nested set sınırlarını üretir.
        ///
        /// Yöntem, ağacı bir "açılış/kapanış olayı" akışına çevirmektir: ön sıralı
        /// gezinmede her düğüm bir kez açılır, alt ağacı bittiğinde bir kez kapanır;
        /// bu olayların sıra numaraları doğrudan sol ve sag değerleridir.
        ///
        /// Olayların sırası, her düğüm için kökten kendisine inen kardeş sıraları
        /// dizisiyle (anahtar) belirlenir. Dizi tamsayı olduğu için karşılaştırma
        /// harmanlamadan bağımsızdır; kapanış olayına eklenen 2147483647 sentineli
        /// de düğümün kapanışını tüm alt ağacının ardına iter (kardes_sirasi asla
        /// bu değere ulaşamaz).
        ///
        /// Kardeş sırası (sira, ad, id) düzenindedir ve uygulamadaki yeniden
        /// numaralandırma da aynı düzeni aynı veritabanı harmanlamasıyla kurar;
        /// bu yüzden geri doldurma ile çalışma zamanı birbirinden ayrışmaz.
        ///
        /// Kök sırası ayrı hesaplanır: üst birimi kendi kiracısında bulunmayan
        /// artıklar da kök gibi ele alındığı için sıraları kendi ust_id
        /// bölümlerinde değil köklerin ortak bölümünde verilmelidir. Aksi halde
        /// bir artık ile gerçek bir kök aynı anahtarı alır, aralıkları iç içe
        /// geçer ve alt ağaç sorguları yabancı dalları da toplar.
        ///
        /// Derinlik sınırı ve organizasyon eşitliği koşulu bozuk veriye karşıdır:
        /// döngüsel bir ust_id bağı özyinelemeyi sonsuza sürüklemesin, kiracılar
        /// birbirinin ağacına karışmasın. Bu nedenle erişilemeyen satırlar
        /// sol = sag = 0 kalır; uygulama bunları kök gibi ele alır ve ilk yapısal
        /// değişiklikte numaralandırır.
        /// </summary>
        private const string SinirlariGeriDoldur = @"
WITH RECURSIVE siralar AS (
    SELECT b.id,
           b.organizasyon_id,
           b.ust_id,
           row_number() OVER (
               PARTITION BY b.organizasyon_id, b.ust_id
               ORDER BY b.sira, b.ad, b.id
           )::int AS kardes_sirasi
      FROM birimler b
),
kokler AS (
    SELECT b.id,
           b.organizasyon_id,
           row_number() OVER (
               PARTITION BY b.organizasyon_id
               ORDER BY b.sira, b.ad, b.id
           )::int AS kok_sirasi
      FROM birimler b
     WHERE b.ust_id IS NULL
        OR NOT EXISTS (
               SELECT 1
                 FROM birimler u
                WHERE u.id = b.ust_id
                  AND u.organizasyon_id = b.organizasyon_id
           )
),
agac AS (
    SELECT k.id, k.organizasyon_id, 0 AS seviye, ARRAY[k.kok_sirasi] AS anahtar
      FROM kokler k
    UNION ALL
    SELECT c.id, c.organizasyon_id, a.seviye + 1, a.anahtar || c.kardes_sirasi
      FROM siralar c
      JOIN agac a
        ON c.ust_id = a.id
       AND c.organizasyon_id = a.organizasyon_id
     WHERE a.seviye < 64
),
olaylar AS (
    SELECT id, organizasyon_id, seviye, anahtar, true AS acilis
      FROM agac
    UNION ALL
    SELECT id, organizasyon_id, seviye, anahtar || 2147483647, false
      FROM agac
),
numaralar AS (
    SELECT id,
           seviye,
           acilis,
           row_number() OVER (PARTITION BY organizasyon_id ORDER BY anahtar)::int AS numara
      FROM olaylar
),
ozet AS (
    SELECT id,
           max(seviye) AS seviye,
           max(numara) FILTER (WHERE acilis)     AS sol,
           max(numara) FILTER (WHERE NOT acilis) AS sag
      FROM numaralar
     GROUP BY id
)
UPDATE birimler b
   SET sol = o.sol,
       sag = o.sag,
       seviye = o.seviye
  FROM ozet o
 WHERE b.id = o.id;
";

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "sag",
                table: "birimler",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "sol",
                table: "birimler",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(SinirlariGeriDoldur);

            migrationBuilder.CreateIndex(
                name: "ix_birimler_organizasyon_id_sag",
                table: "birimler",
                columns: new[] { "organizasyon_id", "sag" });

            migrationBuilder.CreateIndex(
                name: "ix_birimler_organizasyon_id_sol",
                table: "birimler",
                columns: new[] { "organizasyon_id", "sol" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // sol ve sag türev sütunlardır: ust_id komşuluk bilgisi yerinde
            // kaldığı için düşürülmeleri veri kaybı değildir.
            migrationBuilder.DropIndex(
                name: "ix_birimler_organizasyon_id_sag",
                table: "birimler");

            migrationBuilder.DropIndex(
                name: "ix_birimler_organizasyon_id_sol",
                table: "birimler");

            migrationBuilder.DropColumn(
                name: "sag",
                table: "birimler");

            migrationBuilder.DropColumn(
                name: "sol",
                table: "birimler");
        }
    }
}
