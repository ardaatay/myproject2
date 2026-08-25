using Dto.DTOs;
using Dto.FizikselMekan;
using Dto.Rapor;
using Util.Query;

namespace Business.Abstract;

public interface IFizikselMekanService
{
    Task<DataTablesResponse<ListFizikselMekanDto>> GetAllAsync(DataTablesRequest request);

    Task<List<ListFizikselMekanDto>> GetAllExcelAsync();

    Task<List<ListFizikselMekanDto>> GetAllExcelAsync(
        string search, FilterBag filterBag);

    Task<UpdateFizikselMekanDto> GetByIdAsync(int id);
    Task<CreateFizikselMekanDto> AddAsync(CreateFizikselMekanDto dto);
    Task<UpdateFizikselMekanDto> UpdateAsync(UpdateFizikselMekanDto dto);
    Task DeleteAsync(int id);
    Task DeleteDatabaseAsync(int id);
    Task<bool> AnyAsync(int id);
    Task<List<RaporAnasayfa>> RaporAsync();
}