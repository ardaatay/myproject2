using Dto.DTOs;
using Dto.ElektronikBilgi;
using Util.Query;

namespace Business.Abstract;

public interface IElektronikBilgiService
{
    Task<DataTablesResponse<ListElektronikBilgiDto>> GetAllAsync(DataTablesRequest request);

    Task<List<ListElektronikBilgiDto>> GetAllExcelAsync();

    Task<List<ListElektronikBilgiDto>> GetAllExcelAsync(
        string search, FilterBag filterBag);

    Task<UpdateElektronikBilgiDto> GetByIdAsync(int id);
    Task<CreateElektronikBilgiDto> AddAsync(CreateElektronikBilgiDto dto);
    Task<UpdateElektronikBilgiDto> UpdateAsync(UpdateElektronikBilgiDto dto);
    Task DeleteAsync(int id);
    Task DeleteDatabaseAsync(int id);
    Task<bool> AnyAsync(int id);
}