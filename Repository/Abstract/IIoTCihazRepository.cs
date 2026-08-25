using System.Linq.Expressions;
using Core.Repository;
using Dto.DTOs;
using Dto.IoTCihaz;
using Dto.Rapor;
using Entity.Concrete;
using Util.Query;

namespace Repository.Abstract;

public interface IIoTCihazRepository : IGenericRepository<IoTCihaz, int>
{
    // IoTCihaz'a özel metodlar buraya eklenebilir
    Task<List<ListIoTCihazDto>> GetListWithDetailsAsync(
        Expression<Func<ListIoTCihazDto, bool>>? filter = null);

    Task<List<ListIoTCihazDto>> GetListWithDetailsAsync(
        string search, FilterBag filterBag,
        Expression<Func<ListIoTCihazDto, bool>>? filter = null);

    Task<DataTablesResponse<ListIoTCihazDto>> ProcessTableRequestAsync(
        DataTablesRequest request,
        Expression<Func<ListIoTCihazDto, bool>>? filter = null);

    Task<List<RaporAnasayfa>> GetRaporIoTCihazlarAsync();
}