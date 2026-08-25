using AutoMapper;
using Business.Abstract;
using Core.Aspects;
using Core.Exceptions;
using Dto.AgveSistem;
using Dto.DTOs;
using Dto.Kurum;
using Entity.Concrete;
using Repository.Abstract;
using Repository.UnitOfWork;

namespace Business.Concrete;

public class KurumManager(
    IVarlikEnvanteriUnitOfWork unitOfWork,
    IKurumRepository kurumRepository,
    IMapper mapper) : IKurumService
{
    public async Task<DataTablesResponse<ListKurumDto>> GetAllAsync(DataTablesRequest request)
    {
        return await kurumRepository.ProcessTableRequestAsync(request);
    }

    public async Task<IEnumerable<ListKurumDto>> GetAllAsync()
    {
        var kurumList = await kurumRepository.GetAllAsync();
        return mapper.Map<List<ListKurumDto>>(kurumList);
    }

    public async Task<List<ListKurumDto>> GetAllExcelAsync()
    {
        return await kurumRepository.GetListWithDetailsAsync();
    }

    public async Task<List<ListKurumDto>> GetAllExcelAsync(string search)
    {
        return await kurumRepository.GetListWithDetailsAsync(search);
    }

    public async Task<UpdateKurumDto> GetByIdAsync(int id)
    {
        var entity = await kurumRepository.GetAsync(x => x.Id == id);

        return mapper.Map<UpdateKurumDto>(entity);
    }

    [LogAspect]
    public async Task<CreateKurumDto> AddAsync(CreateKurumDto dto)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var entity = mapper.Map<Kurum>(dto);
            var repository = unitOfWork.GetRepository<Kurum, int>();

            entity.CreatedDate = DateTime.Now;
            entity.Durum = true;

            await repository.AddAsync(entity);
            return mapper.Map<CreateKurumDto>(entity);
        });
    }

    [LogAspect]
    public async Task<UpdateKurumDto> UpdateAsync(UpdateKurumDto dto)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<Kurum, int>();
            var entity = await repository.GetAsync(x => x.Id == dto.Id);

            if (entity == null)
                throw new NotFoundException($"Kurum with id {dto.Id} not found");


            mapper.Map(dto, entity);

            entity.UpdatedDate = DateTime.Now;

            await repository.UpdateAsync(entity);
            return mapper.Map<UpdateKurumDto>(entity);
        });
    }

    [LogAspect]
    public async Task DeleteAsync(int id)
    {
        await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<Kurum, int>();
            var entity = await repository.GetAsync(x => x.Id == id);

            if (entity == null)
                throw new NotFoundException($"Kurum with id {id} not found");

            entity.DeletedDate = entity.Durum ? DateTime.Now : null;

            if (!entity.Durum)
                entity.UpdatedDate = DateTime.Now;

            entity.Durum = !entity.Durum;

            await repository.UpdateAsync(entity);
        });
    }

    [LogAspect]
    public async Task DeleteDatabaseAsync(int id)
    {
        await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<Kurum, int>();
            var entity = await repository.GetAsync(x => x.Id == id);

            if (entity == null)
                throw new NotFoundException($"Kurum with id {id} not found");

            await repository.DeleteAsync(entity);
        });
    }

    public async Task<bool> AnyAsync(int id)
    {
        var repository = unitOfWork.GetRepository<Kurum, int>();
        return await repository.AnyAsync(x => x.Id == id);
    }
}