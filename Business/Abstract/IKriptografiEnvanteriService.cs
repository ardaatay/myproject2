using Dto.DTOs;
using Dto.KriptografiEnvanteri;

namespace Business.Abstract;

public interface IKriptografiEnvanteriService
{
    Task<DataTablesResponse<ListKriptografiEnvanteriDto>> GetAllAsync(DataTablesRequest request);
    Task<List<ListKriptografiEnvanteriDto>> GetAllExcelAsync();
    Task<List<ListKriptografiEnvanteriDto>> GetAllExcelAsync(string search);
    Task<UpdateKriptografiEnvanteriDto> GetByIdAsync(int id);
    Task<CreateKriptografiEnvanteriDto> AddAsync(CreateKriptografiEnvanteriDto dto);
    Task<UpdateKriptografiEnvanteriDto> UpdateAsync(UpdateKriptografiEnvanteriDto dto);
    Task DeleteAsync(int id);
    Task DeleteDatabaseAsync(int id);
    Task<bool> AnyAsync(int id);
}