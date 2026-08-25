using Dto.Durum;

namespace Business.Abstract;

public interface IDurumService
{
    Task<List<ListDurumDto>> GetAllAsync();
    Task<UpdateDurumDto> GetByIdAsync(int id);
    Task<CreateDurumDto> AddAsync(CreateDurumDto dto);
    Task<UpdateDurumDto> UpdateAsync(UpdateDurumDto dto);
    Task DeleteAsync(int id);
    Task<bool> AnyAsync(int id);
} 