using Core.Repository;
using Entity.Concrete;
using Repository.Abstract;
using Repository.Context;

namespace Repository.Concrete;

public class GizlilikRepository(VarlikEnvanteriDbContext context)
    : GenericRepository<Gizlilik, int>(context), IGizlilikRepository; 