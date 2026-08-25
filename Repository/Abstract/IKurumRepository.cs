using System.Linq.Expressions;
using Core.Repository;
using Dto.DTOs;
using Dto.Kurum;
using Entity.Concrete;

namespace Repository.Abstract;

public interface IKurumRepository : IGenericRepository<Kurum, int>
{
    Task<List<ListKurumDto>> GetListWithDetailsAsync(
        Expression<Func<ListKurumDto, bool>>? filter = null);

    Task<List<ListKurumDto>> GetListWithDetailsAsync(
        string search,
        Expression<Func<ListKurumDto, bool>>? filter = null);

    Task<DataTablesResponse<ListKurumDto>> ProcessTableRequestAsync(
        DataTablesRequest request,
        Expression<Func<ListKurumDto, bool>>? filter = null);
}