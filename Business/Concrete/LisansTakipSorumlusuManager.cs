using AutoMapper;
using Business.Abstract;
using Core.Exceptions;
using Dto.LisansTakipSorumlusu;
using Entity.Concrete;
using Repository.UnitOfWork;

namespace Business.Concrete;

public class LisansTakipSorumlusuManager(IVarlikEnvanteriUnitOfWork unitOfWork, IMapper mapper)
    : ILisansTakipSorumlusuService
{
    public async Task<List<ListLisansTakipSorumlusuDto>> GetAllAsync()
    {
        var entities = await unitOfWork.GetRepository<LisansTakipSorumlusu, int>().GetAllAsync();
        return mapper.Map<List<ListLisansTakipSorumlusuDto>>(entities);
    }

    public async Task<UpdateLisansTakipSorumlusuDto> GetByIdAsync(int id)
    {
        var entity = await unitOfWork.GetRepository<LisansTakipSorumlusu, int>().GetAsync(x => x.Id == id);
        return mapper.Map<UpdateLisansTakipSorumlusuDto>(entity);
    }

    public async Task<CreateLisansTakipSorumlusuDto> AddAsync(CreateLisansTakipSorumlusuDto dto)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var entity = mapper.Map<LisansTakipSorumlusu>(dto);
            var repository = unitOfWork.GetRepository<LisansTakipSorumlusu, int>();

            await repository.AddAsync(entity);
            return mapper.Map<CreateLisansTakipSorumlusuDto>(entity);
        });
    }

    public async Task<UpdateLisansTakipSorumlusuDto> UpdateAsync(UpdateLisansTakipSorumlusuDto dto)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<LisansTakipSorumlusu, int>();
            var entity = await repository.GetAsync(x => x.Id == dto.Id);

            if (entity == null)
                throw new NotFoundException($"LisansTakipSorumlusu with id {dto.Id} not found");

            mapper.Map(dto, entity);
            await repository.UpdateAsync(entity);
            return mapper.Map<UpdateLisansTakipSorumlusuDto>(entity);
        });
    }

    public async Task DeleteAsync(int id)
    {
        await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<LisansTakipSorumlusu, int>();
            var entity = await repository.GetAsync(x => x.Id == id);

            if (entity == null)
                throw new NotFoundException($"LisansTakipSorumlusu with id {id} not found");

            await repository.DeleteAsync(entity);
        });
    }

    public async Task<bool> AnyAsync(int id)
    {
        var repository = unitOfWork.GetRepository<LisansTakipSorumlusu, int>();
        return await repository.AnyAsync(x => x.Id == id);
    }
}