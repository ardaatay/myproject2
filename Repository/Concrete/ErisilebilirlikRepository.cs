using Core.Repository;
using Entity.Concrete;
using Repository.Abstract;
using Repository.Context;

namespace Repository.Concrete;

public class ErisilebilirlikRepository(VarlikEnvanteriDbContext context)
    : GenericRepository<Erisilebilirlik, int>(context), IErisilebilirlikRepository; 