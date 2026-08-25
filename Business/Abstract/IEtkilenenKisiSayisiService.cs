using Dto.EtkilenenKisiSayisi;

namespace Business.Abstract;

public interface IEtkilenenKisiSayisiService
{
    Task<List<ListEtkilenenKisiSayisiDto>> GetAllAsync();
    Task<UpdateEtkilenenKisiSayisiDto> GetByIdAsync(int id);
    Task<CreateEtkilenenKisiSayisiDto> AddAsync(CreateEtkilenenKisiSayisiDto dto);
    Task<UpdateEtkilenenKisiSayisiDto> UpdateAsync(UpdateEtkilenenKisiSayisiDto dto);
    Task DeleteAsync(int id);
    Task<bool> AnyAsync(int id);
} 