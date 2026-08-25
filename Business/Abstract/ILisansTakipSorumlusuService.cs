using Dto.LisansTakipSorumlusu;

namespace Business.Abstract;

public interface ILisansTakipSorumlusuService
{
    Task<List<ListLisansTakipSorumlusuDto>> GetAllAsync();
    Task<UpdateLisansTakipSorumlusuDto> GetByIdAsync(int id);
    Task<CreateLisansTakipSorumlusuDto> AddAsync(CreateLisansTakipSorumlusuDto dto);
    Task<UpdateLisansTakipSorumlusuDto> UpdateAsync(UpdateLisansTakipSorumlusuDto dto);
    Task DeleteAsync(int id);
    Task<bool> AnyAsync(int id);
} 