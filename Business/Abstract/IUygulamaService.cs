using Dto.DTOs;
using Dto.Rapor;
using Dto.Uygulama;
using Util.Query;

namespace Business.Abstract;

public interface IUygulamaService
{
    Task<DataTablesResponse<ListUygulamaDto>> GetAllAsync(
        DataTablesRequest request);
    Task<List<ListUygulamaDto>> GetAllExcelAsync();

    Task<List<ListUygulamaDto>> GetAllExcelAsync(
        string search, FilterBag filterBag);
    Task<UpdateUygulamaDto> GetByIdAsync(int id);
    Task<CreateUygulamaDto> AddAsync(CreateUygulamaDto dto);
    Task<UpdateUygulamaDto> UpdateAsync(UpdateUygulamaDto dto);
    Task DeleteAsync(int id);
    Task DeleteDatabaseAsync(int id);
    Task<bool> AnyAsync(int id);
    Task<List<RaporAnasayfa>> RaporAsync();
}