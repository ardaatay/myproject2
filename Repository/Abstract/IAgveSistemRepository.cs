using Core.Repository;
using Dto.AgveSistem;
using Dto.DTOs;
using Dto.Rapor;
using Entity.Concrete;
using System.Linq.Expressions;
using Util.Query;

namespace Repository.Abstract;

public interface IAgveSistemRepository : IGenericRepository<AgveSistem, int>
{
    // AgveSistem'e özel metodlar buraya eklenebilir
    Task<List<ListAgveSistemDto>> GetListWithDetailsAsync(
        Expression<Func<ListAgveSistemDto, bool>>? filter = null);

    Task<List<ListAgveSistemDto>> GetListWithDetailsAsync(
        string search,
        FilterBag filterBag,
        Expression<Func<ListAgveSistemDto, bool>>? filter = null);

    Task<DataTablesResponse<ListAgveSistemDto>> ProcessTableRequestAsync(
        DataTablesRequest request,
        Expression<Func<ListAgveSistemDto, bool>>? filter = null);

    Task<List<RaporAnasayfa>> GetRaporAgveSistemlerAsync();
}