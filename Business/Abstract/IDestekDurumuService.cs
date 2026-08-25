using Dto.DestekDurumu;

namespace Business.Abstract;

public interface IDestekDurumuService
{
    Task<List<ListDestekDurumuDto>> GetAllAsync();
    Task<UpdateDestekDurumuDto> GetByIdAsync(int id);
    Task<CreateDestekDurumuDto> AddAsync(CreateDestekDurumuDto dto);
    Task<UpdateDestekDurumuDto> UpdateAsync(UpdateDestekDurumuDto dto);
    Task DeleteAsync(int id);
    Task<bool> AnyAsync(int id);
} 