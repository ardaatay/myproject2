using Dto.KriptolojiTuru;

namespace Business.Abstract;

public interface IKriptolojiTuruService
{
    Task<List<ListKriptolojiTuruDto>> GetAllAsync();
    Task<UpdateKriptolojiTuruDto> GetByIdAsync(int id);
    Task<CreateKriptolojiTuruDto> AddAsync(CreateKriptolojiTuruDto dto);
    Task<UpdateKriptolojiTuruDto> UpdateAsync(UpdateKriptolojiTuruDto dto);
    Task DeleteAsync(int id);
    Task<bool> AnyAsync(int id);
} 