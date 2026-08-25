using Core.Repository;
using Entity.Concrete;

namespace Repository.Abstract;

public interface IDurumRepository : IGenericRepository<Durum, int>
{
    // Durum'a özel metodlar buraya eklenebilir
} 