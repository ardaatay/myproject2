using Core.Repository;
using Entity.Concrete;

namespace Repository.Abstract;

public interface IKullanimSeviyesiRepository : IGenericRepository<KullanimSeviyesi, int>
{
    // KullanimSeviyesi'ne özel metodlar buraya eklenebilir
} 