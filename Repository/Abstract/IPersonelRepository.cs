using System.Linq.Expressions;
using Core.Repository;
using Dto.DTOs;
using Dto.Personel;
using Dto.Rapor;
using Entity.Concrete;
using Util.Query;

namespace Repository.Abstract;

public interface IPersonelRepository : IGenericRepository<Personel, int>
{
    // Personel'e özel metodlar buraya eklenebilir
    Task<List<ListPersonelDto>> GetListWithDetailsAsync(
        Expression<Func<ListPersonelDto, bool>>? filter = null);

    Task<List<ListPersonelDto>> GetListWithDetailsAsync(
        string search, FilterBag filterBag,
        Expression<Func<ListPersonelDto, bool>>? filter = null);

    Task<DataTablesResponse<ListPersonelDto>> ProcessTableRequestAsync(
        DataTablesRequest request,
        Expression<Func<ListPersonelDto, bool>>? filter = null);

    Task<List<RaporAnasayfa>> GetRaporPersonelAsync();
}