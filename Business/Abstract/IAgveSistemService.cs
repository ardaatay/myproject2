using Dto.AgveSistem;
using Dto.DTOs;
using Dto.Rapor;
using Util.Query;

namespace Business.Abstract;

public interface IAgveSistemService
{
    Task<DataTablesResponse<ListAgveSistemDto>> GetAllAsync(
        DataTablesRequest request);

    Task<List<ListAgveSistemDto>> GetAllExcelAsync();

    Task<List<ListAgveSistemDto>> GetAllExcelAsync(string search, FilterBag filterBag);

    Task<UpdateAgveSistemDto> GetByIdAsync(int id);
    Task<CreateAgveSistemDto> AddAsync(CreateAgveSistemDto dto);
    Task<UpdateAgveSistemDto> UpdateAsync(UpdateAgveSistemDto dto);
    Task DeleteAsync(int id);
    Task DeleteDatabaseAsync(int id);
    Task<bool> AnyAsync(int id);
    Task<List<RaporAnasayfa>> RaporAsync();
}