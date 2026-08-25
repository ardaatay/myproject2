using Dto.YedeklemeTipi;

namespace Business.Abstract;

public interface IYedeklemeTipiService
{
    Task<List<ListYedeklemeTipiDto>> GetAllAsync();
    Task<UpdateYedeklemeTipiDto> GetByIdAsync(int id);
    Task<CreateYedeklemeTipiDto> AddAsync(CreateYedeklemeTipiDto dto);
    Task<UpdateYedeklemeTipiDto> UpdateAsync(UpdateYedeklemeTipiDto dto);
    Task DeleteAsync(int id);
    Task<bool> AnyAsync(int id);
} 