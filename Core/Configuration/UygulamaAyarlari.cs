using Core.Security;

namespace Core.Configuration;

/// <summary>
/// Kuruma göre değişen uygulama ayarları. <c>appsettings.json</c> içindeki
/// <c>UygulamaAyarlari</c> bölümünden bağlanır; her alan ortam değişkeniyle
/// geçersiz kılınabilir (<c>UygulamaAyarlari__UygulamaAdi</c> gibi).
/// </summary>
public class UygulamaAyarlari
{
    public const string BolumAdi = "UygulamaAyarlari";

    /// <summary>Tarayıcı başlığında ve üst çubukta görünen ad.</summary>
    public string UygulamaAdi { get; set; } = "Varlık Envanteri";

    /// <summary>wwwroot köküne göre logo yolu. Favicon olarak da kullanılır.</summary>
    public string LogoYolu { get; set; } = "/img/logo.png";

    /// <summary>Sayı, tarih ve harf büyütme kurallarını belirleyen kültür.</summary>
    public string Kultur { get; set; } = "tr-TR";

    /// <summary>Kısa tarih biçimi. Boş bırakılırsa kültürün varsayılanı kullanılır.</summary>
    public string? TarihFormati { get; set; } = "dd.MM.yyyy";

    /// <summary>Oturum ve kimlik çerezinin geçerlilik süresi.</summary>
    public int OturumSuresiDk { get; set; } = 60;

    /// <summary>
    /// Yazılabilir veri dizini: kullanıcı istatistikleri ve DataProtection
    /// anahtarları buraya yazılır. Boşsa uygulama dizini altındaki App_Data kullanılır.
    /// </summary>
    public string? VeriDizini { get; set; }

    public SifrePolitikasi SifrePolitikasi { get; set; } = new();
}
