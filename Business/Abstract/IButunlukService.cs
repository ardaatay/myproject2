using Dto.Butunluk;

namespace Business.Abstract;

public interface IButunlukService
{
    Task<List<ListButunlukDto>> GetAllAsync();
    Task<UpdateButunlukDto> GetByIdAsync(int id);
    Task<CreateButunlukDto> AddAsync(CreateButunlukDto dto);
    Task<UpdateButunlukDto> UpdateAsync(UpdateButunlukDto dto);
    Task DeleteAsync(int id);
    Task<bool> AnyAsync(int id);
} 