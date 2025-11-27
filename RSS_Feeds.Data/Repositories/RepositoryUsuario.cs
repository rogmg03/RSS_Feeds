using Microsoft.EntityFrameworkCore;
using RSS_Feeds.Data.Models;

namespace RSS_Feeds.Data.Repositories
{
    public interface IRepositoryUsuario
    {
        Task<bool> UpsertAsync(Usuario entity, bool isUpdating);
        Task<bool> CreateAsync(Usuario entity);
        Task<bool> DeleteAsync(Usuario entity);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Usuario>> ReadAsync();
        Task<Usuario?> FindAsync(int id);
        Task<bool> UpdateAsync(Usuario entity);
        Task<bool> UpdateManyAsync(IEnumerable<Usuario> entities);
        Task<bool> ExistsAsync(Usuario entity);
        Task<bool> CheckBeforeSavingAsync(Usuario entity);
    }

    public class RepositoryUsuario : RepositoryBase<Usuario>, IRepositoryUsuario
    {
        public RepositoryUsuario(RssDbContext ctx) : base(ctx) { }

        public async Task<bool> CheckBeforeSavingAsync(Usuario entity)
        {
            // Validaciones sencillas de dominio
            if (string.IsNullOrWhiteSpace(entity.Email))
                return false;

            // Aquí podrías setear valores por defecto
            // entity.Nombre ??= "Sin nombre";

            var exists = await ExistsAsync(entity);
            return await UpsertAsync(entity, exists);
        }

        // === Existencia por PK ===
        public new async Task<bool> ExistsAsync(Usuario entity)
            => await DbContext.Usuarios.AnyAsync(x => x.Id == entity.Id);

        // === CRUD ===
        public Task<bool> UpsertAsync(Usuario entity, bool isUpdating) => base.UpsertAsync(entity, isUpdating);
        public Task<bool> CreateAsync(Usuario entity) => base.CreateAsync(entity);
        public Task<bool> UpdateAsync(Usuario entity) => base.UpdateAsync(entity);
        public Task<bool> UpdateManyAsync(IEnumerable<Usuario> entities) => base.UpdateManyAsync(entities);
        public Task<IEnumerable<Usuario>> ReadAsync() => base.ReadAsync();
        public new Task<Usuario?> FindAsync(int id) => base.FindAsync(id);
        public Task<bool> DeleteAsync(Usuario entity) => base.DeleteAsync(entity);

        // Helper por id (comodidad para el Business)
        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await FindAsync(id);
            return entity is not null && await DeleteAsync(entity);
        }
    }
}
