using Dto.YedeklemeSorumlusu;

namespace Business.Abstract;

public interface IYedeklemeSorumlusuService
{
    Task<List<ListYedeklemeSorumlusuDto>> GetAllAsync();
    Task<UpdateYedeklemeSorumlusuDto> GetByIdAsync(int id);
    Task<CreateYedeklemeSorumlusuDto> AddAsync(CreateYedeklemeSorumlusuDto dto);
    Task<UpdateYedeklemeSorumlusuDto> UpdateAsync(UpdateYedeklemeSorumlusuDto dto);
    Task DeleteAsync(int id);
    Task<bool> AnyAsync(int id);
} 