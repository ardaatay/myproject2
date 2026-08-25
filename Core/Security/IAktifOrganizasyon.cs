namespace Core.Security;

/// <summary>
/// İstek boyunca geçerli kiracıyı taşır. Değer, oturum açan kullanıcının
/// claim'inden gelir.
/// </summary>
public interface IAktifOrganizasyon
{
    /// <summary>
    /// Aktif organizasyon. <c>null</c> ise kiracı filtresi uygulanmaz —
    /// bu yalnızca kurumlar arası yetkili (SUPERADMIN) oturumlar ve
    /// kimlik doğrulaması gerektirmeyen açılış işlemleri için geçerlidir.
    /// </summary>
    int? Id { get; }
}
