using System.Linq.Expressions;
using Core.Repository;
using Dto.DTOs;
using Dto.Kullanici;
using Entity.Concrete;

namespace Repository.Abstract;

public interface IKullaniciRepository : IGenericRepository<Kullanici, int>
{
    Task<DataTablesResponse<ListKullaniciDto>> ProcessTableRequestAsync(
        DataTablesRequest request,
        Expression<Func<ListKullaniciDto, bool>>? filter = null);
    Task<List<KullaniciListeDto>> KullanicilariGetirAsync();
}