using AutoMapper;
using Business.Abstract;
using Core.Exceptions;
using Dto.AgveSistem;
using Dto.Kullanici;
using Dto.KullaniciRol;
using Entity.Concrete;
using Repository.Abstract;
using Repository.UnitOfWork;

namespace Business.Concrete
{
    public class KullaniciRolManager(
        IVarlikEnvanteriUnitOfWork unitOfWork,
        IMapper mapper,
        IKullaniciRolRepository kullaniciRolRepository) : IKullaniciRolService
    {
        public async Task<List<ListKullaniciRolDto>> GetAllAsync()
        {
            var repository = unitOfWork.GetRepository<KullaniciRol, int>();
            var entities = await repository.GetAllWithIncludeAsync(x => x.Kullanici, x => x.Rol);

            return mapper.Map<List<ListKullaniciRolDto>>(entities);
        }

        public async Task<UpdateKullaniciRolDto> GetByIdAsync(int id)
        {
            var entity = await unitOfWork.GetRepository<KullaniciRol, int>().GetAsync(x => x.Id == id);
            return mapper.Map<UpdateKullaniciRolDto>(entity);
        }

        public async Task<List<ListKullaniciRolDto>> GetByUsernameAsync(string username)
        {
            var repository = unitOfWork.GetRepository<KullaniciRol, int>();
            var entities = await repository.GetAllWithOptionsAsync(x => x.Kullanici.Username == username, null,
                x => x.Kullanici!,
                x => x.Rol!
            );

            return mapper.Map<List<ListKullaniciRolDto>>(entities);
        }

        public async Task<CreateKullaniciRolDto> AddAsync(CreateKullaniciRolDto dto)
        {
            return await unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                var entity = mapper.Map<KullaniciRol>(dto);
                var repository = unitOfWork.GetRepository<KullaniciRol, int>();

                await repository.AddAsync(entity);
                return mapper.Map<CreateKullaniciRolDto>(entity);
            });
        }

        public async Task<UpdateKullaniciRolDto> UpdateAsync(UpdateKullaniciRolDto dto)
        {
            return await unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                var repository = unitOfWork.GetRepository<KullaniciRol, int>();
                var entity = await repository.GetAsync(x => x.Id == dto.Id);

                if (entity == null)
                    throw new NotFoundException($"KullaniciRol with id {dto.Id} not found");

                mapper.Map(dto, entity);
                await repository.UpdateAsync(entity);
                return mapper.Map<UpdateKullaniciRolDto>(entity);
            });
        }

        public async Task DeleteAsync(int id)
        {
            await unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                var repository = unitOfWork.GetRepository<KullaniciRol, int>();
                var entity = await repository.GetAsync(x => x.Id == id);

                if (entity == null)
                    throw new NotFoundException($"KullaniciRol with id {id} not found");

                await repository.DeleteAsync(entity);
            });
        }

        public async Task<bool> AnyAsync(int id)
        {
            var repository = unitOfWork.GetRepository<KullaniciRol, int>();
            return await repository.AnyAsync(x => x.Id == id);
        }

        public async Task<KullaniciRolAtamaDto> KullaniciRolleriniGetirAsync(int kullaniciId)
        {
            return await kullaniciRolRepository.KullaniciRolleriniGetirAsync(kullaniciId) ?? new KullaniciRolAtamaDto();
        }
    }
}