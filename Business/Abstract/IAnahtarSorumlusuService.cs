using Dto.AnahtarSorumlusu;

namespace Business.Abstract;

public interface IAnahtarSorumlusuService
{
    Task<List<ListAnahtarSorumlusuDto>> GetAllAsync();
    Task<UpdateAnahtarSorumlusuDto> GetByIdAsync(int id);
    Task<CreateAnahtarSorumlusuDto> AddAsync(CreateAnahtarSorumlusuDto dto);
    Task<UpdateAnahtarSorumlusuDto> UpdateAsync(UpdateAnahtarSorumlusuDto dto);
    Task DeleteAsync(int id);
    Task<bool> AnyAsync(int id);
} 