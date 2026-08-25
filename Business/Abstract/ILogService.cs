using Entity.Concrete;

namespace Business.Abstract;

public interface ILogService
{
    void Add(Log entity);
}