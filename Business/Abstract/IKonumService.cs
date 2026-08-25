using Dto.Konum;

namespace Business.Abstract;

public interface IKonumService
{
    Task<List<ListKonumDto>> GetAllAsync();
    Task<UpdateKonumDto> GetByIdAsync(int id);
    Task<CreateKonumDto> AddAsync(CreateKonumDto dto);
    Task<UpdateKonumDto> UpdateAsync(UpdateKonumDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> AnyAsync(int id);
}