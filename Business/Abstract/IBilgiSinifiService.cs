using Dto.BilgiSinifi;

namespace Business.Abstract;

public interface IBilgiSinifiService
{
    Task<List<ListBilgiSinifiDto>> GetAllAsync();
    Task<UpdateBilgiSinifiDto> GetByIdAsync(int id);
    Task<CreateBilgiSinifiDto> AddAsync(CreateBilgiSinifiDto dto);
    Task<UpdateBilgiSinifiDto> UpdateAsync(UpdateBilgiSinifiDto dto);
    Task DeleteAsync(int id);
    Task<bool> AnyAsync(int id);
} 