using Core.Repository;
using Entity.Concrete;
using Repository.Abstract;
using Repository.Context;

namespace Repository.Concrete;

public class BagimliVarliklarRepository(VarlikEnvanteriDbContext context)
    : GenericRepository<BagimliVarlik, int>(context), IBagimliVarliklarRepository; 