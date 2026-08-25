using System.Linq.Expressions;
using Core.Repository;
using Dto.DTOs;
using Dto.Surec;
using Entity.Concrete;
using Util.Query;

namespace Repository.Abstract;

public interface ISurecRepository : IGenericRepository<Surec, int>
{
    // Surec'e özel metodlar buraya eklenebilir
    Task<List<ListSurecDto>> GetListWithDetailsAsync(
        Expression<Func<ListSurecDto, bool>>? filter = null);

    Task<List<ListSurecDto>> GetListWithDetailsAsync(
        string search, FilterBag filterBag,
        Expression<Func<ListSurecDto, bool>>? filter = null);

    Task<DataTablesResponse<ListSurecDto>> ProcessTableRequestAsync(
        DataTablesRequest request,
        Expression<Func<ListSurecDto, bool>>? filter = null);
}