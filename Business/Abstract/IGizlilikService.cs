using Dto.Gizlilik;

namespace Business.Abstract;

public interface IGizlilikService
{
    Task<List<ListGizlilikDto>> GetAllAsync();
    Task<UpdateGizlilikDto> GetByIdAsync(int id);
    Task<CreateGizlilikDto> AddAsync(CreateGizlilikDto dto);
    Task<UpdateGizlilikDto> UpdateAsync(UpdateGizlilikDto dto);
    Task DeleteAsync(int id);
    Task<bool> AnyAsync(int id);
} 