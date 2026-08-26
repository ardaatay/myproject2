using Dto.Organizasyon;

namespace Business.Abstract;

/// <summary>
/// Kurulumun kendi kurum kaydı (kiracı). E-posta taleplerinde referans verilen
/// <c>Kurum</c> listesinden ayrıdır — bkz. <see cref="IKurumService"/>.
/// </summary>
public interface IKurumBilgileriService
{
    Task<KurumBilgileriDto> GetirAsync();

    Task GuncelleAsync(KurumBilgileriDto dto);

    /// <summary>
    /// Başlık ve logo için görünen kimlik. Her sayfa çiziminde çağrıldığından
    /// istek başına bir kez okunur ve hata durumunda uygulama ayarlarına düşer;
    /// bu çağrı hiçbir koşulda sayfanın çizilmesini engellememelidir.
    /// </summary>
    Task<KurumKimligiDto> GorunenKimlikAsync();
}
