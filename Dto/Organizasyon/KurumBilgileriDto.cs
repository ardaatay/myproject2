using System.ComponentModel.DataAnnotations;

namespace Dto.Organizasyon;

/// <summary>
/// Uygulamayı kullanan kurumun kendi kaydı — verinin sahibi olan kiracı.
///
/// E-posta taleplerinde referans verilen <c>Kurum</c> listesiyle karıştırılmamalıdır:
/// o liste üçüncü taraf kurumları da içerir, bu kayıt ise kurulumun kendisidir.
/// </summary>
public class KurumBilgileriDto : IValidatableObject
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Kurum adı zorunludur.")]
    [MaxLength(200)]
    [Display(Name = "Kurum adı")]
    public string Ad { get; set; } = null!;

    [MaxLength(50)]
    [Display(Name = "Kısa kod")]
    public string? Kod { get; set; }

    [MaxLength(500)]
    [Display(Name = "Logo yolu")]
    public string? LogoUrl { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(LogoUrl))
            yield break;

        var deger = LogoUrl.Trim();

        // Değer doğrudan img/link etiketlerine yazılıyor; yalnızca site içi bir
        // yol ya da http(s) adresi kabul edilir. Aksi halde "javascript:" gibi
        // bir şema sayfaya sızabilir.
        var gecerli = deger.StartsWith('/') ||
                      (Uri.TryCreate(deger, UriKind.Absolute, out var adres) &&
                       (adres.Scheme == Uri.UriSchemeHttp || adres.Scheme == Uri.UriSchemeHttps));

        if (!gecerli)
        {
            yield return new ValidationResult(
                "Logo yolu / ile başlayan site içi bir yol ya da http(s) adresi olmalıdır.",
                [nameof(LogoUrl)]);
        }
    }
}

/// <summary>
/// Başlıkta ve giriş ekranında görünen kimlik. Kurum kaydı doldurulmamışsa
/// uygulama ayarlarındaki değerlere düşülür.
/// </summary>
public class KurumKimligiDto
{
    public string Ad { get; set; } = null!;
    public string LogoYolu { get; set; } = null!;
}
