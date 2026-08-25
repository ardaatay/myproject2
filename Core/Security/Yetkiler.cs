namespace Core.Security;

/// <summary>
/// Yetkilendirme politikalarının adları. Denetleyicilerde rol adları yerine
/// bunlar kullanılır; hangi rolün hangi yetkiye sahip olduğu konfigürasyondan
/// gelir. Böylece her kurum kendi rol adlarını kullanabilir.
/// </summary>
public static class Yetkiler
{
    // Teknik varlıklar: ağ ve sistemler, uygulamalar, taşınabilir cihazlar,
    // IoT cihazları, fiziksel mekanlar, personel.
    public const string TeknikVarlikListele = "TeknikVarlik.Listele";
    public const string TeknikVarlikGoruntule = "TeknikVarlik.Goruntule";
    public const string TeknikVarlikDuzenle = "TeknikVarlik.Duzenle";
    public const string TeknikVarlikOlustur = "TeknikVarlik.Olustur";

    // Bilgi varlıkları: basılı ve elektronik bilgiler, süreçler, veritabanları.
    public const string BilgiVarlikListele = "BilgiVarlik.Listele";
    public const string BilgiVarlikGoruntule = "BilgiVarlik.Goruntule";
    public const string BilgiVarlikDuzenle = "BilgiVarlik.Duzenle";
    public const string BilgiVarlikOlustur = "BilgiVarlik.Olustur";

    // Kriptografi envanteri kendi rol kümesini kullanır.
    public const string KriptoGoruntule = "Kripto.Goruntule";
    public const string KriptoDuzenle = "Kripto.Duzenle";
    public const string KriptoOlustur = "Kripto.Olustur";

    // E-posta talepleri: listeleme ile diğer işlemler farklı rollere açıktır.
    public const string EpostaTalepListele = "EpostaTalep.Listele";
    public const string EpostaTalepYonet = "EpostaTalep.Yonet";

    /// <summary>Tanımlama ekranları, raporlar, kullanıcı ve birim yönetimi, kalıcı silme.</summary>
    public const string SistemYonet = "Sistem.Yonet";

    public static IReadOnlyList<string> Tumu =>
    [
        TeknikVarlikListele, TeknikVarlikGoruntule, TeknikVarlikDuzenle, TeknikVarlikOlustur,
        BilgiVarlikListele, BilgiVarlikGoruntule, BilgiVarlikDuzenle, BilgiVarlikOlustur,
        KriptoGoruntule, KriptoDuzenle, KriptoOlustur,
        EpostaTalepListele, EpostaTalepYonet,
        SistemYonet
    ];
}
