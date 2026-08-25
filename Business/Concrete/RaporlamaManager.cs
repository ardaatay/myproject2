using Business.Abstract;
using Dto.DTOs;
using Dto.Raporlama;
using Repository.Abstract;
using Util.Query;

namespace Business.Concrete;

public class RaporlamaManager(IRaporlamaRepository raporlamaRepository) : IRaporlamaService
{
    public async Task<DataTablesResponse<ListRaporDto>> GetAllAsync(DataTablesRequest request)
    {
        return await raporlamaRepository.ProcessTableRequestAsync(request);
    }

    public async Task<List<ListRaporDto>> GetAllExcelAsync(string? search = null, FilterBag? filterBag = null)
    {
        return await raporlamaRepository.GetAllExcelAsync(
            search, filterBag);
    }
}