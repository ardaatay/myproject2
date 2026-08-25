using Core.Repository;
using Entity.Concrete;
using Repository.Abstract;
using Repository.Context;

namespace Repository.Concrete;

public class KriptolojiTuruRepository(VarlikEnvanteriDbContext context)
    : GenericRepository<KriptolojiTuru, int>(context), IKriptolojiTuruRepository; 