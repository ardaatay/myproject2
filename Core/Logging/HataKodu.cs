using System.Security.Cryptography;
using System.Text;

namespace Core.Logging;

/// <summary>
/// Kullanıcıya gösterilen kısa hata referansı.
///
/// Amaç, kullanıcının teknik ayrıntıyı görmeden yöneticiye iletebileceği bir
/// tutamak vermek: kod hata kaydına birebir bağlıdır ve yönetim ekranından
/// aranarak tam bilgiye ulaşılır. Bu yüzden telefonda okunabilecek kadar kısa
/// ve okunaklıdır — karıştırılan karakterler (I, L, O, U, 0, 1) alfabede yoktur.
/// </summary>
public static class HataKodu
{
    /// <summary>İstek boyunca üretilen kodun <c>HttpContext.Items</c> içindeki anahtarı.</summary>
    public const string OgeAnahtari = "VarlikEnvanteri.HataKodu";

    public const string Onek = "HTA";

    /// <summary>Kodun veritabanındaki kolon uzunluğu (HTA-XXXX-XXXX).</summary>
    public const int Uzunluk = 13;

    private const string Alfabe = "23456789ABCDEFGHJKMNPQRSTVWXYZ";
    private const int KarakterSayisi = 8;

    /// <summary>Yeni bir kod üretir: <c>HTA-K7F4-9QXZ</c>.</summary>
    public static string Uret()
    {
        var karakterler = new char[KarakterSayisi];

        for (var i = 0; i < KarakterSayisi; i++)
            karakterler[i] = Alfabe[RandomNumberGenerator.GetInt32(Alfabe.Length)];

        return $"{Onek}-{new string(karakterler, 0, 4)}-{new string(karakterler, 4, 4)}";
    }

    /// <summary>
    /// Kullanıcının yazdığı kodu kanonik biçime getirir. Boşluk, tire ve küçük
    /// harf farkları göz ardı edilir; ön ek yazılmasa da bulunur. Kod tanınmazsa
    /// <c>null</c> döner ve arama yapılmaz.
    /// </summary>
    public static string? Duzelt(string? girdi)
    {
        if (string.IsNullOrWhiteSpace(girdi))
            return null;

        var temiz = new StringBuilder(KarakterSayisi);

        foreach (var karakter in girdi.ToUpperInvariant())
        {
            if (Alfabe.Contains(karakter))
                temiz.Append(karakter);
        }

        // "HTA" ön ekindeki harfler de alfabede olduğu için temizlenen dizinin
        // başında kalır; kod bunun ardından gelir.
        var metin = temiz.ToString();

        if (metin.StartsWith(Onek, StringComparison.Ordinal) && metin.Length > KarakterSayisi)
            metin = metin[Onek.Length..];

        return metin.Length == KarakterSayisi
            ? $"{Onek}-{metin[..4]}-{metin[4..]}"
            : null;
    }
}
