using Dto.BagimliVarliklar;

namespace Business.Abstract;

public interface IBagimliVarliklarService
{
    Task<List<ListBagimliVarliklarDto>> GetAllAsync();
    Task<UpdateBagimliVarliklarDto> GetByIdAsync(int id);
    Task<CreateBagimliVarliklarDto> AddAsync(CreateBagimliVarliklarDto dto);
    Task<UpdateBagimliVarliklarDto> UpdateAsync(UpdateBagimliVarliklarDto dto);
    Task DeleteAsync(int id);
    Task<bool> AnyAsync(int id);
} 