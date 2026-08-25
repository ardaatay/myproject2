using Core.Repository;
using Entity.Concrete;

namespace Repository.Abstract;

public interface IYedeklemeSorumlusuRepository : IGenericRepository<YedeklemeSorumlusu, int>
{
    // YedeklemeSorumlusu'na özel metodlar buraya eklenebilir
} 