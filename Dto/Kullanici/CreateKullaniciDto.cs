using System.ComponentModel.DataAnnotations;
using Dto.Kullanici.Enum;

namespace Dto.Kullanici
{
    public class CreateKullaniciDto : IValidatableObject
    {
        [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
        [MaxLength(100)]
        [Display(Name = "Kullanıcı adı")]
        public string Username { get; set; } = default!;

        [Display(Name = "Giriş yöntemi")]
        public GirisYontemi GirisYontemi { get; set; } = GirisYontemi.Yerel;

        /// <summary>
        /// Dizindeki hesap adı. Boş bırakılırsa <see cref="Username"/> kullanılır;
        /// yalnızca uygulamadaki ad dizindekinden farklıysa doldurulur.
        /// </summary>
        [MaxLength(255)]
        [Display(Name = "Active Directory hesabı")]
        public string? ActiveDirectoryKullaniciAdi { get; set; }

        [MaxLength(200)]
        [Display(Name = "Ad soyad")]
        public string? AdSoyad { get; set; }

        [MaxLength(200)]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi girin.")]
        [Display(Name = "E-posta")]
        public string? Eposta { get; set; }

        [Display(Name = "Birim")]
        public int BirimId { get; set; }

        public string BirimAd { get; set; } = default!;

        [Display(Name = "Aktif")]
        public bool Durum { get; set; } = true;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (BirimId <= 0)
                yield return new ValidationResult("Birim seçilmelidir.", [nameof(BirimId)]);
        }
    }
}
