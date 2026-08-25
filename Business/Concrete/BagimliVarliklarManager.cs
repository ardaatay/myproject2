using AutoMapper;
using Business.Abstract;
using Core.Exceptions;
using Dto.BagimliVarliklar;
using Entity.Concrete;
using Repository.UnitOfWork;

namespace Business.Concrete;

public class BagimliVarliklarManager(IVarlikEnvanteriUnitOfWork unitOfWork, IMapper mapper) : IBagimliVarliklarService
{
    public async Task<List<ListBagimliVarliklarDto>> GetAllAsync()
    {
        var entities = await unitOfWork.GetRepository<BagimliVarlik, int>().GetAllAsync();
        return mapper.Map<List<ListBagimliVarliklarDto>>(entities);
    }

    public async Task<UpdateBagimliVarliklarDto> GetByIdAsync(int id)
    {
        var entity = await unitOfWork.GetRepository<BagimliVarlik, int>().GetAsync(x => x.Id == id);
        return mapper.Map<UpdateBagimliVarliklarDto>(entity);
    }

    public async Task<CreateBagimliVarliklarDto> AddAsync(CreateBagimliVarliklarDto dto)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var entity = mapper.Map<BagimliVarlik>(dto);
            var repository = unitOfWork.GetRepository<BagimliVarlik, int>();

            await repository.AddAsync(entity);
            return mapper.Map<CreateBagimliVarliklarDto>(entity);
        });
    }

    public async Task<UpdateBagimliVarliklarDto> UpdateAsync(UpdateBagimliVarliklarDto dto)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<BagimliVarlik, int>();
            var entity = await repository.GetAsync(x => x.Id == dto.Id);

            if (entity == null)
                throw new NotFoundException($"BagimliVarliklar with id {dto.Id} not found");

            mapper.Map(dto, entity);
            await repository.UpdateAsync(entity);
            return mapper.Map<UpdateBagimliVarliklarDto>(entity);
        });
    }

    public async Task DeleteAsync(int id)
    {
        await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<BagimliVarlik, int>();
            var entity = await repository.GetAsync(x => x.Id == id);

            if (entity == null)
                throw new NotFoundException($"BagimliVarliklar with id {id} not found");

            await repository.DeleteAsync(entity);
        });
    }

    public async Task<bool> AnyAsync(int id)
    {
        var repository = unitOfWork.GetRepository<BagimliVarlik, int>();
        return await repository.AnyAsync(x => x.Id == id);
    }
}