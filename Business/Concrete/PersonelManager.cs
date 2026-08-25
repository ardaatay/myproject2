using AutoMapper;
using Business.Abstract;
using Core.Aspects;
using Core.Exceptions;
using Dto.DTOs;
using Dto.Durum.Enum;
using Dto.Personel;
using Dto.Rapor;
using Entity.Concrete;
using Microsoft.AspNetCore.Http;
using Repository.Abstract;
using Repository.UnitOfWork;
using Util.Query;

namespace Business.Concrete;

public class PersonelManager(
    IVarlikEnvanteriUnitOfWork unitOfWork,
    IPersonelRepository personelRepository,
    IMapper mapper,
    IHttpContextAccessor httpContextAccessor) : IPersonelService
{
    public async Task<DataTablesResponse<ListPersonelDto>> GetAllAsync(DataTablesRequest request)
    {
        var user = httpContextAccessor.HttpContext.User;

        if (user.IsInRole("ADMIN"))
        {
            return await personelRepository.ProcessTableRequestAsync(request);
        }
        else
        {
            var birimId = user.Claims.FirstOrDefault(c => c.Type == "BirimId")?.Value;

            return await personelRepository.ProcessTableRequestAsync(request,
                x => (x.VarlikSahibiId == Convert.ToInt32(birimId) ||
                      x.OperasyonelSahibiId == Convert.ToInt32(birimId)));
        }
    }

    public async Task<List<ListPersonelDto>> GetAllExcelAsync()
    {
        var user = httpContextAccessor.HttpContext.User;

        if (user.IsInRole("ADMIN"))
        {
            return await personelRepository.GetListWithDetailsAsync();
        }
        else
        {
            var birimId = user.Claims.FirstOrDefault(c => c.Type == "BirimId")?.Value;

            return await personelRepository.GetListWithDetailsAsync(x =>
                (x.VarlikSahibiId == Convert.ToInt32(birimId) ||
                 x.OperasyonelSahibiId == Convert.ToInt32(birimId)));
        }
    }

    public async Task<List<ListPersonelDto>> GetAllExcelAsync(
        string search, FilterBag filterBag)
    {
        var user = httpContextAccessor.HttpContext.User;

        if (user.IsInRole("ADMIN"))
        {
            return await personelRepository.GetListWithDetailsAsync(
                search, filterBag);
        }
        else
        {
            var birimId = user.Claims.FirstOrDefault(c => c.Type == "BirimId")?.Value;

            return await personelRepository.GetListWithDetailsAsync(
                search, filterBag,
                x =>
                    (x.VarlikSahibiId == Convert.ToInt32(birimId) ||
                     x.OperasyonelSahibiId == Convert.ToInt32(birimId)));
        }
    }

    public async Task<UpdatePersonelDto> GetByIdAsync(int id)
    {
        var repository = unitOfWork.GetRepository<Personel, int>();
        var user = httpContextAccessor.HttpContext.User;

        Personel? entity;

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

        return mapper.Map<UpdatePersonelDto>(entity);
    }

    [LogAspect]
    public async Task<CreatePersonelDto> AddAsync(CreatePersonelDto dto)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var entity = mapper.Map<Personel>(dto);
            var repository = unitOfWork.GetRepository<Personel, int>();

            await repository.AddAsync(entity);
            return mapper.Map<CreatePersonelDto>(entity);
        });
    }

    [LogAspect]
    public async Task<UpdatePersonelDto> UpdateAsync(UpdatePersonelDto dto)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<Personel, int>();
            var entity = await repository.GetAsync(x => x.Id == dto.Id);

            if (entity == null)
                throw new NotFoundException($"Personel with id {dto.Id} not found");

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
            return mapper.Map<UpdatePersonelDto>(entity);
        });
    }

    [LogAspect]
    public async Task DeleteAsync(int id)
    {
        await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<Personel, int>();
            var entity = await repository.GetAsync(x => x.Id == id);

            if (entity == null)
                throw new NotFoundException($"Personel with id {id} not found");

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
            var repository = unitOfWork.GetRepository<Personel, int>();
            var entity = await repository.GetAsync(x => x.Id == id);

            if (entity == null)
                throw new NotFoundException($"Personel with id {id} not found");

            entity.SilinsinMi = true;
            entity.EnvanterdenCikisTarihi = DateTime.Now;

            await repository.UpdateAsync(entity);
        });
    }

    public async Task<bool> AnyAsync(int id)
    {
        var repository = unitOfWork.GetRepository<Personel, int>();
        return await repository.AnyAsync(x => x.Id == id);
    }

    public async Task<List<RaporAnasayfa>> RaporAsync()
    {
        return await personelRepository.GetRaporPersonelAsync();
    }
}