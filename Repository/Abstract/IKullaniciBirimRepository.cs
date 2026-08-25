using System.Linq.Expressions;
using Core.Repository;
using Dto.DTOs;
using Dto.KullaniciBirim;
using Entity.Concrete;

namespace Repository.Abstract;

public interface IKullaniciBirimRepository : IGenericRepository<KullaniciBirim, int>
{
    Task<DataTablesResponse<ListKullaniciBirimDto>> ProcessTableRequestAsync(
        DataTablesRequest request,
        Expression<Func<ListKullaniciBirimDto, bool>>? filter = null);

    Task<ListKullaniciBirimDto?> GetKullaniciBirimById(int id);

    Task<List<KullaniciBirim>> GetByKullaniciIdAsync(int kullaniciId);
}