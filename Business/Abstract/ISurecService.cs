using Dto.DTOs;
using Dto.Surec;
using Util.Query;

namespace Business.Abstract;

public interface ISurecService
{
    Task<DataTablesResponse<ListSurecDto>> GetAllAsync(DataTablesRequest request);

    Task<List<ListSurecDto>> GetAllExcelAsync();

    Task<List<ListSurecDto>> GetAllExcelAsync(
        string search, FilterBag filterBag);

    Task<UpdateSurecDto> GetByIdAsync(int id);
    Task<CreateSurecDto> AddAsync(CreateSurecDto dto);
    Task<UpdateSurecDto> UpdateAsync(UpdateSurecDto dto);
    Task DeleteAsync(int id);
    Task DeleteDatabaseAsync(int id);
    Task<bool> AnyAsync(int id);
}