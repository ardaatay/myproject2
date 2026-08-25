namespace Core.Security;

/// <summary>
/// Şifre kuralları. Değerler konfigürasyondan bağlanır
/// (<c>UygulamaAyarlari:SifrePolitikasi</c>).
/// </summary>
public class SifrePolitikasi
{
    public int EnAzUzunluk { get; set; } = 10;
    public bool BuyukHarfGerekli { get; set; } = true;
    public bool KucukHarfGerekli { get; set; } = true;
    public bool RakamGerekli { get; set; } = true;
    public bool OzelKarakterGerekli { get; set; } = true;

    /// <summary>Eşiğe ulaşan başarısız giriş sayısında hesap kilitlenir.</summary>
    public int KilitEsigi { get; set; } = 5;

    /// <summary>Kilit süresi (dakika).</summary>
    public int KilitSuresiDk { get; set; } = 15;

    /// <summary>Kurala uymayan her madde için bir hata döner; liste boşsa şifre geçerlidir.</summary>
    public IReadOnlyList<string> Dogrula(string? sifre)
    {
        var hatalar = new List<string>();

        if (string.IsNullOrWhiteSpace(sifre))
        {
            hatalar.Add("Şifre boş olamaz.");
            return hatalar;
        }

        if (sifre.Length < EnAzUzunluk)
            hatalar.Add($"Şifre en az {EnAzUzunluk} karakter olmalıdır.");

        if (BuyukHarfGerekli && !sifre.Any(char.IsUpper))
            hatalar.Add("Şifre en az bir büyük harf içermelidir.");

        if (KucukHarfGerekli && !sifre.Any(char.IsLower))
            hatalar.Add("Şifre en az bir küçük harf içermelidir.");

        if (RakamGerekli && !sifre.Any(char.IsDigit))
            hatalar.Add("Şifre en az bir rakam içermelidir.");

        if (OzelKarakterGerekli && sifre.All(char.IsLetterOrDigit))
            hatalar.Add("Şifre en az bir özel karakter içermelidir.");

        return hatalar;
    }
}
