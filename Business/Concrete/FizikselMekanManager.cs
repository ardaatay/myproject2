using AutoMapper;
using Business.Abstract;
using Core.Aspects;
using Core.Exceptions;
using Dto.DTOs;
using Dto.Durum.Enum;
using Dto.FizikselMekan;
using Dto.Rapor;
using Entity.Concrete;
using Microsoft.AspNetCore.Http;
using Repository.Abstract;
using Repository.UnitOfWork;
using Util.Query;

namespace Business.Concrete;

public class FizikselMekanManager(
    IVarlikEnvanteriUnitOfWork unitOfWork,
    IFizikselMekanRepository fizikselMekanRepository,
    IMapper mapper,
    IHttpContextAccessor httpContextAccessor) : IFizikselMekanService
{
    public async Task<DataTablesResponse<ListFizikselMekanDto>> GetAllAsync(DataTablesRequest request)
    {
        var user = httpContextAccessor.HttpContext.User;

        if (user.IsInRole("ADMIN"))
        {
            return await fizikselMekanRepository.ProcessTableRequestAsync(request);
        }
        else
        {
            var birimId = user.Claims.FirstOrDefault(c => c.Type == "BirimId")?.Value;

            return await fizikselMekanRepository.ProcessTableRequestAsync(request,
                x => (x.VarlikSahibiId == Convert.ToInt32(birimId) ||
                      x.OperasyonelSahibiId == Convert.ToInt32(birimId)));
        }
    }

    public async Task<List<ListFizikselMekanDto>> GetAllExcelAsync()
    {
        var user = httpContextAccessor.HttpContext.User;

        if (user.IsInRole("ADMIN"))
        {
            return await fizikselMekanRepository.GetListWithDetailsAsync();
        }
        else
        {
            var birimId = user.Claims.FirstOrDefault(c => c.Type == "BirimId")?.Value;

            return await fizikselMekanRepository.GetListWithDetailsAsync(x =>
                (x.VarlikSahibiId == Convert.ToInt32(birimId) ||
                 x.OperasyonelSahibiId == Convert.ToInt32(birimId)));
        }
    }

    public async Task<List<ListFizikselMekanDto>> GetAllExcelAsync(
        string search, FilterBag filterBag)
    {
        var user = httpContextAccessor.HttpContext.User;

        if (user.IsInRole("ADMIN"))
        {
            return await fizikselMekanRepository.GetListWithDetailsAsync(search, filterBag);
        }
        else
        {
            var birimId = user.Claims.FirstOrDefault(c => c.Type == "BirimId")?.Value;

            return await fizikselMekanRepository.GetListWithDetailsAsync(
                search, filterBag,
                x =>
                    (x.VarlikSahibiId == Convert.ToInt32(birimId) ||
                     x.OperasyonelSahibiId == Convert.ToInt32(birimId)));
        }
    }

    public async Task<UpdateFizikselMekanDto> GetByIdAsync(int id)
    {
        var repository = unitOfWork.GetRepository<FizikselMekan, int>();
        var user = httpContextAccessor.HttpContext.User;

        FizikselMekan? entity;

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

        return mapper.Map<UpdateFizikselMekanDto>(entity);
    }

    [LogAspect]
    public async Task<CreateFizikselMekanDto> AddAsync(CreateFizikselMekanDto dto)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var entity = mapper.Map<FizikselMekan>(dto);
            var repository = unitOfWork.GetRepository<FizikselMekan, int>();

            await repository.AddAsync(entity);
            return mapper.Map<CreateFizikselMekanDto>(entity);
        });
    }

    [LogAspect]
    public async Task<UpdateFizikselMekanDto> UpdateAsync(UpdateFizikselMekanDto dto)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<FizikselMekan, int>();
            var entity = await repository.GetAsync(x => x.Id == dto.Id);

            if (entity == null)
                throw new NotFoundException($"FizikselMekan with id {dto.Id} not found");

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
            return mapper.Map<UpdateFizikselMekanDto>(entity);
        });
    }

    [LogAspect]
    public async Task DeleteAsync(int id)
    {
        await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<FizikselMekan, int>();
            var entity = await repository.GetAsync(x => x.Id == id);

            if (entity == null)
                throw new NotFoundException($"FizikselMekan with id {id} not found");

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
            var repository = unitOfWork.GetRepository<FizikselMekan, int>();
            var entity = await repository.GetAsync(x => x.Id == id);

            if (entity == null)
                throw new NotFoundException($"FizikselMekan with id {id} not found");

            entity.SilinsinMi = true;
            entity.EnvanterdenCikisTarihi = DateTime.Now;

            await repository.UpdateAsync(entity);
        });
    }

    public async Task<bool> AnyAsync(int id)
    {
        var repository = unitOfWork.GetRepository<FizikselMekan, int>();
        return await repository.AnyAsync(x => x.Id == id);
    }

    public async Task<List<RaporAnasayfa>> RaporAsync()
    {
        return await fizikselMekanRepository.GetRaporFizikselMekanlarAsync();
    }
}