using Core.UnitOfWork;
using Repository.Context;

namespace Repository.UnitOfWork;

public class VarlikEnvanteriUnitOfWork : Core.UnitOfWork.UnitOfWork, IVarlikEnvanteriUnitOfWork
{
    public VarlikEnvanteriUnitOfWork(VarlikEnvanteriDbContext context) : base(context)
    {
        Context = context;
    }

    public VarlikEnvanteriDbContext Context { get; }
} 