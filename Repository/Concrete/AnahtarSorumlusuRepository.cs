using Core.Repository;
using Entity.Concrete;
using Repository.Abstract;
using Repository.Context;

namespace Repository.Concrete;

public class AnahtarSorumlusuRepository(VarlikEnvanteriDbContext context)
    : GenericRepository<AnahtarSorumlusu, int>(context), IAnahtarSorumlusuRepository; 