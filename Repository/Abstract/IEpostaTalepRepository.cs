using System.Linq.Expressions;
using Core.Repository;
using Dto.DTOs;
using Dto.EpostaTalep;
using Entity.Concrete;

namespace Repository.Abstract;

public interface IEpostaTalepRepository : IGenericRepository<EpostaTalep, int>
{
    Task<List<ListEpostaTalepDto>> GetListWithDetailsAsync(
        Expression<Func<ListEpostaTalepDto, bool>>? filter = null);

    Task<List<ListEpostaTalepDto>> GetListWithDetailsAsync(
        string search,
        Expression<Func<ListEpostaTalepDto, bool>>? filter = null);
    
    Task<DataTablesResponse<ListEpostaTalepDto>> ProcessTableRequestAsync(
        DataTablesRequest request,
        Expression<Func<ListEpostaTalepDto, bool>>? filter = null);
}