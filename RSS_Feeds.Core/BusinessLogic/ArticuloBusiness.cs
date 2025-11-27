using RSS_Feeds.Data.Models;
using RSS_Feeds.Data.Repositories;

namespace RSS_Feeds.Core.BusinessLogic;

public interface IArticuloBusiness
{
    Task<IEnumerable<Articulo>> GetAsync();
    Task<IEnumerable<Articulo>> GetByIdAsync(int id);
    Task<IEnumerable<Articulo>> GetByFeedAsync(int feedId);
    Task<bool> SaveAsync(Articulo entity);
    Task<bool> DeleteAsync(int id);
}

public class ArticuloBusiness(IRepositoryArticulo repo) : IArticuloBusiness
{
    private readonly IRepositoryArticulo _repo = repo;

    public Task<IEnumerable<Articulo>> GetAsync()
        => _repo.ReadAsync();

    public async Task<IEnumerable<Articulo>> GetByIdAsync(int id)
    {
        var item = await _repo.FindAsync(id);
        return item is null ? Enumerable.Empty<Articulo>() : new[] { item };
    }

    public async Task<IEnumerable<Articulo>> GetByFeedAsync(int feedId)
    {
        var all = await _repo.ReadAsync();
        return all.Where(a => a.FeedId == feedId);
    }

    public Task<bool> SaveAsync(Articulo entity)
        => _repo.CheckBeforeSavingAsync(entity);

    public Task<bool> DeleteAsync(int id)
        => _repo.DeleteAsync(id);
}
