using Dto.DTOs;
using Dto.Veritabani;
using Util.Query;

namespace Business.Abstract;

public interface IVeritabaniService
{
    Task<DataTablesResponse<ListVeritabaniDto>> GetAllAsync(DataTablesRequest request);

    Task<List<ListVeritabaniDto>> GetAllExcelAsync();

    Task<List<ListVeritabaniDto>> GetAllExcelAsync(
        string search, FilterBag filterBag);

    Task<UpdateVeritabaniDto> GetByIdAsync(int id);
    Task<CreateVeritabaniDto> AddAsync(CreateVeritabaniDto dto);
    Task<UpdateVeritabaniDto> UpdateAsync(UpdateVeritabaniDto dto);
    Task DeleteAsync(int id);
    Task DeleteDatabaseAsync(int id);
    Task<bool> AnyAsync(int id);
}