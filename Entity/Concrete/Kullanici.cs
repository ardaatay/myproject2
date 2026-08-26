using System.ComponentModel.DataAnnotations;
using Core.Entity;
using Dto.Kullanici.Enum;

namespace Entity.Concrete
{
    public class Kullanici : IEntity<int>, IKiraciEntity
    {
        public int Id { get; set; }

        /// <summary>Kaydın ait olduğu kiracı.</summary>
        public int OrganizasyonId { get; set; }

        [MaxLength(100)] public string Username { get; set; } = default!;

        /// <summary>
        /// Kimliğin nerede doğrulanacağı. Kullanıcı bazındadır: aynı kurumda
        /// yerel hesaplarla dizin hesapları bir arada bulunabilir.
        /// </summary>
        public GirisYontemi GirisYontemi { get; set; } = GirisYontemi.Yerel;

        /// <summary>
        /// Dizindeki hesap adı. Boşsa <see cref="Username"/> kullanılır; yalnızca
        /// uygulamadaki kullanıcı adı dizindekinden farklıysa doldurulur.
        /// </summary>
        [MaxLength(255)] public string? ActiveDirectoryKullaniciAdi { get; set; }

        [MaxLength(200)] public string? AdSoyad { get; set; }

        [MaxLength(200)] public string? Eposta { get; set; }

        /// <summary>
        /// PBKDF2 türevi karma. Boş bırakılırsa kullanıcı henüz şifre belirlememiştir
        /// ve giriş yapamaz. Dizine bağlı hesaplarda her zaman boş kalır — dizin
        /// şifresi uygulamada hiçbir biçimde tutulmaz.
        /// </summary>
        [MaxLength(500)]
        public string? PasswordHash { get; set; }

        /// <summary>
        /// Şifre her değiştiğinde yenilenir. Oturum çerezindeki değer buradakiyle
        /// eşleşmezse oturum düşürülür; böylece şifre değişince açık kalan diğer
        /// oturumlar geçersizleşir.
        /// </summary>
        [MaxLength(50)]
        public string? SecurityStamp { get; set; }

        /// <summary>İlk girişte veya yönetici sıfırlamasından sonra şifre değiştirme zorunluluğu.</summary>
        public bool SifreDegistirmeliMi { get; set; }

        public DateTime? SonGirisTarihi { get; set; }

        /// <summary>Başarılı girişte sıfırlanır. Eşik aşılınca hesap geçici olarak kilitlenir.</summary>
        public int BasarisizGirisSayisi { get; set; }

        /// <summary>Dolu ve gelecekteyse hesap kilitlidir.</summary>
        public DateTime? KilitBitisTarihi { get; set; }

        public int BirimId { get; set; }
        [MaxLength(500)] public string BirimAd { get; set; } = default!;
        public bool Durum { get; set; }

        public ICollection<KullaniciRol> KullaniciRoller { get; set; } = new List<KullaniciRol>();

        /// <summary>Dizinde aranırken kullanılacak hesap adı.</summary>
        public string DizinKullaniciAdi =>
            string.IsNullOrWhiteSpace(ActiveDirectoryKullaniciAdi) ? Username : ActiveDirectoryKullaniciAdi;
    }
}
