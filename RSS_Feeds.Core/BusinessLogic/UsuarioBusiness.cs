using RSS_Feeds.Data.Models;
using RSS_Feeds.Data.Repositories;


namespace RSS_Feeds.Core.BusinessLogic;

public interface IUsuarioBusiness
{
    Task<IEnumerable<Usuario>> GetAsync();           // GET all
    Task<IEnumerable<Usuario>> GetByIdAsync(int id); // GET {id}
    Task<bool> SaveAsync(Usuario entity);            // POST/PUT
    Task<bool> DeleteAsync(int id);                  // DELETE {id}
}

public class UsuarioBusiness(IRepositoryUsuario repo) : IUsuarioBusiness
{
    private readonly IRepositoryUsuario _repo = repo;

    public Task<IEnumerable<Usuario>> GetAsync()
        => _repo.ReadAsync();

    public async Task<IEnumerable<Usuario>> GetByIdAsync(int id)
    {
        var item = await _repo.FindAsync(id);
        return item is null ? Enumerable.Empty<Usuario>() : new[] { item };
    }

    public Task<bool> SaveAsync(Usuario entity)
        => _repo.CheckBeforeSavingAsync(entity);

    public Task<bool> DeleteAsync(int id)
        => _repo.DeleteAsync(id);
}
