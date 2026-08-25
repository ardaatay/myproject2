using Dto.DTOs;
using Dto.EpostaTalep;

namespace Business.Abstract;

public interface IEpostaTalepService
{
    Task<DataTablesResponse<ListEpostaTalepDto>> GetAllAsync(
        DataTablesRequest request);

    Task<List<ListEpostaTalepDto>> GetAllExcelAsync();

    Task<List<ListEpostaTalepDto>> GetAllExcelAsync(
        string search);

    Task<UpdateEpostaTalepDto> GetByIdAsync(int id);
    Task<CreateEpostaTalepDto> AddAsync(CreateEpostaTalepDto dto);
    Task<UpdateEpostaTalepDto> UpdateAsync(UpdateEpostaTalepDto dto);
    Task DeleteAsync(int id);
    Task DeleteDatabaseAsync(int id);
    Task<bool> AnyAsync(int id);
}