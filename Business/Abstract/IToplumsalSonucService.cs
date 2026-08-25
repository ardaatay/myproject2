using Dto.ToplumsalSonuc;

namespace Business.Abstract;

public interface IToplumsalSonucService
{
    Task<List<ListToplumsalSonucDto>> GetAllAsync();
    Task<UpdateToplumsalSonucDto> GetByIdAsync(int id);
    Task<CreateToplumsalSonucDto> AddAsync(CreateToplumsalSonucDto dto);
    Task<UpdateToplumsalSonucDto> UpdateAsync(UpdateToplumsalSonucDto dto);
    Task DeleteAsync(int id);
    Task<bool> AnyAsync(int id);
} 