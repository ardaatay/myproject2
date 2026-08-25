using Dto.Kategori;

namespace Business.Abstract;

public interface IKategoriService
{
    Task<List<ListKategoriDto>> GetAllAsync();
    Task<List<ListKategoriDto>> GetAllByUstIdAsync(int ustId);
    Task<UpdateKategoriDto> GetByIdAsync(int id);
    Task<CreateKategoriDto> AddAsync(CreateKategoriDto dto);
    Task<UpdateKategoriDto> UpdateAsync(UpdateKategoriDto dto);
    Task DeleteAsync(int id);
    Task<bool> AnyAsync(int id);
}