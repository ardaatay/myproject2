using Core.Repository;
using Entity.Concrete;

namespace Repository.Abstract;

public interface IKonumRepository : IGenericRepository<Konum, int>
{
    // Konum'a özel metodlar buraya eklenebilir
} 