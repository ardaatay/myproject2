using Core.Repository;
using Entity.Concrete;
using Repository.Abstract;
using Repository.Context;

namespace Repository.Concrete;

public class KonumRepository(VarlikEnvanteriDbContext context)
    : GenericRepository<Konum, int>(context), IKonumRepository; 