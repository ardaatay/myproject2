using Dto.DTOs;
using Dto.Rapor;
using Dto.TasinabilirCihazveOrtam;
using Util.Query;

namespace Business.Abstract;

public interface ITasinabilirCihazveOrtamService
{
    Task<DataTablesResponse<ListTasinabilirCihazveOrtamDto>> GetAllAsync(
        DataTablesRequest request
    );

    Task<List<ListTasinabilirCihazveOrtamDto>> GetAllExcelAsync();

    Task<List<ListTasinabilirCihazveOrtamDto>> GetAllExcelAsync(
        string search, FilterBag filterBag);

    Task<UpdateTasinabilirCihazveOrtamDto> GetByIdAsync(int id);
    Task<CreateTasinabilirCihazveOrtamDto> AddAsync(CreateTasinabilirCihazveOrtamDto dto);
    Task<UpdateTasinabilirCihazveOrtamDto> UpdateAsync(UpdateTasinabilirCihazveOrtamDto dto);
    Task DeleteAsync(int id);
    Task DeleteDatabaseAsync(int id);
    Task<bool> AnyAsync(int id);
    Task<List<RaporAnasayfa>> RaporAsync();
}