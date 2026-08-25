using Core.Repository;
using Entity.Concrete;
using Repository.Abstract;
using Repository.Context;

namespace Repository.Concrete;

public class KategoriRepository(VarlikEnvanteriDbContext context)
    : GenericRepository<Kategori, int>(context), IKategoriRepository; 