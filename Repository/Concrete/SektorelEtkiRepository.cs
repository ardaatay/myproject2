using Core.Repository;
using Entity.Concrete;
using Repository.Abstract;
using Repository.Context;

namespace Repository.Concrete;

public class SektorelEtkiRepository(VarlikEnvanteriDbContext context)
    : GenericRepository<SektorelEtki, int>(context), ISektorelEtkiRepository; 