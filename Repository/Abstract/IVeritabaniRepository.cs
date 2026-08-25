using System.Linq.Expressions;
using Core.Repository;
using Dto.DTOs;
using Dto.Veritabani;
using Entity.Concrete;
using Util.Query;

namespace Repository.Abstract;

public interface IVeritabaniRepository : IGenericRepository<Veritabani, int>
{
    // Veritabani'na özel metodlar buraya eklenebilir
    Task<List<ListVeritabaniDto>> GetListWithDetailsAsync(
        Expression<Func<ListVeritabaniDto, bool>>? filter = null);

    Task<List<ListVeritabaniDto>> GetListWithDetailsAsync(
        string search, FilterBag filterBag,
        Expression<Func<ListVeritabaniDto, bool>>? filter = null);

    Task<DataTablesResponse<ListVeritabaniDto>> ProcessTableRequestAsync(
        DataTablesRequest request,
        Expression<Func<ListVeritabaniDto, bool>>? filter = null);
}