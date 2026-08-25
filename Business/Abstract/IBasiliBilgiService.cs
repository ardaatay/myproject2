using Dto.BasiliBilgi;
using Dto.DTOs;
using Util.Query;

namespace Business.Abstract;

public interface IBasiliBilgiService
{
    Task<DataTablesResponse<ListBasiliBilgiDto>> GetAllAsync(DataTablesRequest request);

    Task<List<ListBasiliBilgiDto>> GetAllExcelAsync();

    Task<List<ListBasiliBilgiDto>> GetAllExcelAsync(
        string search, FilterBag filterBag);

    Task<UpdateBasiliBilgiDto> GetByIdAsync(int id);
    Task<CreateBasiliBilgiDto> AddAsync(CreateBasiliBilgiDto dto);
    Task<UpdateBasiliBilgiDto> UpdateAsync(UpdateBasiliBilgiDto dto);
    Task DeleteAsync(int id);
    Task DeleteDatabaseAsync(int id);
    Task<bool> AnyAsync(int id);
}