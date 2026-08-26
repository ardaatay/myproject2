using System.ComponentModel.DataAnnotations;
using Core.Entity;
using Core.Logging;

namespace Entity.Concrete;

/// <summary>
/// İşlem logu: <c>[LogAspect]</c> ile işaretlenmiş iş katmanı çağrılarının
/// kaydı. Kim, ne zaman, hangi veriyle ne yaptı sorusunu yanıtlar.
///
/// Kayıt, işin kendi işleminden ayrı bir bağlantıyla yazılır; böylece iş
/// işlemi geri alınsa bile log kalır (bkz. <c>LogRepository</c>).
/// </summary>
public class Log : IEntity<int>, IKiraciEntity
{
    public int Id { get; set; }
    public int OrganizasyonId { get; set; }
    [MaxLength(500)] public string MethodName { get; set; } = null!;
    [MaxLength(500)] public string ClassName { get; set; } = null!;
    [MaxLength(4000)] public string Parameters { get; set; } = null!;
    public DateTime ExecutingTime { get; set; }
    [MaxLength(4000)] public string? ReturnValue { get; set; }
    [MaxLength(4000)] public string? Error { get; set; }
    [MaxLength(500)] public string Username { get; set; } = null!;

    /// <summary>İşlem hatasız tamamlandı mı. Liste ekranında birincil süzgeçtir.</summary>
    public bool Basarili { get; set; } = true;

    /// <summary>
    /// İşlem hata ile bittiyse, aynı istekte oluşan hata kaydının referansı.
    /// Kullanıcıya gösterilen kodla birebir aynıdır.
    /// </summary>
    [MaxLength(Core.Logging.HataKodu.Uzunluk)] public string? HataKodu { get; set; }

    /// <summary>Çağrının süresi. Yavaşlayan işlemleri ayıklamak için.</summary>
    public int SureMs { get; set; }

    [MaxLength(45)] public string? IpAdresi { get; set; }

    /// <summary>İşlemi tetikleyen istek yolu.</summary>
    [MaxLength(500)] public string? Yol { get; set; }
}
