using Dto.KurumsalSonuc;

namespace Business.Abstract;

public interface IKurumsalSonucService
{
    Task<List<ListKurumsalSonucDto>> GetAllAsync();
    Task<UpdateKurumsalSonucDto> GetByIdAsync(int id);
    Task<CreateKurumsalSonucDto> AddAsync(CreateKurumsalSonucDto dto);
    Task<UpdateKurumsalSonucDto> UpdateAsync(UpdateKurumsalSonucDto dto);
    Task DeleteAsync(int id);
    Task<bool> AnyAsync(int id);
} 