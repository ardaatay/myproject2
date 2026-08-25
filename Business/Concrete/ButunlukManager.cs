using AutoMapper;
using Business.Abstract;
using Core.Exceptions;
using Dto.Butunluk;
using Entity.Concrete;
using Repository.UnitOfWork;

namespace Business.Concrete;

public class ButunlukManager(IVarlikEnvanteriUnitOfWork unitOfWork, IMapper mapper) : IButunlukService
{
    public async Task<List<ListButunlukDto>> GetAllAsync()
    {
        var entities = await unitOfWork.GetRepository<Butunluk, int>().GetAllAsync();
        return mapper.Map<List<ListButunlukDto>>(entities);
    }

    public async Task<UpdateButunlukDto> GetByIdAsync(int id)
    {
        var entity = await unitOfWork.GetRepository<Butunluk, int>().GetAsync(x => x.Id == id);
        return mapper.Map<UpdateButunlukDto>(entity);
    }

    public async Task<CreateButunlukDto> AddAsync(CreateButunlukDto dto)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var entity = mapper.Map<Butunluk>(dto);
            var repository = unitOfWork.GetRepository<Butunluk, int>();

            await repository.AddAsync(entity);
            return mapper.Map<CreateButunlukDto>(entity);
        });
    }

    public async Task<UpdateButunlukDto> UpdateAsync(UpdateButunlukDto dto)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<Butunluk, int>();
            var entity = await repository.GetAsync(x => x.Id == dto.Id);

            if (entity == null)
                throw new NotFoundException($"Butunluk with id {dto.Id} not found");

            mapper.Map(dto, entity);
            await repository.UpdateAsync(entity);
            return mapper.Map<UpdateButunlukDto>(entity);
        });
    }

    public async Task DeleteAsync(int id)
    {
        await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<Butunluk, int>();
            var entity = await repository.GetAsync(x => x.Id == id);

            if (entity == null)
                throw new NotFoundException($"Butunluk with id {id} not found");

            await repository.DeleteAsync(entity);
        });
    }

    public async Task<bool> AnyAsync(int id)
    {
        var repository = unitOfWork.GetRepository<Butunluk, int>();
        return await repository.AnyAsync(x => x.Id == id);
    }
}