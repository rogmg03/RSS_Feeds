using RSS_Feeds.Data.Models;
using RSS_Feeds.Data.Repositories;

namespace RSS_Feeds.Core.BusinessLogic;

public interface IUsuarioFeedBusiness
{
    Task<IEnumerable<UsuarioFeed>> GetAsync();
    Task<IEnumerable<UsuarioFeed>> GetByIdAsync(int id);
    Task<IEnumerable<UsuarioFeed>> GetByUsuarioAsync(int usuarioId);
    Task<bool> SaveAsync(UsuarioFeed entity);
    Task<bool> DeleteAsync(int id);
}

public class UsuarioFeedBusiness(IRepositoryUsuarioFeed repo) : IUsuarioFeedBusiness
{
    private readonly IRepositoryUsuarioFeed _repo = repo;

    public Task<IEnumerable<UsuarioFeed>> GetAsync()
        => _repo.ReadAsync();

    public async Task<IEnumerable<UsuarioFeed>> GetByIdAsync(int id)
    {
        var item = await _repo.FindAsync(id);
        return item is null ? Enumerable.Empty<UsuarioFeed>() : new[] { item };
    }

    public async Task<IEnumerable<UsuarioFeed>> GetByUsuarioAsync(int usuarioId)
    {
        var all = await _repo.ReadAsync();
        return all.Where(x => x.UsuarioId == usuarioId);
    }

    public async Task<bool> SaveAsync(UsuarioFeed entity)
    {
        if (entity == null)
            return false;

        // Dominio: valores automáticos
        if (entity.Id == 0)
        {
            entity.CreadoEn = DateTime.UtcNow;
        }

        // (opcional pero recomendado)
        if (entity.UsuarioId <= 0 || entity.FeedId <= 0)
            return false;

        return await _repo.CheckBeforeSavingAsync(entity);
    }

    public Task<bool> DeleteAsync(int id)
        => _repo.DeleteAsync(id);
}
