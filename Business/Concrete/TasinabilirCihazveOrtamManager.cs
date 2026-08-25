using AutoMapper;
using Business.Abstract;
using Core.Aspects;
using Core.Exceptions;
using Dto.AgveSistem;
using Dto.DTOs;
using Dto.Durum.Enum;
using Dto.Rapor;
using Dto.TasinabilirCihazveOrtam;
using Entity.Concrete;
using Microsoft.AspNetCore.Http;
using Repository.Abstract;
using Repository.UnitOfWork;
using Util.Query;

namespace Business.Concrete;

public class TasinabilirCihazveOrtamManager(
    IVarlikEnvanteriUnitOfWork unitOfWork,
    ITasinabilirCihazveOrtamRepository tasinabilirCihazveOrtamRepository,
    IMapper mapper,
    IHttpContextAccessor httpContextAccessor)
    : ITasinabilirCihazveOrtamService
{
    public async Task<DataTablesResponse<ListTasinabilirCihazveOrtamDto>> GetAllAsync(
        DataTablesRequest request
    )
    {
        var user = httpContextAccessor.HttpContext.User;

        if (user.IsInRole("ADMIN"))
        {
            return await tasinabilirCihazveOrtamRepository.ProcessTableRequestAsync(request);
        }
        else
        {
            var birimId = user.Claims.FirstOrDefault(c => c.Type == "BirimId")?.Value;

            return await tasinabilirCihazveOrtamRepository.ProcessTableRequestAsync(request,
                x => (x.VarlikSahibiId == Convert.ToInt32(birimId) ||
                      x.OperasyonelSahibiId == Convert.ToInt32(birimId)));
        }
    }

    public async Task<List<ListTasinabilirCihazveOrtamDto>> GetAllExcelAsync()
    {
        var user = httpContextAccessor.HttpContext.User;

        if (user.IsInRole("ADMIN"))
        {
            return await tasinabilirCihazveOrtamRepository.GetListWithDetailsAsync();
        }
        else
        {
            var birimId = user.Claims.FirstOrDefault(c => c.Type == "BirimId")?.Value;

            return await tasinabilirCihazveOrtamRepository.GetListWithDetailsAsync(x =>
                (x.VarlikSahibiId == Convert.ToInt32(birimId) ||
                 x.OperasyonelSahibiId == Convert.ToInt32(birimId)));
        }
    }

    public async Task<List<ListTasinabilirCihazveOrtamDto>> GetAllExcelAsync(
        string search, FilterBag filterBag)
    {
        var user = httpContextAccessor.HttpContext.User;

        if (user.IsInRole("ADMIN"))
        {
            return await tasinabilirCihazveOrtamRepository.GetListWithDetailsAsync(
                search, filterBag);
        }
        else
        {
            var birimId = user.Claims.FirstOrDefault(c => c.Type == "BirimId")?.Value;

            return await tasinabilirCihazveOrtamRepository.GetListWithDetailsAsync(
                search,filterBag,
                x =>
                    (x.VarlikSahibiId == Convert.ToInt32(birimId) ||
                     x.OperasyonelSahibiId == Convert.ToInt32(birimId)));
        }
    }

    public async Task<UpdateTasinabilirCihazveOrtamDto> GetByIdAsync(int id)
    {
        var repository = unitOfWork.GetRepository<TasinabilirCihazveOrtam, int>();
        var user = httpContextAccessor.HttpContext.User;

        TasinabilirCihazveOrtam? entity;

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

        return mapper.Map<UpdateTasinabilirCihazveOrtamDto>(entity);
    }

    [LogAspect]
    public async Task<CreateTasinabilirCihazveOrtamDto> AddAsync(CreateTasinabilirCihazveOrtamDto dto)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<TasinabilirCihazveOrtam, int>();

            var exists = await repository.AnyAsync(x => x.VarlikAdi == dto.VarlikAdi && x.SilinsinMi != true);
            if (exists)
                throw new UniqueConstraintException($"'{dto.VarlikAdi}' adında bir taşınabilir cihaz/ortam varlığı zaten mevcut.");

            var entity = mapper.Map<TasinabilirCihazveOrtam>(dto);
            await repository.AddAsync(entity);
            return mapper.Map<CreateTasinabilirCihazveOrtamDto>(entity);
        });
    }

    [LogAspect]
    public async Task<UpdateTasinabilirCihazveOrtamDto> UpdateAsync(UpdateTasinabilirCihazveOrtamDto dto)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<TasinabilirCihazveOrtam, int>();
            var entity = await repository.GetAsync(x => x.Id == dto.Id);

            if (entity == null)
                throw new NotFoundException($"TasinabilirCihazveOrtam with id {dto.Id} not found");

            var exists = await repository.AnyAsync(x => x.VarlikAdi == dto.VarlikAdi && x.Id != dto.Id && x.SilinsinMi != true);
            if (exists)
                throw new UniqueConstraintException($"'{dto.VarlikAdi}' adında bir taşınabilir cihaz/ortam varlığı zaten mevcut.");

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
            return mapper.Map<UpdateTasinabilirCihazveOrtamDto>(entity);
        });
    }

    [LogAspect]
    public async Task DeleteAsync(int id)
    {
        await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<TasinabilirCihazveOrtam, int>();
            var entity = await repository.GetAsync(x => x.Id == id);

            if (entity == null)
                throw new NotFoundException($"TasinabilirCihazveOrtam with id {id} not found");

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
            var repository = unitOfWork.GetRepository<TasinabilirCihazveOrtam, int>();
            var entity = await repository.GetAsync(x => x.Id == id);

            if (entity == null)
                throw new NotFoundException($"TasinabilirCihazveOrtam with id {id} not found");

            entity.SilinsinMi = true;
            entity.EnvanterdenCikisTarihi = DateTime.Now;

            await repository.UpdateAsync(entity);
        });
    }

    public async Task<bool> AnyAsync(int id)
    {
        var repository = unitOfWork.GetRepository<TasinabilirCihazveOrtam, int>();
        return await repository.AnyAsync(x => x.Id == id);
    }

    public async Task<List<RaporAnasayfa>> RaporAsync()
    {
        return await tasinabilirCihazveOrtamRepository.GetRaporTasinabilirCihazveOrtamAsync();
    }
}