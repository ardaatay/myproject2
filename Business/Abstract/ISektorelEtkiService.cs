using Dto.SektorelEtki;

namespace Business.Abstract;

public interface ISektorelEtkiService
{
    Task<List<ListSektorelEtkiDto>> GetAllAsync();
    Task<UpdateSektorelEtkiDto> GetByIdAsync(int id);
    Task<CreateSektorelEtkiDto> AddAsync(CreateSektorelEtkiDto dto);
    Task<UpdateSektorelEtkiDto> UpdateAsync(UpdateSektorelEtkiDto dto);
    Task DeleteAsync(int id);
    Task<bool> AnyAsync(int id);
} 