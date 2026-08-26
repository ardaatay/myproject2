using System.ComponentModel.DataAnnotations;
using Dto.Kullanici.Enum;

namespace Dto.Kullanici
{
    public class UpdateKullaniciDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
        [MaxLength(100)]
        [Display(Name = "Kullanıcı adı")]
        public string Username { get; set; } = default!;

        [Display(Name = "Giriş yöntemi")]
        public GirisYontemi GirisYontemi { get; set; }

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
        public bool Durum { get; set; }
    }
}
