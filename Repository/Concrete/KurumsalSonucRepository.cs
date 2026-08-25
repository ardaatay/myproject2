using Core.Repository;
using Entity.Concrete;
using Repository.Abstract;
using Repository.Context;


namespace Repository.Concrete
{
    public class KurumsalSonucRepository(VarlikEnvanteriDbContext context)
        : GenericRepository<KurumsalSonuc, int>(context), IKurumsalSonucRepository;
} 