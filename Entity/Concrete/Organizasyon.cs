using System.ComponentModel.DataAnnotations;
using Core.Entity;

namespace Entity.Concrete;

/// <summary>
/// Kiracı: uygulamayı kullanan kurum. Verinin sahibi budur.
///
/// Mevcut <see cref="Kurum"/> ile karıştırılmamalıdır — o, e-posta taleplerinde
/// "bu talep hangi kurumla ilgili" sorusunu yanıtlayan ve üçüncü taraf kurumları
/// da içerebilen ortak bir referans listesidir.
/// </summary>
public class Organizasyon : IEntity<int>
{
    public int Id { get; set; }

    [MaxLength(200)] public string Ad { get; set; } = null!;

    /// <summary>Kısa, benzersiz tanımlayıcı. Alt alan adı veya dağıtım anahtarı olarak kullanılabilir.</summary>
    [MaxLength(50)]
    public string? Kod { get; set; }

    [MaxLength(500)] public string? LogoUrl { get; set; }

    /// <summary>Organizasyona özgü serbest ayarlar (JSON).</summary>
    public string? Ayarlar { get; set; }

    public bool Durum { get; set; }

    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public DateTime? DeletedDate { get; set; }
}
