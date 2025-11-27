using RSS_Feeds.Architecture;
using RSS_Feeds.Architecture.Providers;
using RSS_Feeds.Models.DTOs;
using RSS_Feeds.ServiceLocator.Extensions;
using RSS_Feeds.ServiceLocator.Services.Contracts;

namespace RSS_Feeds.ServiceLocator.Services
{
    public interface IUsuarioArticulosGuardadoService : IService<UsuarioArticulosGuardadoDTO>
    {
        Task<IEnumerable<UsuarioArticulosGuardadoDTO>> GetDataByIdAsync(int id);
        Task<bool> CreateAsync(UsuarioArticulosGuardadoDTO dto);
        Task<bool> UpdateAsync(int id, UsuarioArticulosGuardadoDTO dto);
        Task<bool> DeleteAsync(int id);
    }

    public class UsuarioArticulosGuardadoService(IRestProvider restProvider, IConfiguration configuration)
        : IService<UsuarioArticulosGuardadoDTO>, IUsuarioArticulosGuardadoService
    {
        public async Task<IEnumerable<UsuarioArticulosGuardadoDTO>> GetDataAsync()
        {
            var url = configuration.GetStringFromAppSettings("APIS", "UsuarioArticulosGuardado");
            var response = await restProvider.GetAsync(url, null);
            return await JsonProvider.DeserializeAsync<IEnumerable<UsuarioArticulosGuardadoDTO>>(response);
        }

        public async Task<IEnumerable<UsuarioArticulosGuardadoDTO>> GetDataByIdAsync(int id)
        {
            var url = configuration.GetStringFromAppSettings("APIS", "UsuarioArticulosGuardado");
            var full = $"{url.TrimEnd('/')}/{id}";
            var response = await restProvider.GetAsync(full, null);
            return await JsonProvider.DeserializeAsync<IEnumerable<UsuarioArticulosGuardadoDTO>>(response);
        }

        public async Task<bool> CreateAsync(UsuarioArticulosGuardadoDTO dto)
        {
            var url = configuration.GetStringFromAppSettings("APIS", "UsuarioArticulosGuardado");
            var body = JsonProvider.Serialize(dto);
            var response = await restProvider.PostAsync(url, body);

            var s = response?.Trim().Trim('"');
            return bool.TryParse(s, out var ok) && ok;
        }

        public async Task<bool> UpdateAsync(int id, UsuarioArticulosGuardadoDTO dto)
        {
            var url = configuration.GetStringFromAppSettings("APIS", "UsuarioArticulosGuardado");
            var body = JsonProvider.Serialize(dto);
            var full = $"{url.TrimEnd('/')}/{id}";
            var response = await restProvider.PutAsync(full, null, body);

            var s = response?.Trim().Trim('"');
            return bool.TryParse(s, out var ok) && ok;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var url = configuration.GetStringFromAppSettings("APIS", "UsuarioArticulosGuardado");
            var full = $"{url.TrimEnd('/')}/{id}";
            var response = await restProvider.DeleteAsync(full, null);

            var s = response?.Trim().Trim('"');
            return bool.TryParse(s, out var ok) && ok;
        }
    }
}
