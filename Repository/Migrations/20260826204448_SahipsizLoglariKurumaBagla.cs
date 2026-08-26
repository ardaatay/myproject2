using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <summary>
    /// Şema değişmez; yalnızca veri düzeltir.
    ///
    /// Kiracı kimliği yazılmadan oluşmuş log kayıtları <c>organizasyon_id = 0</c>
    /// taşır ve kiracı sorgu filtresi yüzünden yönetim ekranlarında hiç
    /// görünmez. Bunlar iki kaynaktan gelir: bu sürümden önce yazılan tüm
    /// işlem logları (kiracı hiç yazılmıyordu) ve oturum açılmadan oluşan
    /// kayıtlar.
    ///
    /// Dağıtım tek kurumlu olduğu için kayıtlar o organizasyona bağlanır.
    /// Birden fazla organizasyon varsa hangisine ait oldukları bilinemez;
    /// güncelleme o durumda hiçbir satıra dokunmaz.
    /// </summary>
    public partial class SahipsizLoglariKurumaBagla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                UPDATE logs
                SET organizasyon_id = (SELECT id FROM organizasyonlar LIMIT 1)
                WHERE organizasyon_id = 0
                  AND (SELECT COUNT(*) FROM organizasyonlar) = 1;
                """);

            migrationBuilder.Sql(
                """
                UPDATE hata_loglari
                SET organizasyon_id = (SELECT id FROM organizasyonlar LIMIT 1)
                WHERE organizasyon_id = 0
                  AND (SELECT COUNT(*) FROM organizasyonlar) = 1;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Geri alınamaz: hangi kayıtların baştan sahipsiz olduğu bilgisi
            // güncellemeden sonra kalmaz. Şema değişmediği için geri almanın
            // bir karşılığı da yoktur.
        }
    }
}
