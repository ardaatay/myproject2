using Dto.DTOs;
using Dto.Kullanici;

namespace Business.Abstract
{
    public interface IKullaniciService
    {
        Task<DataTablesResponse<ListKullaniciDto>> GetAllAsync(
            DataTablesRequest request);

        Task<List<ListKullaniciDto>> GetAllAsync();
        Task<UpdateKullaniciDto> GetByIdAsync(int id);
        Task<ListKullaniciDto> GetByUsernameAsync(string username);
        Task<UpdateKullaniciDto> AddAsync(CreateKullaniciDto dto);
        Task<UpdateKullaniciDto> UpdateAsync(UpdateKullaniciDto dto);
        Task DeleteAsync(int id);
        Task<bool> AnyAsync(int id);
        Task<List<KullaniciListeDto>> KullanicilariGetirAsync();
    }
}