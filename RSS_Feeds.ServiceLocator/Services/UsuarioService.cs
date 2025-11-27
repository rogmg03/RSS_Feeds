using RSS_Feeds.Architecture;
using RSS_Feeds.Architecture.Providers;
using RSS_Feeds.Models.DTOs;
using RSS_Feeds.ServiceLocator.Extensions;
using RSS_Feeds.ServiceLocator.Services.Contracts;

namespace RSS_Feeds.ServiceLocator.Services
{
    public interface IUsuarioService : IService<UsuarioDTO>
    {
        Task<IEnumerable<UsuarioDTO>> GetDataByIdAsync(int id);
        Task<bool> CreateAsync(UsuarioDTO dto);
        Task<bool> UpdateAsync(int id, UsuarioDTO dto);
        Task<bool> DeleteAsync(int id);
    }

    public class UsuarioService(IRestProvider restProvider, IConfiguration configuration)
        : IService<UsuarioDTO>, IUsuarioService
    {
        public async Task<IEnumerable<UsuarioDTO>> GetDataAsync()
        {
            var url = configuration.GetStringFromAppSettings("APIS", "Usuario");
            var response = await restProvider.GetAsync(url, null);
            return await JsonProvider.DeserializeAsync<IEnumerable<UsuarioDTO>>(response);
        }

        public async Task<IEnumerable<UsuarioDTO>> GetDataByIdAsync(int id)
        {
            var url = configuration.GetStringFromAppSettings("APIS", "Usuario");
            var full = $"{url.TrimEnd('/')}/{id}";
            var response = await restProvider.GetAsync(full, null);
            return await JsonProvider.DeserializeAsync<IEnumerable<UsuarioDTO>>(response);
        }

        public async Task<bool> CreateAsync(UsuarioDTO dto)
        {
            var url = configuration.GetStringFromAppSettings("APIS", "Usuario");
            var body = JsonProvider.Serialize(dto);
            var response = await restProvider.PostAsync(url, body);

            var s = response?.Trim().Trim('"');
            return bool.TryParse(s, out var ok) && ok;
        }

        public async Task<bool> UpdateAsync(int id, UsuarioDTO dto)
        {
            var url = configuration.GetStringFromAppSettings("APIS", "Usuario");
            var body = JsonProvider.Serialize(dto);
            var full = $"{url.TrimEnd('/')}/{id}";
            var response = await restProvider.PutAsync(full, null, body);

            var s = response?.Trim().Trim('"');
            return bool.TryParse(s, out var ok) && ok;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var url = configuration.GetStringFromAppSettings("APIS", "Usuario");
            var full = $"{url.TrimEnd('/')}/{id}";
            var response = await restProvider.DeleteAsync(full, null);

            var s = response?.Trim().Trim('"');
            return bool.TryParse(s, out var ok) && ok;
        }
    }
}
