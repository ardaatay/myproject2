namespace Core.Entity;

/// <summary>
/// Kiracıya ait kayıtları işaretler. Bu arayüzü uygulayan her entity için
/// DbContext'te global sorgu filtresi kurulur ve yeni kayıtlara aktif
/// organizasyon otomatik atanır.
///
/// Referans/liste tabloları (gizlilik, kategori, durum ve benzeri) bilinçli
/// olarak bu arayüzü uygulamaz: tüm organizasyonlarda ortaktır.
/// </summary>
public interface IKiraciEntity
{
    int OrganizasyonId { get; set; }
}
