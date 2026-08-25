using Core.Repository;
using Dto.Kullanici;
using Entity.Concrete;

namespace Repository.Abstract;

public interface IRolRepository : IGenericRepository<Rol, int>
{
    Task RolleriKaydetAsync(KullaniciRolAtamaDto model);
}