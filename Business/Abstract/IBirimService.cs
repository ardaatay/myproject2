using Dto.Birim;
using Dto.DTOs;

namespace Business.Abstract;

public interface IBirimService
{
    Task<DataTablesResponse<ListBirimDto>> GetAllAsync(DataTablesRequest request);

    /// <summary>Ağaç görünümü: tüm birimler hiyerarşik sırada.</summary>
    Task<List<ListBirimDto>> GetAgacAsync();

    /// <summary>Varlık formlarındaki birinci kademe açılır listeyi besler.</summary>
    Task<List<BirimSecimDto>> GetUstBirimlerAsync();

    /// <summary>Varlık formlarındaki ikinci kademe (alt departman) açılır listeyi besler.</summary>
    Task<List<BirimSecimDto>> GetAltBirimByParentIdAsync(int ustId);

    /// <summary>Üst birim seçim listesi; düzenlemede birimin kendi alt ağacı listeden çıkarılır.</summary>
    Task<List<BirimSecimDto>> GetUstBirimSecenekleriAsync(int? haricId = null);

    Task<UpdateBirimDto?> GetByIdAsync(int id);
    Task<CreateBirimDto> AddAsync(CreateBirimDto dto);
    Task<UpdateBirimDto> UpdateAsync(UpdateBirimDto dto);
    Task DeleteAsync(int id);
    Task<bool> AnyAsync(int id);
}
