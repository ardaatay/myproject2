using AutoMapper;
using Business.Abstract;
using Core.Exceptions;
using Dto.EtkilenenKisiSayisi;
using Entity.Concrete;
using Repository.UnitOfWork;

namespace Business.Concrete;

public class EtkilenenKisiSayisiManager(IVarlikEnvanteriUnitOfWork unitOfWork, IMapper mapper)
    : IEtkilenenKisiSayisiService
{
    public async Task<List<ListEtkilenenKisiSayisiDto>> GetAllAsync()
    {
        var entities = await unitOfWork.GetRepository<EtkilenenKisiSayisi, int>().GetAllAsync();
        return mapper.Map<List<ListEtkilenenKisiSayisiDto>>(entities);
    }

    public async Task<UpdateEtkilenenKisiSayisiDto> GetByIdAsync(int id)
    {
        var entity = await unitOfWork.GetRepository<EtkilenenKisiSayisi, int>().GetAsync(x => x.Id == id);
        return mapper.Map<UpdateEtkilenenKisiSayisiDto>(entity);
    }

    public async Task<CreateEtkilenenKisiSayisiDto> AddAsync(CreateEtkilenenKisiSayisiDto dto)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var entity = mapper.Map<EtkilenenKisiSayisi>(dto);
            var repository = unitOfWork.GetRepository<EtkilenenKisiSayisi, int>();

            await repository.AddAsync(entity);
            return mapper.Map<CreateEtkilenenKisiSayisiDto>(entity);
        });
    }

    public async Task<UpdateEtkilenenKisiSayisiDto> UpdateAsync(UpdateEtkilenenKisiSayisiDto dto)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<EtkilenenKisiSayisi, int>();
            var entity = await repository.GetAsync(x => x.Id == dto.Id);

            if (entity == null)
                throw new NotFoundException($"EtkilenenKisiSayisi with id {dto.Id} not found");

            mapper.Map(dto, entity);
            await repository.UpdateAsync(entity);
            return mapper.Map<UpdateEtkilenenKisiSayisiDto>(entity);
        });
    }

    public async Task DeleteAsync(int id)
    {
        await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<EtkilenenKisiSayisi, int>();
            var entity = await repository.GetAsync(x => x.Id == id);

            if (entity == null)
                throw new NotFoundException($"EtkilenenKisiSayisi with id {id} not found");

            await repository.DeleteAsync(entity);
        });
    }

    public async Task<bool> AnyAsync(int id)
    {
        var repository = unitOfWork.GetRepository<EtkilenenKisiSayisi, int>();
        return await repository.AnyAsync(x => x.Id == id);
    }
}