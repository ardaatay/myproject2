using Core.Repository;
using Entity.Concrete;

namespace Repository.Abstract;

public interface IKategoriRepository : IGenericRepository<Kategori, int>
{
    // Kategori'ye özel metodlar buraya eklenebilir
} 