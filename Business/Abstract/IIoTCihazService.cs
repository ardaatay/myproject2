using Dto.DTOs;
using Dto.IoTCihaz;
using Dto.Rapor;
using Util.Query;

namespace Business.Abstract;

public interface IIoTCihazService
{
    Task<DataTablesResponse<ListIoTCihazDto>> GetAllAsync(
        DataTablesRequest request);

    Task<List<ListIoTCihazDto>> GetAllExcelAsync();

    Task<List<ListIoTCihazDto>> GetAllExcelAsync(
        string search, FilterBag filterBag);

    Task<UpdateIoTCihazDto> GetByIdAsync(int id);
    Task<CreateIoTCihazDto> AddAsync(CreateIoTCihazDto dto);
    Task<UpdateIoTCihazDto> UpdateAsync(UpdateIoTCihazDto dto);
    Task DeleteAsync(int id);
    Task DeleteDatabaseAsync(int id);
    Task<bool> AnyAsync(int id);
    Task<List<RaporAnasayfa>> RaporAsync();
}