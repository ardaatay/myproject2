using Core.Repository;
using Entity.Concrete;
using Repository.Abstract;
using Repository.Context;

namespace Repository.Concrete;

public class KullanimSeviyesiRepository(VarlikEnvanteriDbContext context)
    : GenericRepository<KullanimSeviyesi, int>(context), IKullanimSeviyesiRepository; 