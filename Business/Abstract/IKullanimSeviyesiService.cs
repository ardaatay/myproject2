using Dto.KullanimSeviyesi;

namespace Business.Abstract;

public interface IKullanimSeviyesiService
{
    Task<List<ListKullanimSeviyesiDto>> GetAllAsync();
    Task<UpdateKullanimSeviyesiDto> GetByIdAsync(int id);
    Task<CreateKullanimSeviyesiDto> AddAsync(CreateKullanimSeviyesiDto dto);
    Task<UpdateKullanimSeviyesiDto> UpdateAsync(UpdateKullanimSeviyesiDto dto);
    Task DeleteAsync(int id);
    Task<bool> AnyAsync(int id);
} 