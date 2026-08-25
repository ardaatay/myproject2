using Dto.Erisilebilirlik;

namespace Business.Abstract;

public interface IErisilebilirlikService
{
    Task<List<ListErisilebilirlikDto>> GetAllAsync();
    Task<UpdateErisilebilirlikDto> GetByIdAsync(int id);
    Task<CreateErisilebilirlikDto> AddAsync(CreateErisilebilirlikDto dto);
    Task<UpdateErisilebilirlikDto> UpdateAsync(UpdateErisilebilirlikDto dto);
    Task DeleteAsync(int id);
    Task<bool> AnyAsync(int id);
} 