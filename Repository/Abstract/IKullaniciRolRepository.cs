using Core.Repository;
using Dto.Kullanici;
using Entity.Concrete;

namespace Repository.Abstract;

public interface IKullaniciRolRepository : IGenericRepository<KullaniciRol, int>
{
    Task<KullaniciRolAtamaDto?> KullaniciRolleriniGetirAsync(int kullaniciId);
}