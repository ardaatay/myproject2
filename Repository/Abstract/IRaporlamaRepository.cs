using System.Linq.Expressions;
using Dto.DTOs;
using Dto.Raporlama;
using Util.Query;

namespace Repository.Abstract;

public interface IRaporlamaRepository
{
    Task<DataTablesResponse<ListRaporDto>> ProcessTableRequestAsync(
        DataTablesRequest request,
        Expression<Func<ListRaporDto, bool>>? filter = null);

    Task<List<ListRaporDto>> GetAllExcelAsync(
        string? search = null, FilterBag? filterBag = null);
}