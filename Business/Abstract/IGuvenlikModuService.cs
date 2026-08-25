namespace Business.Abstract;

public interface IGuvenlikModuService
{
    Task<bool> UpdateGuvenlikModu(bool durum);
    Task<bool> GetGuvenlikModuDurumu();
}