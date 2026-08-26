using System.ComponentModel.DataAnnotations;
using Dto.Kullanici.Enum;

namespace Dto.Kullanici;

/// <summary>
/// Yönetim ekranındaki kullanıcı düzenleme formu. Liste kullanıcı-birim
/// kayıtları üzerinden kurulduğu için hem kullanıcının hem de o kaydın
/// kimliğini taşır.
/// </summary>
public class KullaniciDuzenleDto : IValidatableObject
{
    /// <summary>Listedeki satırın (kullanıcı-birim kaydının) kimliği.</summary>
    public int KullaniciBirimId { get; set; }

    public int KullaniciId { get; set; }

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

    /// <summary>Yöntem değişikliğinin sonucunu ekranda uyarı olarak göstermek için.</summary>
    public GirisYontemi MevcutGirisYontemi { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (BirimId <= 0)
            yield return new ValidationResult("Birim seçilmelidir.", [nameof(BirimId)]);
    }
}
