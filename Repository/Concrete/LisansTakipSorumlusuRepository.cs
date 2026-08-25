using Core.Repository;
using Entity.Concrete;
using Repository.Abstract;
using Repository.Context;

namespace Repository.Concrete;

public class LisansTakipSorumlusuRepository(VarlikEnvanteriDbContext context)
    : GenericRepository<LisansTakipSorumlusu, int>(context), ILisansTakipSorumlusuRepository; 