using Business.Abstract;
using Entity.Concrete;
using Repository.Abstract;

namespace Business.Concrete;

public class LogManager(ILogRepository repository) : ILogService
{
    public void Add(Log entity)
    {
        repository.AddLog(entity);
    }
}