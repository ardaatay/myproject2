using System.Linq.Expressions;
using Core.Repository;
using Dto.BasiliBilgi;
using Dto.DTOs;
using Entity.Concrete;
using Util.Query;

namespace Repository.Abstract;

public interface IBasiliBilgiRepository : IGenericRepository<BasiliBilgi, int>
{
    // BasiliBilgi'ye özel metodlar buraya eklenebilir
    Task<List<ListBasiliBilgiDto>> GetListWithDetailsAsync(
        Expression<Func<ListBasiliBilgiDto, bool>>? filter = null);

    Task<List<ListBasiliBilgiDto>> GetListWithDetailsAsync(
        string search, FilterBag filterBag,
        Expression<Func<ListBasiliBilgiDto, bool>>? filter = null);

    Task<DataTablesResponse<ListBasiliBilgiDto>> ProcessTableRequestAsync(
        DataTablesRequest request,
        Expression<Func<ListBasiliBilgiDto, bool>>? filter = null);
}