using Dto.DTOs;
using Dto.Raporlama;
using Util.Query;

namespace Business.Abstract;

public interface IRaporlamaService
{
    Task<DataTablesResponse<ListRaporDto>> GetAllAsync(
        DataTablesRequest request);

    Task<List<ListRaporDto>> GetAllExcelAsync(
        string? search = null, FilterBag? filterBag = null);
}