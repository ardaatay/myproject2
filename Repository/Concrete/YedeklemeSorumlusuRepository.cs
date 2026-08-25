using Core.Repository;
using Entity.Concrete;
using Repository.Abstract;
using Repository.Context;

namespace Repository.Concrete;

public class YedeklemeSorumlusuRepository(VarlikEnvanteriDbContext context)
    : GenericRepository<YedeklemeSorumlusu, int>(context), IYedeklemeSorumlusuRepository; 