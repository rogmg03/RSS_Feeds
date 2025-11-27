using Microsoft.EntityFrameworkCore;
using RSS_Feeds.Data.Models;

namespace RSS_Feeds.Data.Repositories
{
    public interface IRepositoryFeed
    {
        Task<bool> UpsertAsync(Feed entity, bool isUpdating);
        Task<bool> CreateAsync(Feed entity);
        Task<bool> DeleteAsync(Feed entity);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Feed>> ReadAsync();
        Task<Feed?> FindAsync(int id);
        Task<bool> UpdateAsync(Feed entity);
        Task<bool> UpdateManyAsync(IEnumerable<Feed> entities);
        Task<bool> ExistsAsync(Feed entity);
        Task<bool> CheckBeforeSavingAsync(Feed entity);
    }

    public class RepositoryFeed : RepositoryBase<Feed>, IRepositoryFeed
    {
        public RepositoryFeed(RssDbContext ctx) : base(ctx) { }

        public async Task<bool> CheckBeforeSavingAsync(Feed entity)
        {
            // Validaciones sencillas de dominio
            if (string.IsNullOrWhiteSpace(entity.Url))
                return false;

            var exists = await ExistsAsync(entity);
            return await UpsertAsync(entity, exists);
        }

        // === Existencia por PK ===
        public new async Task<bool> ExistsAsync(Feed entity)
            => await DbContext.Feeds.AnyAsync(x => x.Id == entity.Id);

        // CRUD
        public Task<bool> UpsertAsync(Feed entity, bool isUpdating) => base.UpsertAsync(entity, isUpdating);
        public Task<bool> CreateAsync(Feed entity) => base.CreateAsync(entity);
        public Task<bool> UpdateAsync(Feed entity) => base.UpdateAsync(entity);
        public Task<bool> UpdateManyAsync(IEnumerable<Feed> entities) => base.UpdateManyAsync(entities);
        public Task<IEnumerable<Feed>> ReadAsync() => base.ReadAsync();
        public new Task<Feed?> FindAsync(int id) => base.FindAsync(id);
        public Task<bool> DeleteAsync(Feed entity) => base.DeleteAsync(entity);

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await FindAsync(id);
            return entity is not null && await DeleteAsync(entity);
        }
    }
}
