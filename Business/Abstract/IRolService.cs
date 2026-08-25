using Dto.Kullanici;
using Dto.Rol;

namespace Business.Abstract
{
    public interface IRolService
    {
        Task<List<ListRolDto>> GetAllAsync();
        Task<UpdateRolDto> GetByIdAsync(int id);
        Task<CreateRolDto> AddAsync(CreateRolDto dto);
        Task<UpdateRolDto> UpdateAsync(UpdateRolDto dto);
        Task DeleteAsync(int id);
        Task<bool> AnyAsync(int id);
        Task RolleriKaydetAsync(KullaniciRolAtamaDto model);
    }
}