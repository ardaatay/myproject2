using System.ComponentModel.DataAnnotations;

namespace Dto.Loglar;

/// <summary>İşlem logu listesi. Ayrıntı alanları (parametre, dönüş değeri) taşınmaz.</summary>
public class ListIslemLogDto
{
    public int Id { get; set; }
    public DateTime Tarih { get; set; }
    public string Kullanici { get; set; } = null!;

    /// <summary>Çağrılan iş sınıfı, "Manager" eki atılmış hâliyle.</summary>
    public string Modul { get; set; } = null!;

    public string Islem { get; set; } = null!;
    public bool Basarili { get; set; }
    public string DurumStr { get; set; } = null!;
    public string? HataKodu { get; set; }
    public int SureMs { get; set; }
    public string? Yol { get; set; }
    public string? IpAdresi { get; set; }
}

/// <summary>Tek bir işlem logunun tamamı.</summary>
public class IslemLogDetayDto : ListIslemLogDto
{
    public string? Parametreler { get; set; }
    public string? DonusDegeri { get; set; }
    public string? Hata { get; set; }

    /// <summary>Hata koduna karşılık bir hata kaydı bulunuyorsa kimliği.</summary>
    public int? HataLogId { get; set; }
}

/// <summary>Hata logu listesi. Yığın izi taşınmaz.</summary>
public class ListHataLogDto
{
    public int Id { get; set; }
    public string Kod { get; set; } = null!;
    public DateTime Tarih { get; set; }
    public string Tur { get; set; } = null!;
    public string Mesaj { get; set; } = null!;
    public int DurumKodu { get; set; }
    public string? Yol { get; set; }
    public string? Kullanici { get; set; }
    public bool Cozuldu { get; set; }
    public string DurumStr { get; set; } = null!;
}

/// <summary>Tek bir hata kaydının tamamı.</summary>
public class HataLogDetayDto : ListHataLogDto
{
    public string? KullaniciMesaji { get; set; }
    public string? Ayrinti { get; set; }
    public string? HttpYontemi { get; set; }
    public string? IpAdresi { get; set; }
    public string? IstekId { get; set; }
    public string? CozumNotu { get; set; }
    public DateTime? CozulmeTarihi { get; set; }
    public string? CozenKullanici { get; set; }

    /// <summary>Aynı koda bağlı işlem logu varsa kimliği; iki kayıt arasında geçiş için.</summary>
    public int? IslemLogId { get; set; }
}

/// <summary>
/// Liste ekranlarının süzgeç çubuğu. Boş bırakılan alanlar süzgece katılmaz.
/// </summary>
public class LogFiltreDto
{
    [Display(Name = "Başlangıç")]
    [DataType(DataType.Date)]
    public DateTime? Baslangic { get; set; }

    [Display(Name = "Bitiş")]
    [DataType(DataType.Date)]
    public DateTime? Bitis { get; set; }

    [Display(Name = "Kullanıcı")]
    [MaxLength(500)]
    public string? Kullanici { get; set; }

    /// <summary>İşlem logunda yalnızca hatalı çağrılar, hata logunda yalnızca çözülmemişler.</summary>
    [Display(Name = "Yalnızca sorunlular")]
    public bool YalnizcaSorunlu { get; set; }

    [Display(Name = "Hata kodu")]
    [MaxLength(32)]
    public string? HataKodu { get; set; }

    /// <summary>Bitişi gün sonuna taşır; aksi halde seçilen günün kayıtları listeye girmez.</summary>
    public DateTime? BitisGunSonu => Bitis?.Date.AddDays(1);
}

/// <summary>Hata kaydını çözüldü olarak işaretleme isteği.</summary>
public class HataCozumDto
{
    public int Id { get; set; }
    public bool Cozuldu { get; set; }

    [MaxLength(2000)]
    [Display(Name = "Çözüm notu")]
    public string? Not { get; set; }
}
