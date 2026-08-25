using Dto.DTOs;
using Dto.Kurum;

namespace Business.Abstract;

public interface IKurumService
{
    Task<DataTablesResponse<ListKurumDto>> GetAllAsync(
        DataTablesRequest request);

    Task<IEnumerable<ListKurumDto>> GetAllAsync();

    Task<List<ListKurumDto>> GetAllExcelAsync();

    Task<List<ListKurumDto>> GetAllExcelAsync(
        string search);

    Task<UpdateKurumDto> GetByIdAsync(int id);
    Task<CreateKurumDto> AddAsync(CreateKurumDto dto);
    Task<UpdateKurumDto> UpdateAsync(UpdateKurumDto dto);
    Task DeleteAsync(int id);
    Task DeleteDatabaseAsync(int id);
    Task<bool> AnyAsync(int id);
}