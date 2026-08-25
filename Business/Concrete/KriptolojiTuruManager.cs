using AutoMapper;
using Business.Abstract;
using Core.Exceptions;
using Dto.KriptolojiTuru;
using Entity.Concrete;
using Repository.UnitOfWork;

namespace Business.Concrete;

public class KriptolojiTuruManager(IVarlikEnvanteriUnitOfWork unitOfWork, IMapper mapper) : IKriptolojiTuruService
{
    public async Task<List<ListKriptolojiTuruDto>> GetAllAsync()
    {
        var entities = await unitOfWork.GetRepository<KriptolojiTuru, int>().GetAllAsync();
        return mapper.Map<List<ListKriptolojiTuruDto>>(entities);
    }

    public async Task<UpdateKriptolojiTuruDto> GetByIdAsync(int id)
    {
        var entity = await unitOfWork.GetRepository<KriptolojiTuru, int>().GetAsync(x => x.Id == id);
        return mapper.Map<UpdateKriptolojiTuruDto>(entity);
    }

    public async Task<CreateKriptolojiTuruDto> AddAsync(CreateKriptolojiTuruDto dto)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var entity = mapper.Map<KriptolojiTuru>(dto);
            var repository = unitOfWork.GetRepository<KriptolojiTuru, int>();

            await repository.AddAsync(entity);
            return mapper.Map<CreateKriptolojiTuruDto>(entity);
        });
    }

    public async Task<UpdateKriptolojiTuruDto> UpdateAsync(UpdateKriptolojiTuruDto dto)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<KriptolojiTuru, int>();
            var entity = await repository.GetAsync(x => x.Id == dto.Id);

            if (entity == null)
                throw new NotFoundException($"KriptolojiTuru with id {dto.Id} not found");

            mapper.Map(dto, entity);
            await repository.UpdateAsync(entity);
            return mapper.Map<UpdateKriptolojiTuruDto>(entity);
        });
    }

    public async Task DeleteAsync(int id)
    {
        await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<KriptolojiTuru, int>();
            var entity = await repository.GetAsync(x => x.Id == id);

            if (entity == null)
                throw new NotFoundException($"KriptolojiTuru with id {id} not found");

            await repository.DeleteAsync(entity);
        });
    }

    public async Task<bool> AnyAsync(int id)
    {
        var repository = unitOfWork.GetRepository<KriptolojiTuru, int>();
        return await repository.AnyAsync(x => x.Id == id);
    }
}