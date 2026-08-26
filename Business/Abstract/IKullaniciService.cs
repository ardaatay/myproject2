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
        Task<KullaniciDuzenleDto?> DuzenlemeIcinGetirAsync(int kullaniciBirimId);

        /// <summary>
        /// Kullanıcı kimlik bilgilerini, giriş yöntemini ve birimini birlikte günceller.
        /// Giriş yöntemi değiştiğinde yerel şifre ve açık oturumlar geçersizleşir.
        /// </summary>
        Task DuzenleAsync(KullaniciDuzenleDto dto);
        Task<ListKullaniciDto> GetByUsernameAsync(string username);
        Task<UpdateKullaniciDto> AddAsync(CreateKullaniciDto dto);
        Task<UpdateKullaniciDto> UpdateAsync(UpdateKullaniciDto dto);
        Task DeleteAsync(int id);
        Task<bool> AnyAsync(int id);
        Task<List<KullaniciListeDto>> KullanicilariGetirAsync();
    }
}