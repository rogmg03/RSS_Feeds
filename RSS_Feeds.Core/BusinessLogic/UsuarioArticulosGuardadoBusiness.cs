using RSS_Feeds.Data.Models;
using RSS_Feeds.Data.Repositories;

namespace RSS_Feeds.Core.BusinessLogic;

public interface IUsuarioArticulosGuardadoBusiness
{
    Task<IEnumerable<UsuarioArticulosGuardado>> GetAsync();
    Task<IEnumerable<UsuarioArticulosGuardado>> GetByIdAsync(int id);
    Task<IEnumerable<UsuarioArticulosGuardado>> GetByUsuarioAsync(int usuarioId);
    Task<bool> SaveAsync(UsuarioArticulosGuardado entity);
    Task<bool> DeleteAsync(int id);
}

public class UsuarioArticulosGuardadoBusiness(
    IRepositoryUsuarioArticulosGuardado repo
) : IUsuarioArticulosGuardadoBusiness
{
    private readonly IRepositoryUsuarioArticulosGuardado _repo = repo;

    public Task<IEnumerable<UsuarioArticulosGuardado>> GetAsync()
        => _repo.ReadAsync();

    public async Task<IEnumerable<UsuarioArticulosGuardado>> GetByIdAsync(int id)
    {
        var item = await _repo.FindAsync(id);
        return item is null ? Enumerable.Empty<UsuarioArticulosGuardado>() : new[] { item };
    }

    public async Task<IEnumerable<UsuarioArticulosGuardado>> GetByUsuarioAsync(int usuarioId)
    {
        var all = await _repo.ReadAsync();
        return all.Where(x => x.UsuarioId == usuarioId);
    }

    public Task<bool> SaveAsync(UsuarioArticulosGuardado entity)
        => _repo.CheckBeforeSavingAsync(entity);

    public Task<bool> DeleteAsync(int id)
        => _repo.DeleteAsync(id);
}
