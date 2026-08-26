using Entity.Concrete;
using Microsoft.EntityFrameworkCore;

namespace Repository.Context;

/// <summary>
/// Kiracı kimliği belirlenemeyen kayıtlar için son çare.
///
/// Bazı kayıtlar oturum açılmadan oluşur: giriş ekranındaki hatalar, bilinmeyen
/// bir kullanıcı adıyla yapılan giriş denemeleri. Bunlar sahipsiz bırakılırsa
/// (<c>organizasyon_id = 0</c>) kiracı sorgu filtresi yüzünden hiçbir listede
/// görünmez — oysa yöneticinin görmesi gereken kayıtlar tam olarak bunlardır.
///
/// Dağıtım tek kurumlu olduğu için kayıt o organizasyona bağlanır. Birden fazla
/// organizasyon bulunursa hangisine ait olduğu bilinemez ve kayıt sahipsiz kalır.
/// </summary>
public static class KiraciCozumleme
{
    public static async Task<Organizasyon?> TekOrganizasyonAsync(
        this VarlikEnvanteriDbContext context,
        CancellationToken cancellationToken = default)
    {
        // İki kayıt çekilir: "tek mi" sorusunu yanıtlamak için sayıyı ayrıca
        // saymaya gerek yok.
        var kayitlar = await context.Organizasyonlar
            .AsNoTracking()
            .Take(2)
            .ToListAsync(cancellationToken);

        return kayitlar.Count == 1 ? kayitlar[0] : null;
    }

    public static async Task<int> TekOrganizasyonIdAsync(
        this VarlikEnvanteriDbContext context,
        CancellationToken cancellationToken = default)
    {
        var kimlikler = await context.Organizasyonlar
            .AsNoTracking()
            .Select(o => o.Id)
            .Take(2)
            .ToListAsync(cancellationToken);

        return kimlikler.Count == 1 ? kimlikler[0] : 0;
    }

    /// <summary>
    /// Eşzamansız karşılığı olmayan log yazımı için. Yalnızca kiracı gerçekten
    /// bilinmediğinde çağrılır, yani ender bir yoldur.
    /// </summary>
    public static int TekOrganizasyonId(this VarlikEnvanteriDbContext context)
    {
        var kimlikler = context.Organizasyonlar
            .AsNoTracking()
            .Select(o => o.Id)
            .Take(2)
            .ToList();

        return kimlikler.Count == 1 ? kimlikler[0] : 0;
    }
}
