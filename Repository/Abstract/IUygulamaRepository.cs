using System.Linq.Expressions;
using Core.Repository;
using Dto.DTOs;
using Dto.Rapor;
using Dto.Uygulama;
using Entity.Concrete;
using Util.Query;

namespace Repository.Abstract;

public interface IUygulamaRepository : IGenericRepository<Uygulama, int>
{
    // Uygulama'ya özel metodlar buraya eklenebilir
    Task<List<ListUygulamaDto>> GetListWithDetailsAsync(
        Expression<Func<ListUygulamaDto, bool>>? filter = null);

    Task<List<ListUygulamaDto>> GetListWithDetailsAsync(
        string search, FilterBag filterBag,
        Expression<Func<ListUygulamaDto, bool>>? filter = null);

    Task<DataTablesResponse<ListUygulamaDto>> ProcessTableRequestAsync(
        DataTablesRequest request,
        Expression<Func<ListUygulamaDto, bool>>? filter = null);

    Task<List<RaporAnasayfa>> GetRaporUygulamalarAsync();
}