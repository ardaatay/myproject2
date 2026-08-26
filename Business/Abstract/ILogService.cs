using Dto.DTOs;
using Dto.Loglar;
using Entity.Concrete;

namespace Business.Abstract;

public interface ILogService
{
    /// <summary>
    /// İşlem logunu yazar. Loglama hiçbir koşulda asıl işi bozmamalıdır;
    /// yazma başarısız olursa istisna dışarı sızmaz.
    /// </summary>
    void Add(Log entity);

    /// <summary>
    /// Oturum ve şifre olaylarını kaydeder.
    ///
    /// Bu akışlar düz metin şifre taşıdığı için aspect ile sarılamaz; olay bu
    /// yüzden yalnızca güvenli alanlarla, açıkça yazılır. Kiracı kimliği dışarıdan
    /// verilir: giriş anında oturum henüz açılmadığı için istekten okunamaz.
    /// </summary>
    void OturumOlayiEkle(
        string islem,
        string kullaniciAdi,
        int organizasyonId,
        bool basarili,
        string? sonuc = null);

    /// <summary>
    /// İstisnayı kalıcı olarak kaydeder ve kullanıcıya gösterilecek kodu döner.
    /// Kayıt yapılamazsa yine de bir kod döner — kullanıcıya "kod yok" demek
    /// yerine, kodu olan ama kaydı bulunamayan bir hata daha yönetilebilirdir.
    /// </summary>
    Task<string> HataKaydetAsync(
        Exception istisna,
        int durumKodu,
        string? kullaniciMesaji,
        CancellationToken cancellationToken = default);

    Task<DataTablesResponse<ListIslemLogDto>> IslemLoglariAsync(DataTablesRequest request, LogFiltreDto filtre);
    Task<IslemLogDetayDto?> IslemLogGetirAsync(int id);

    Task<DataTablesResponse<ListHataLogDto>> HataLoglariAsync(DataTablesRequest request, LogFiltreDto filtre);
    Task<HataLogDetayDto?> HataLogGetirAsync(int id);
    Task<HataLogDetayDto?> HataKoduIleGetirAsync(string kod);

    Task<bool> CozumIsaretleAsync(int id, bool cozuldu, string? not);

    Task<(int Toplam, int Cozulmemis, int SonYirmiDortSaat)> HataOzetiAsync();
}
