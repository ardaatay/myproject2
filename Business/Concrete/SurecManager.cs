using AutoMapper;
using Business.Abstract;
using Core.Aspects;
using Core.Exceptions;
using Dto.DTOs;
using Dto.Durum.Enum;
using Dto.Surec;
using Entity.Concrete;
using Microsoft.AspNetCore.Http;
using Repository.Abstract;
using Repository.UnitOfWork;
using Util.Query;

namespace Business.Concrete;

public class SurecManager(
    IVarlikEnvanteriUnitOfWork unitOfWork,
    ISurecRepository surecRepository,
    IMapper mapper,
    IHttpContextAccessor httpContextAccessor) : ISurecService
{
    public async Task<DataTablesResponse<ListSurecDto>> GetAllAsync(DataTablesRequest request)
    {
        var user = httpContextAccessor.HttpContext.User;

        if (user.IsInRole("ADMIN"))
        {
            return await surecRepository.ProcessTableRequestAsync(request);
        }
        else
        {
            var birimId = user.Claims.FirstOrDefault(c => c.Type == "BirimId")?.Value;

            return await surecRepository.ProcessTableRequestAsync(request,
                x => (x.VarlikSahibiId == Convert.ToInt32(birimId) ||
                      x.OperasyonelSahibiId == Convert.ToInt32(birimId)));
        }
    }

    public async Task<List<ListSurecDto>> GetAllExcelAsync()
    {
        var user = httpContextAccessor.HttpContext.User;

        if (user.IsInRole("ADMIN"))
        {
            return await surecRepository.GetListWithDetailsAsync();
        }
        else
        {
            var birimId = user.Claims.FirstOrDefault(c => c.Type == "BirimId")?.Value;

            return await surecRepository.GetListWithDetailsAsync(x =>
                (x.VarlikSahibiId == Convert.ToInt32(birimId) ||
                 x.OperasyonelSahibiId == Convert.ToInt32(birimId)));
        }
    }

    public async Task<List<ListSurecDto>> GetAllExcelAsync(
        string search, FilterBag filterBag)
    {
        var user = httpContextAccessor.HttpContext.User;

        if (user.IsInRole("ADMIN"))
        {
            return await surecRepository.GetListWithDetailsAsync(
                search, filterBag);
        }
        else
        {
            var birimId = user.Claims.FirstOrDefault(c => c.Type == "BirimId")?.Value;

            return await surecRepository.GetListWithDetailsAsync(
                search, filterBag,
                x =>
                    (x.VarlikSahibiId == Convert.ToInt32(birimId) ||
                     x.OperasyonelSahibiId == Convert.ToInt32(birimId)));
        }
    }

    public async Task<UpdateSurecDto> GetByIdAsync(int id)
    {
        var repository = unitOfWork.GetRepository<Surec, int>();
        var user = httpContextAccessor.HttpContext.User;

        Surec? entity;

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

        return mapper.Map<UpdateSurecDto>(entity);
    }

    [LogAspect]
    public async Task<CreateSurecDto> AddAsync(CreateSurecDto dto)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var entity = mapper.Map<Surec>(dto);
            var repository = unitOfWork.GetRepository<Surec, int>();

            await repository.AddAsync(entity);
            return mapper.Map<CreateSurecDto>(entity);
        });
    }

    [LogAspect]
    public async Task<UpdateSurecDto> UpdateAsync(UpdateSurecDto dto)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<Surec, int>();
            var entity = await repository.GetAsync(x => x.Id == dto.Id);

            if (entity == null)
                throw new NotFoundException($"Surec with id {dto.Id} not found");

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
            return mapper.Map<UpdateSurecDto>(entity);
        });
    }

    [LogAspect]
    public async Task DeleteAsync(int id)
    {
        await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<Surec, int>();
            var entity = await repository.GetAsync(x => x.Id == id);

            if (entity == null)
                throw new NotFoundException($"Surec with id {id} not found");

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
            var repository = unitOfWork.GetRepository<Surec, int>();
            var entity = await repository.GetAsync(x => x.Id == id);

            if (entity == null)
                throw new NotFoundException($"Surec with id {id} not found");

            entity.SilinsinMi = true;
            entity.EnvanterdenCikisTarihi = DateTime.Now;

            await repository.UpdateAsync(entity);
        });
    }

    public async Task<bool> AnyAsync(int id)
    {
        var repository = unitOfWork.GetRepository<Surec, int>();
        return await repository.AnyAsync(x => x.Id == id);
    }
}