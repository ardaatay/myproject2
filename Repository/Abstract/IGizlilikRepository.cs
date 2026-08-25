using Core.Repository;
using Entity.Concrete;

namespace Repository.Abstract;

public interface IGizlilikRepository : IGenericRepository<Gizlilik, int>
{
    // Gizlilik'e özel metodlar buraya eklenebilir
} 