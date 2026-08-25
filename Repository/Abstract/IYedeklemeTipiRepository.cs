using Core.Repository;
using Entity.Concrete;

namespace Repository.Abstract;

public interface IYedeklemeTipiRepository : IGenericRepository<YedeklemeTipi, int>
{
    // YedeklemeTipi'ne özel metodlar buraya eklenebilir
}