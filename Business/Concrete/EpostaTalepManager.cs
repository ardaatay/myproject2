using AutoMapper;
using Business.Abstract;
using Core.Aspects;
using Core.Exceptions;
using Dto.DTOs;
using Dto.EpostaTalep;
using Dto.Kurum;
using Entity.Concrete;
using Repository.Abstract;
using Repository.UnitOfWork;

namespace Business.Concrete;

public class EpostaTalepManager(
    IVarlikEnvanteriUnitOfWork unitOfWork,
    IEpostaTalepRepository epostaTalepRepository,
    IKurumService kurumService,
    IMapper mapper) : IEpostaTalepService
{
      public async Task<DataTablesResponse<ListEpostaTalepDto>> GetAllAsync(DataTablesRequest request)
    {
        return await epostaTalepRepository.ProcessTableRequestAsync(request);
    }

    public async Task<List<ListEpostaTalepDto>> GetAllExcelAsync()
    {
        return await epostaTalepRepository.GetListWithDetailsAsync();
    }

    public async Task<List<ListEpostaTalepDto>> GetAllExcelAsync(string search)
    {
        return await epostaTalepRepository.GetListWithDetailsAsync(search);
    }

    public async Task<UpdateEpostaTalepDto> GetByIdAsync(int id)
    {
        var entity = await epostaTalepRepository.GetAsync(x => x.Id == id);

        return mapper.Map<UpdateEpostaTalepDto>(entity);
    }

    [LogAspect]
    public async Task<CreateEpostaTalepDto> AddAsync(CreateEpostaTalepDto dto)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var entity = mapper.Map<EpostaTalep>(dto);
            var repository = unitOfWork.GetRepository<EpostaTalep, int>();
            
            var kurum= await kurumService.GetByIdAsync(dto.KurumId);

            entity.Ad = kurum.Ad;
            entity.CreatedDate = DateTime.Now;
            entity.Durum = true;

            await repository.AddAsync(entity);
            return mapper.Map<CreateEpostaTalepDto>(entity);
        });
    }

    [LogAspect]
    public async Task<UpdateEpostaTalepDto> UpdateAsync(UpdateEpostaTalepDto dto)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<EpostaTalep, int>();
            var entity = await repository.GetAsync(x => x.Id == dto.Id);

            if (entity == null)
                throw new NotFoundException($"EpostaTalep with id {dto.Id} not found");
            
            mapper.Map(dto, entity);

            var kurum= await kurumService.GetByIdAsync(dto.KurumId);

            entity.Ad = kurum.Ad;
            entity.UpdatedDate = DateTime.Now;

            await repository.UpdateAsync(entity);
            return mapper.Map<UpdateEpostaTalepDto>(entity);
        });
    }

    [LogAspect]
    public async Task DeleteAsync(int id)
    {
        await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<EpostaTalep, int>();
            var entity = await repository.GetAsync(x => x.Id == id);

            if (entity == null)
                throw new NotFoundException($"EpostaTalep with id {id} not found");

            entity.DeletedDate = DateTime.Now;
            entity.Durum = false;

            await repository.UpdateAsync(entity);
        });
    }

    [LogAspect]
    public async Task DeleteDatabaseAsync(int id)
    {
        await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<EpostaTalep, int>();
            var entity = await repository.GetAsync(x => x.Id == id);

            if (entity == null)
                throw new NotFoundException($"EpostaTalep with id {id} not found");

            await repository.DeleteAsync(entity);
        });
    }

    public async Task<bool> AnyAsync(int id)
    {
        var repository = unitOfWork.GetRepository<EpostaTalep, int>();
        return await repository.AnyAsync(x => x.Id == id);
    }
}