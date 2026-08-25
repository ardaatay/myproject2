using System.Linq.Expressions;
using Core.Repository;
using Dto.DTOs;
using Dto.FizikselMekan;
using Dto.Rapor;
using Entity.Concrete;
using Util.Query;

namespace Repository.Abstract;

public interface IFizikselMekanRepository : IGenericRepository<FizikselMekan, int>
{
    // FizikselMekan'a özel metodlar buraya eklenebilir
    Task<List<ListFizikselMekanDto>> GetListWithDetailsAsync(
        Expression<Func<ListFizikselMekanDto, bool>>? filter = null);

    Task<List<ListFizikselMekanDto>> GetListWithDetailsAsync(
        string search, FilterBag filterBag,
        Expression<Func<ListFizikselMekanDto, bool>>? filter = null);

    Task<DataTablesResponse<ListFizikselMekanDto>> ProcessTableRequestAsync(
        DataTablesRequest request,
        Expression<Func<ListFizikselMekanDto, bool>>? filter = null);

    Task<List<RaporAnasayfa>> GetRaporFizikselMekanlarAsync();
}