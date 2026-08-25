using AutoMapper;
using Business.Abstract;
using Core.Exceptions;
using Dto.Veritabani;
using Entity.Concrete;
using Repository.UnitOfWork;
using System.Xml;
using Core.Aspects;
using Dto.DTOs;
using Dto.Durum.Enum;
using Microsoft.AspNetCore.Http;
using Repository.Abstract;
using Util.Query;

namespace Business.Concrete;

public class VeritabaniManager(
    IVarlikEnvanteriUnitOfWork unitOfWork,
    IVeritabaniRepository veritabaniRepository,
    IMapper mapper,
    IHttpContextAccessor httpContextAccessor) : IVeritabaniService
{
    public async Task<DataTablesResponse<ListVeritabaniDto>> GetAllAsync(DataTablesRequest request)
    {
        var user = httpContextAccessor.HttpContext.User;

        if (user.IsInRole("ADMIN"))
        {
            return await veritabaniRepository.ProcessTableRequestAsync(request);
        }
        else
        {
            var birimId = user.Claims.FirstOrDefault(c => c.Type == "BirimId")?.Value;

            return await veritabaniRepository.ProcessTableRequestAsync(request,
                x => (x.VarlikSahibiId == Convert.ToInt32(birimId) ||
                      x.OperasyonelSahibiId == Convert.ToInt32(birimId)));
        }
    }

    public async Task<List<ListVeritabaniDto>> GetAllExcelAsync()
    {
        var user = httpContextAccessor.HttpContext.User;

        if (user.IsInRole("ADMIN"))
        {
            return await veritabaniRepository.GetListWithDetailsAsync();
        }
        else
        {
            var birimId = user.Claims.FirstOrDefault(c => c.Type == "BirimId")?.Value;

            return await veritabaniRepository.GetListWithDetailsAsync(x =>
                (x.VarlikSahibiId == Convert.ToInt32(birimId) ||
                 x.OperasyonelSahibiId == Convert.ToInt32(birimId)));
        }
    }

    public async Task<List<ListVeritabaniDto>> GetAllExcelAsync( string search, FilterBag filterBag)
    {
        var user = httpContextAccessor.HttpContext.User;

        if (user.IsInRole("ADMIN"))
        {
            return await veritabaniRepository.GetListWithDetailsAsync(
                search, filterBag);
        }
        else
        {
            var birimId = user.Claims.FirstOrDefault(c => c.Type == "BirimId")?.Value;

            return await veritabaniRepository.GetListWithDetailsAsync(
                search, filterBag,
                x =>
                    (x.VarlikSahibiId == Convert.ToInt32(birimId) ||
                     x.OperasyonelSahibiId == Convert.ToInt32(birimId)));
        }
    }

    public async Task<UpdateVeritabaniDto> GetByIdAsync(int id)
    {
        var repository = unitOfWork.GetRepository<Veritabani, int>();
        var user = httpContextAccessor.HttpContext.User;

        Veritabani? entity;

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

        return mapper.Map<UpdateVeritabaniDto>(entity);
    }

    [LogAspect]
    public async Task<CreateVeritabaniDto> AddAsync(CreateVeritabaniDto dto)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var entity = mapper.Map<Veritabani>(dto);
            var repository = unitOfWork.GetRepository<Veritabani, int>();

            await repository.AddAsync(entity);
            return mapper.Map<CreateVeritabaniDto>(entity);
        });
    }

    [LogAspect]
    public async Task<UpdateVeritabaniDto> UpdateAsync(UpdateVeritabaniDto dto)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<Veritabani, int>();
            var entity = await repository.GetAsync(x => x.Id == dto.Id);

            if (entity == null)
                throw new NotFoundException($"Veritabani with id {dto.Id} not found");

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
            return mapper.Map<UpdateVeritabaniDto>(entity);
        });
    }

    [LogAspect]
    public async Task DeleteAsync(int id)
    {
        await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<Veritabani, int>();
            var entity = await repository.GetAsync(x => x.Id == id);

            if (entity == null)
                throw new NotFoundException($"Veritabani with id {id} not found");

            entity.EnvanterdenCikisTarihi = DateTime.Now;
            entity.DurumId = (int)DurumEnum.PasifVarlik;

            await repository.UpdateAsync(entity);
        });
    }

    [LogAspect]
    public async Task DeleteDatabaseAsync(int id)
    {
        await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var repository = unitOfWork.GetRepository<Veritabani, int>();
            var entity = await repository.GetAsync(x => x.Id == id);

            if (entity == null)
                throw new NotFoundException($"Veritabani with id {id} not found");

            entity.SilinsinMi = true;
            entity.EnvanterdenCikisTarihi = DateTime.Now;

            await repository.UpdateAsync(entity);
        });
    }

    public async Task<bool> AnyAsync(int id)
    {
        var repository = unitOfWork.GetRepository<Veritabani, int>();
        return await repository.AnyAsync(x => x.Id == id);
    }
}