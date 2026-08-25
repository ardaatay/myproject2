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

    /// <summary>Kök birimler (üst birimi olmayanlar), sıra ve ada göre.</summary>
    Task<List<BirimSecimDto>> GetKokBirimlerAsync(bool sadeceAktif = true);

    /// <summary>Verilen birimin tüm alt ağacı. Birimin kendisi dahil edilmez.</summary>
    Task<List<BirimSecimDto>> GetAltAgacAsync(int ustId, bool sadeceAktif = true);

    /// <summary>Verilen birimin doğrudan çocukları.</summary>
    Task<List<Birim>> GetDogrudanAltBirimlerAsync(int ustId);

    /// <summary>Alt ağaçtaki tüm birimler; üst birim taşındığında yol güncellemesi için.</summary>
    Task<List<Birim>> GetAltAgacEntityAsync(string yolOneki);

    /// <summary>Ağaç görünümü için tüm birimler, hiyerarşik sırada.</summary>
    Task<List<ListBirimDto>> GetAgacAsync();
}
