using System.Linq.Expressions;
using Core.Repository;
using Dto.Birim;
using Dto.DTOs;
using Entity.Concrete;

namespace Repository.Abstract;

public interface IBirimRepository : IGenericRepository<Birim, int>
{
    Task<DataTablesResponse<ListBirimDto>> ProcessTableRequestAsync(
        DataTablesRequest request,
        Expression<Func<ListBirimDto, bool>>? filter = null);

    /// <summary>Kök birimler (üst birimi olmayanlar), ağaçtaki sıralarına göre.</summary>
    Task<List<BirimSecimDto>> GetKokBirimlerAsync(bool sadeceAktif = true);

    /// <summary>Verilen birimin tüm alt ağacı. Birimin kendisi dahil edilmez.</summary>
    Task<List<BirimSecimDto>> GetAltAgacAsync(int ustId, bool sadeceAktif = true);

    /// <summary>Verilen birimin doğrudan çocukları.</summary>
    Task<List<Birim>> GetDogrudanAltBirimlerAsync(int ustId);

    /// <summary>Verilen düğümün alt ağacındaki birimler, ön sıralı gezinme sırasında.</summary>
    Task<List<Birim>> GetAltAgacEntityAsync(Birim kok, bool kendisiDahil = false);


    /// <summary>Ağaç görünümü için tüm birimler, hiyerarşik sırada.</summary>
    Task<List<ListBirimDto>> GetAgacAsync();

    /// <summary>
    /// Kiracının ağacındaki türev sütunları (Sol, Sag, Seviye, Yol) komşuluk
    /// bilgisinden (UstId + Sira + Ad) yeniden üretir ve değişen satırları yazar.
    /// Yapısal her değişiklikten sonra çağrılır; yinelenebilir ve zaten tutarlı
    /// bir ağaçta hiçbir satıra dokunmaz. Değişen satır sayısını döndürür.
    /// </summary>
    Task<int> AgaciYenidenKurAsync(int organizasyonId);
}
