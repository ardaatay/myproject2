using System.Linq.Expressions;
using Core.Repository;
using Dto.DTOs;
using Dto.KriptografiEnvanteri;
using Entity.Concrete;

namespace Repository.Abstract;

public interface IKriptografiEnvanteriRepository : IGenericRepository<KriptografiEnvanteri, int>
{
    // KriptografiEnvanteri'ne özel metodlar buraya eklenebilir
    Task<List<ListKriptografiEnvanteriDto>> GetListWithDetailsAsync(
        Expression<Func<ListKriptografiEnvanteriDto, bool>>? filter = null);

    Task<List<ListKriptografiEnvanteriDto>> GetListWithDetailsAsync(
        string search,
        Expression<Func<ListKriptografiEnvanteriDto, bool>>? filter = null);

    Task<DataTablesResponse<ListKriptografiEnvanteriDto>> ProcessTableRequestAsync(
        DataTablesRequest request,
        Expression<Func<ListKriptografiEnvanteriDto, bool>>? filter = null);
}