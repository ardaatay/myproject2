using AutoMapper;
using Business.Abstract;
using Core.Aspects;
using Core.Exceptions;
using Dto.DTOs;
using Dto.Durum.Enum;
using Dto.ElektronikBilgi;
using Entity.Concrete;
using Microsoft.AspNetCore.Http;
using Repository.Abstract;
using Repository.UnitOfWork;
using Util.Query;

namespace Business.Concrete;

public class ElektronikBilgiManager(
    IVarlikEnvanteriUnitOfWork unitOfWork,
    IElektronikBilgiRepository elektronikBilgiRepository,
    IMapper mapper,
    IHttpContextAccessor httpContextAccessor) : IElektronikBilgiService
{
    public async Task<DataTablesResponse<ListElektronikBilgiDto>> GetAllAsync(DataTablesRequest request)
    {
        var user = httpContextAccessor.HttpContext.User;

        if (user.IsInRole("ADMIN"))
        {
            return await elektronikBilgiRepository.ProcessTableRequestAsync(request);
        }
        else
        {
            var birimId = user.Claims.FirstOrDefault(c => c.Type == "BirimId")?.Value;

            return await elektronikBilgiRepository.ProcessTableRequestAsync(request,
                x => (x.VarlikSahibiId == Convert.ToInt32(birimId) ||
                      x.OperasyonelSahibiId == Convert.ToInt32(birimId)));
        }
    }

    public async Task<List<ListElektronikBilgiDto>> GetAllExcelAsync()
    {
        var user = httpContextAccessor.HttpContext.User;

        if (user.IsInRole("ADMIN"))
        {
            return await elektronikBilgiRepository.GetListWithDetailsAsync();
        }
        else
        {
            var birimId = user.Claims.FirstOrDefault(c => c.Type == "BirimId")?.Value;

            return await elektronikBilgiRepository.GetListWithDetailsAsync(x =>
                (x.VarlikSahibiId == Convert.ToInt32(birimId) ||
                 x.OperasyonelSahibiId == Convert.ToInt32(birimId)));
        }
    }
    
    public async Task<List<ListElektronikBilgiDto>> GetAllExcelAsync(
        string search, FilterBag filterBag)
    {
        var user = httpContextAccessor.HttpContext.User;

        if (user.IsInRole("ADMIN"))
        {
            return await elektronikBilgiRepository.GetListWithDetailsAsync(
                search, filterBag);
        }
        else
        {
            var birimId = user.Claims.FirstOrDefault(c => c.Type == "BirimId")?.Value;

            return await elektronikBilgiRepository.GetListWithDetailsAsync(
                search, filterBag,
                x =>
                (x.VarlikSahibiId == Convert.ToInt32(birimId) ||
                 x.OperasyonelSahibiId == Convert.ToInt32(birimId)));
        }
    }

    public async Task<UpdateElektronikBilgiDto> GetByIdAsync(int id)
    {
        var repository = unitOfWork.GetRepository<ElektronikBilgi, int>();
        var user = httpContextAccessor.HttpContext.User;

        ElektronikBilgi? entity;

        if (user.IsInRole("ADMIN"))
        {
            entity = await repository.GetAsync(x => x.Id == id);
        }
        else
        {
            var birimId = user.Claims.FirstOrDefault(c => c.Type == "BirimId")?.Value;

            entity = await repository.GetAsync(x =>
                x.Id == id && (x.VarlikSahibiId == Convert.ToInt32(birimId) ||
                               x.OperasyonelSahibiId == Convert.ToInt32(birimId)));
        }

        return mapper.Map<UpdateElektronikBilgiDto>(entity);
    }

    [LogAspect]
    public async Task<CreateElektronikBilgiDto> AddAsync(CreateElektronikBilgiDto dto)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var entity = mapper.Map<ElektronikBilgi>(dto);
            var repository = unitOfWork.GetRepository<ElektronikBilgi, int>();

            await repository.AddAsync(entity);
            return mapper.Map<CreateElektronikBilgiDto>(entity);
        });
    }

    [LogAspect]
    public async Task<UpdateElektronikBilgiDto> UpdateAsync(UpdateElektronikBilgiDto dto)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<ElektronikBilgi, int>();
            var entity = await repository.GetAsync(x => x.Id == dto.Id);

            if (entity == null)
                throw new NotFoundException($"ElektronikBilgi with id {dto.Id} not found");

            mapper.Map(dto, entity);

            if (entity.DurumId != (int)DurumEnum.HurdaImha)
            {
                entity.EnvanterGuncellemeTarihi = DateTime.Now;
                entity.EnvanterdenCikisTarihi = null;
            }

            if (entity.DurumId == (int)DurumEnum.HurdaImha)
            {
                entity.EnvanterdenCikisTarihi = DateTime.Now;
            }

            await repository.UpdateAsync(entity);
            return mapper.Map<UpdateElektronikBilgiDto>(entity);
        });
    }

    [LogAspect]
    public async Task DeleteAsync(int id)
    {
        await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<ElektronikBilgi, int>();
            var entity = await repository.GetAsync(x => x.Id == id);

            if (entity == null)
                throw new NotFoundException($"ElektronikBilgi with id {id} not found");

            entity.EnvanterdenCikisTarihi = DateTime.Now;
            entity.DurumId = (int)DurumEnum.HurdaImha;

            await repository.UpdateAsync(entity);
        });
    }

    [LogAspect]
    public async Task DeleteDatabaseAsync(int id)
    {
        await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<ElektronikBilgi, int>();
            var entity = await repository.GetAsync(x => x.Id == id);

            if (entity == null)
                throw new NotFoundException($"ElektronikBilgi with id {id} not found");

            entity.SilinsinMi = true;
            entity.EnvanterdenCikisTarihi = DateTime.Now;

            await repository.UpdateAsync(entity);
        });
    }

    public async Task<bool> AnyAsync(int id)
    {
        var repository = unitOfWork.GetRepository<ElektronikBilgi, int>();
        return await repository.AnyAsync(x => x.Id == id);
    }
}