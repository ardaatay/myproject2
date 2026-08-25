using System.Linq.Expressions;
using Core.Repository;
using Dto.DTOs;
using Dto.Rapor;
using Dto.TasinabilirCihazveOrtam;
using Entity.Concrete;
using Util.Query;

namespace Repository.Abstract;

public interface ITasinabilirCihazveOrtamRepository : IGenericRepository<TasinabilirCihazveOrtam, int>
{
    // TasinabilirCihazveOrtam'a özel metodlar buraya eklenebilir
    Task<List<ListTasinabilirCihazveOrtamDto>> GetListWithDetailsAsync(
        Expression<Func<ListTasinabilirCihazveOrtamDto, bool>>? filter = null);

    Task<List<ListTasinabilirCihazveOrtamDto>> GetListWithDetailsAsync(
        string search, FilterBag filterBag,
        Expression<Func<ListTasinabilirCihazveOrtamDto, bool>>? filter = null);

    Task<DataTablesResponse<ListTasinabilirCihazveOrtamDto>> ProcessTableRequestAsync(
        DataTablesRequest request,
        Expression<Func<ListTasinabilirCihazveOrtamDto, bool>>? filter = null);

    Task<List<RaporAnasayfa>> GetRaporTasinabilirCihazveOrtamAsync();
}