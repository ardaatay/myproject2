using Dto.DTOs;
using Dto.Kullanici;
using Dto.KullaniciBirim;

namespace Business.Abstract;

public interface IKullaniciBirimService
{
    Task<DataTablesResponse<ListKullaniciBirimDto>> GetAllAsync(DataTablesRequest request);
    public Task<List<KullaniciBirimDto>> GetByKullaniciIdAsync(int kullaniciId);
    public Task<UpdateKullaniciBirimDto?> GetByKullaniciIdAndBirimIdAsync(int kullaniciId, int birimId);
    public Task<ListKullaniciBirimDto?> GetByIdAsync(int id);
    public Task AddAsync(CreateKullaniciBirimDto createKullaniciBirimDto);
    public Task UpdateAsync(UpdateKullaniciBirimDto updateKullaniciBirimDto);
    public Task DeleteAsync(int id);
}