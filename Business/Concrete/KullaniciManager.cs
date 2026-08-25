using AutoMapper;
using Business.Abstract;
using Core.Exceptions;
using Dto.DTOs;
using Dto.Kullanici;
using Dto.KullaniciBirim;
using Entity.Concrete;
using Repository.Abstract;
using Repository.UnitOfWork;

namespace Business.Concrete
{
    public class KullaniciManager(
        IVarlikEnvanteriUnitOfWork unitOfWork,
        IMapper mapper,
        IKullaniciRepository kullaniciRepository,
        IKullaniciBirimService kullaniciBirimService) : IKullaniciService
    {
        public async Task<DataTablesResponse<ListKullaniciDto>> GetAllAsync(DataTablesRequest request)
        {
            return await kullaniciRepository.ProcessTableRequestAsync(request);
        }

        public async Task<List<ListKullaniciDto>> GetAllAsync()
        {
            var repository = unitOfWork.GetRepository<Kullanici, int>();
            var entities = await repository.GetAllAsync();

            return mapper.Map<List<ListKullaniciDto>>(entities);
        }

        public async Task<UpdateKullaniciDto> GetByIdAsync(int id)
        {
            var entity = await unitOfWork.GetRepository<Kullanici, int>().GetAsync(x => x.Id == id);
            return mapper.Map<UpdateKullaniciDto>(entity);
        }

        public async Task<ListKullaniciDto> GetByUsernameAsync(string username)
        {
            var entity = await unitOfWork.GetRepository<Kullanici, int>().GetAsync(x => x.Username == username);
            return mapper.Map<ListKullaniciDto>(entity);
        }

        public async Task<UpdateKullaniciDto> AddAsync(CreateKullaniciDto dto)
        {
            return await unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                var repository = unitOfWork.GetRepository<Kullanici, int>();

                var entity = await repository.GetAsync(x => x.Username == dto.Username);
                if (entity == null)
                {
                    entity = mapper.Map<Kullanici>(dto);
                    entity = await repository.AddAsync(entity);
                }

                var kullaniciBiriDto = new CreateKullaniciBirimDto()
                {
                    BirimAd = dto.BirimAd,
                    KullaniciId = entity.Id,
                    BirimId = dto.BirimId
                };

                await kullaniciBirimService.AddAsync(kullaniciBiriDto);

                return mapper.Map<UpdateKullaniciDto>(entity);
            });
        }

        public async Task<UpdateKullaniciDto> UpdateAsync(UpdateKullaniciDto dto)
        {
            return await unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                var repository = unitOfWork.GetRepository<Kullanici, int>();
                var entity = await repository.GetAsync(x => x.Id == dto.Id);

                if (entity == null)
                    throw new NotFoundException($"Kullanıcı {dto.Id} bulunamadı.");

                var kullaniciBirimler = await kullaniciBirimService.GetByKullaniciIdAsync(entity.Id);
                foreach (var birim in kullaniciBirimler)
                {
                    if (birim.BirimId == dto.BirimId)
                    {
                        entity.BirimId = dto.BirimId;
                        entity.BirimAd = dto.BirimAd;
                        break;
                    }

                    entity.BirimId = 0;
                    entity.BirimAd = string.Empty;
                }

                await repository.UpdateAsync(entity);
                return mapper.Map<UpdateKullaniciDto>(entity);
            });
        }

        public async Task DeleteAsync(int id)
        {
            var kullaniciBirim = await kullaniciBirimService.GetByIdAsync(id);
            if (kullaniciBirim == null)
                throw new NotFoundException($"Kullanıcı birim kaydı {id} bulunamadı.");

            await unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                var repository = unitOfWork.GetRepository<Kullanici, int>();
                var entity = await repository.GetAsync(x => x.Id == kullaniciBirim.KullaniciId);

                if (entity == null)
                    throw new NotFoundException($"Kullanıcı {kullaniciBirim.KullaniciId} bulunamadı.");

                entity.BirimAd = string.Empty;
                entity.BirimId = 0;

                await repository.UpdateAsync(entity);
            });

            await kullaniciBirimService.DeleteAsync(id);
        }

        public async Task<bool> AnyAsync(int id)
        {
            var repository = unitOfWork.GetRepository<Kullanici, int>();
            return await repository.AnyAsync(x => x.Id == id);
        }

        public async Task<List<KullaniciListeDto>> KullanicilariGetirAsync()
        {
            return await kullaniciRepository.KullanicilariGetirAsync();
        }
    }
}