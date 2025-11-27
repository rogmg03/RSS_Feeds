using RSS_Feeds.Models.DTOs;
using RSS_Feeds.ServiceLocator.Services;
using RSS_Feeds.ServiceLocator.Services.Contracts;

namespace RSS_Feeds.ServiceLocator.Helper
{
    public interface IServiceMapper
    {
        IService<T> GetService<T>(string name);
    }

    public class ServiceMapper : IServiceMapper
    {
        private readonly IServiceProvider serviceProvider;

        public ServiceMapper(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IService<T> GetService<T>(string name)
        {
            return name.ToLower() switch
            {
                "feed" => (IService<T>)serviceProvider.GetRequiredService<IFeedService>(),
                "usuario" => (IService<T>)serviceProvider.GetRequiredService<IUsuarioService>(),
                "articulo" => (IService<T>)serviceProvider.GetRequiredService<IArticuloService>(),
                _ => throw new KeyNotFoundException($"Service not found for '{name}'.")
            };
        }
    }
}
