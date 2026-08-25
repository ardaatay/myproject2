using Dto.Kullanici;
using Dto.KullaniciRol;

namespace Business.Abstract
{
    public interface IKullaniciRolService
    {
        Task<List<ListKullaniciRolDto>> GetAllAsync();
        Task<UpdateKullaniciRolDto> GetByIdAsync(int id);
        Task<List<ListKullaniciRolDto>> GetByUsernameAsync(string username);
        Task<CreateKullaniciRolDto> AddAsync(CreateKullaniciRolDto dto);
        Task<UpdateKullaniciRolDto> UpdateAsync(UpdateKullaniciRolDto dto);
        Task DeleteAsync(int id);
        Task<bool> AnyAsync(int id);
        Task<KullaniciRolAtamaDto> KullaniciRolleriniGetirAsync(int kullaniciId);
    }
}
