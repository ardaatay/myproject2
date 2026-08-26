using AutoMapper;
using Business.Abstract;
using Core.Aspects;
using Core.Exceptions;
using Dto.DTOs;
using Dto.Kullanici;
using Dto.Kullanici.Enum;
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

        public async Task<KullaniciDuzenleDto?> DuzenlemeIcinGetirAsync(int kullaniciBirimId)
        {
            var kullaniciBirim = await kullaniciBirimService.GetByIdAsync(kullaniciBirimId);
            if (kullaniciBirim == null)
                return null;

            var entity = await unitOfWork.GetRepository<Kullanici, int>()
                .GetAsync(x => x.Id == kullaniciBirim.KullaniciId);

            if (entity == null)
                return null;

            return new KullaniciDuzenleDto
            {
                KullaniciBirimId = kullaniciBirim.Id,
                KullaniciId = entity.Id,
                Username = entity.Username,
                GirisYontemi = entity.GirisYontemi,
                MevcutGirisYontemi = entity.GirisYontemi,
                ActiveDirectoryKullaniciAdi = entity.ActiveDirectoryKullaniciAdi,
                AdSoyad = entity.AdSoyad,
                Eposta = entity.Eposta,
                BirimId = kullaniciBirim.BirimId,
                BirimAd = kullaniciBirim.BirimAd ?? string.Empty,
                Durum = entity.Durum
            };
        }


        [LogAspect]
        public async Task DuzenleAsync(KullaniciDuzenleDto dto)
        {
            await unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                var repository = unitOfWork.GetRepository<Kullanici, int>();
                var entity = await repository.GetAsync(x => x.Id == dto.KullaniciId);

                if (entity == null)
                    throw new NotFoundException($"Kullanıcı {dto.KullaniciId} bulunamadı.");

                entity.Username = dto.Username.Trim().ToLower();
                entity.AdSoyad = Kirp(dto.AdSoyad);
                entity.Eposta = Kirp(dto.Eposta);
                entity.Durum = dto.Durum;

                if (dto.GirisYontemi != entity.GirisYontemi)
                    GirisYonteminiDegistir(entity, dto.GirisYontemi);

                entity.ActiveDirectoryKullaniciAdi = dto.GirisYontemi == GirisYontemi.ActiveDirectory
                    ? Kirp(dto.ActiveDirectoryKullaniciAdi)
                    : null;

                entity.BirimId = dto.BirimId;
                entity.BirimAd = dto.BirimAd;

                await repository.UpdateAsync(entity);

                await kullaniciBirimService.UpdateAsync(new UpdateKullaniciBirimDto
                {
                    Id = dto.KullaniciBirimId,
                    BirimId = dto.BirimId,
                    BirimAd = dto.BirimAd
                });
            });
        }

        /// <summary>
        /// Giriş yöntemi değişince yerel şifre her iki yönde de geçersizleşir:
        /// dizine geçen hesabın uygulamada şifresi kalmamalı, yerele dönen hesap
        /// ise yöneticinin sıfırlayacağı yeni bir şifreyi beklemelidir. Damga
        /// yenilendiği için kullanıcının açık oturumları da düşer.
        /// </summary>
        private static void GirisYonteminiDegistir(Kullanici entity, GirisYontemi yeniYontem)
        {
            entity.GirisYontemi = yeniYontem;
            entity.PasswordHash = null;
            entity.SifreDegistirmeliMi = yeniYontem == GirisYontemi.Yerel;
            entity.SecurityStamp = Guid.NewGuid().ToString("N");
            entity.BasarisizGirisSayisi = 0;
            entity.KilitBitisTarihi = null;
        }

        private static string? Kirp(string? deger) =>
            string.IsNullOrWhiteSpace(deger) ? null : deger.Trim();

        public async Task<ListKullaniciDto> GetByUsernameAsync(string username)
        {
            var entity = await unitOfWork.GetRepository<Kullanici, int>().GetAsync(x => x.Username == username);
            return mapper.Map<ListKullaniciDto>(entity);
        }


        [LogAspect]
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


        [LogAspect]
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


        [LogAspect]
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