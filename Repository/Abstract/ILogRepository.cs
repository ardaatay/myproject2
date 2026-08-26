using Dto.DTOs;
using Dto.Loglar;
using Entity.Concrete;

namespace Repository.Abstract;

public interface ILogRepository
{
    /// <summary>İşlem logunu yazar. Çağıranın işleminden bağımsızdır.</summary>
    void AddLog(Log entity);

    /// <summary>
    /// Hata kaydını yazar ve kalıcı olan kodu döner. Kod çakışırsa yenisi
    /// üretilip yeniden denenir, bu yüzden dönen değer kullanılmalıdır.
    /// </summary>
    Task<string> AddHataLogAsync(HataLog entity, CancellationToken cancellationToken = default);

    Task<DataTablesResponse<ListIslemLogDto>> IslemLoglariAsync(DataTablesRequest request, LogFiltreDto filtre);
    Task<IslemLogDetayDto?> IslemLogGetirAsync(int id);

    Task<DataTablesResponse<ListHataLogDto>> HataLoglariAsync(DataTablesRequest request, LogFiltreDto filtre);
    Task<HataLogDetayDto?> HataLogGetirAsync(int id);

    /// <summary>Kullanıcının bildirdiği kodla hata kaydını bulur.</summary>
    Task<HataLogDetayDto?> HataKoduIleGetirAsync(string kod);

    Task<bool> CozumIsaretleAsync(int id, bool cozuldu, string? not, string kullanici);

    /// <summary>Liste ekranındaki özet sayaçlar.</summary>
    Task<(int Toplam, int Cozulmemis, int SonYirmiDortSaat)> HataOzetiAsync();
}
