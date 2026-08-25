namespace Core.Security;

public enum SifreDogrulamaSonucu
{
    Basarisiz,
    Basarili,

    /// <summary>
    /// Şifre doğru, ancak karma eski bir algoritma/iterasyon sayısıyla üretilmiş.
    /// Çağıran taraf düz metin elindeyken karmayı yenilemelidir.
    /// </summary>
    BasariliYenilenmeli
}

public interface ISifreKoruyucu
{
    string Karmala(string sifre);
    SifreDogrulamaSonucu Dogrula(string? karma, string sifre);
}
