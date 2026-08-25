namespace Core.Security;

/// <summary>
/// Politika → rol eşlemesi. <c>appsettings.json</c> içindeki <c>Yetkilendirme</c>
/// bölümünden bağlanır. Konfigürasyonda belirtilmeyen politikalar aşağıdaki
/// varsayılanı kullanır; bu varsayılan, kuruma özgü rol adlarının koda gömülü
/// olduğu önceki davranışın birebir karşılığıdır.
/// </summary>
public class YetkiAyarlari
{
    public const string BolumAdi = "Yetkilendirme";

    /// <summary>Anahtar: politika adı (<see cref="Yetkiler"/>), değer: o yetkiye sahip roller.</summary>
    public Dictionary<string, List<string>> Politikalar { get; set; } = new();

    private static readonly Dictionary<string, string[]> Varsayilanlar = new()
    {
        [Yetkiler.TeknikVarlikListele]   = ["ADMIN", "BIGRADMINS", "BIGRUSERS", "OPOWNERS", "VERIGIRIS"],
        [Yetkiler.TeknikVarlikGoruntule] = ["ADMIN", "BIGRADMINS", "BIGRUSERS", "VERIGIRIS"],
        [Yetkiler.TeknikVarlikDuzenle]   = ["ADMIN", "BIGRADMINS", "OPOWNERS", "VERIGIRIS"],
        [Yetkiler.TeknikVarlikOlustur]   = ["ADMIN", "BIGRADMINS", "VERIGIRIS"],

        [Yetkiler.BilgiVarlikListele]    = ["ADMIN", "BGYSADMINS", "BGYSUSERS", "OPOWNERS", "VERIGIRIS"],
        [Yetkiler.BilgiVarlikGoruntule]  = ["ADMIN", "BGYSADMINS", "BGYSUSERS", "VERIGIRIS"],
        [Yetkiler.BilgiVarlikDuzenle]    = ["ADMIN", "BGYSADMINS", "OPOWNERS", "VERIGIRIS"],
        [Yetkiler.BilgiVarlikOlustur]    = ["ADMIN", "BGYSADMINS", "VERIGIRIS"],

        [Yetkiler.KriptoGoruntule]       = ["ADMIN", "BIGRADMINS", "BIGRUSERS"],
        [Yetkiler.KriptoDuzenle]         = ["ADMIN", "BIGRADMINS", "OPOWNERS"],
        [Yetkiler.KriptoOlustur]         = ["ADMIN", "BIGRADMINS"],

        [Yetkiler.EpostaTalepListele]    = ["ADMIN", "BIGRADMINS"],
        [Yetkiler.EpostaTalepYonet]      = ["ADMIN", "BGYSADMINS"],

        [Yetkiler.SistemYonet]           = ["ADMIN"]
    };

    /// <summary>
    /// Bir politikanın rollerini döner. Konfigürasyonda tanımlıysa o, değilse
    /// varsayılan kullanılır. Hiçbiri yoksa boş liste döner — bu durumda
    /// politikayı kimse karşılayamaz, yani güvenli tarafta kalınır.
    /// </summary>
    public IReadOnlyList<string> RolleriGetir(string politika)
    {
        if (Politikalar.TryGetValue(politika, out var roller) && roller.Count > 0)
            return roller;

        return Varsayilanlar.TryGetValue(politika, out var varsayilan) ? varsayilan : [];
    }
}
