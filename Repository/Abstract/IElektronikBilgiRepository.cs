using System.Linq.Expressions;
using Core.Repository;
using Dto.DTOs;
using Dto.ElektronikBilgi;
using Entity.Concrete;
using Util.Query;

namespace Repository.Abstract;

public interface IElektronikBilgiRepository : IGenericRepository<ElektronikBilgi, int>
{
    // ElektronikBilgi'ye özel metodlar buraya eklenebilir
    Task<List<ListElektronikBilgiDto>> GetListWithDetailsAsync(
        Expression<Func<ListElektronikBilgiDto, bool>>? filter = null);

    Task<List<ListElektronikBilgiDto>> GetListWithDetailsAsync(
        string search, FilterBag filterBag,
        Expression<Func<ListElektronikBilgiDto, bool>>? filter = null);

    Task<DataTablesResponse<ListElektronikBilgiDto>> ProcessTableRequestAsync(
        DataTablesRequest request,
        Expression<Func<ListElektronikBilgiDto, bool>>? filter = null);
}