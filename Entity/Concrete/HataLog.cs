using System.ComponentModel.DataAnnotations;
using Core.Entity;
using Core.Logging;

namespace Entity.Concrete;

/// <summary>
/// Hata logu: kullanıcıya bir hata bildirimi gösterilmesine yol açan her
/// istisna buraya düşer.
///
/// <see cref="Kod"/> kullanıcıya gösterilen referanstır; yönetici hatayı
/// yalnızca bu kodla arayarak bulabilmelidir. Bu yüzden kod hem tekil hem de
/// dizinlidir.
/// </summary>
public class HataLog : IEntity<int>, IKiraciEntity
{
    public int Id { get; set; }

    /// <summary>
    /// Kaydın ait olduğu kiracı. Oturum açılmadan oluşan hatalarda 0 kalır;
    /// bu kayıtlar yalnızca kurumlar arası yetkili tarafından görülür.
    /// </summary>
    public int OrganizasyonId { get; set; }

    /// <summary>Kullanıcıya gösterilen referans: <c>HTA-K7F4-9QXZ</c>.</summary>
    [MaxLength(HataKodu.Uzunluk)] public string Kod { get; set; } = null!;

    public DateTime OlusmaTarihi { get; set; }

    /// <summary>İstisnanın tür adı, örneğin <c>NotFoundException</c>.</summary>
    [MaxLength(300)] public string Tur { get; set; } = null!;

    [MaxLength(2000)] public string Mesaj { get; set; } = null!;

    /// <summary>Kullanıcıya gösterilen metin. Teknik mesajdan farklı olabilir.</summary>
    [MaxLength(2000)] public string? KullaniciMesaji { get; set; }

    /// <summary>Yığın izi ve iç istisnalar. Yalnızca yönetim ekranında görünür.</summary>
    public string? Ayrinti { get; set; }

    /// <summary>Kullanıcıya dönen HTTP durum kodu.</summary>
    public int DurumKodu { get; set; }

    [MaxLength(500)] public string? Yol { get; set; }
    [MaxLength(10)] public string? HttpYontemi { get; set; }
    [MaxLength(500)] public string? Kullanici { get; set; }
    [MaxLength(45)] public string? IpAdresi { get; set; }
    [MaxLength(100)] public string? IstekId { get; set; }

    /// <summary>Yönetici hatayı ele aldığında işaretlenir; liste bunu süzer.</summary>
    public bool Cozuldu { get; set; }

    [MaxLength(2000)] public string? CozumNotu { get; set; }
    public DateTime? CozulmeTarihi { get; set; }
    [MaxLength(500)] public string? CozenKullanici { get; set; }
}
