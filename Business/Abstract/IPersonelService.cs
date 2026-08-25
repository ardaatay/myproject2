using Dto.DTOs;
using Dto.Personel;
using Dto.Rapor;
using Util.Query;

namespace Business.Abstract;

public interface IPersonelService
{
    Task<DataTablesResponse<ListPersonelDto>> GetAllAsync(DataTablesRequest request);

    Task<List<ListPersonelDto>> GetAllExcelAsync();

    Task<List<ListPersonelDto>> GetAllExcelAsync(
        string search, FilterBag filterBag);

    Task<UpdatePersonelDto> GetByIdAsync(int id);
    Task<CreatePersonelDto> AddAsync(CreatePersonelDto dto);
    Task<UpdatePersonelDto> UpdateAsync(UpdatePersonelDto dto);
    Task DeleteAsync(int id);
    Task DeleteDatabaseAsync(int id);
    Task<bool> AnyAsync(int id);
    Task<List<RaporAnasayfa>> RaporAsync();
}