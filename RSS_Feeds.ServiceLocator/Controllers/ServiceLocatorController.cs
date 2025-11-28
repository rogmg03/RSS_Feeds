using Microsoft.AspNetCore.Mvc;
using RSS_Feeds.Models.DTOs;
using RSS_Feeds.ServiceLocator.Services;

namespace RSS_Feeds.ServiceLocator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [IgnoreAntiforgeryToken]
    public class ServiceLocatorController : ControllerBase
    {
        private readonly IFeedService _feedService;
        private readonly IUsuarioService _usuarioService;
        private readonly IArticuloService _articuloService;
        private readonly IUsuarioFeedService _usuarioFeedService;
        private readonly IUsuarioArticulosGuardadoService _usuarioArticulosGuardadoService;

        public ServiceLocatorController(
            IFeedService feedService,
            IUsuarioService usuarioService,
            IArticuloService articuloService,
            IUsuarioFeedService usuarioFeedService,
            IUsuarioArticulosGuardadoService usuarioArticulosGuardadoService)
        {
            _feedService = feedService;
            _usuarioService = usuarioService;
            _articuloService = articuloService;
            _usuarioFeedService = usuarioFeedService;
            _usuarioArticulosGuardadoService = usuarioArticulosGuardadoService;
        }

        // ---------- FEEDS ----------
        [HttpGet("feeds")]
        public Task<IEnumerable<FeedDTO>> GetFeeds()
            => _feedService.GetDataAsync();

        [HttpGet("feeds/{id:int}")]
        public Task<IEnumerable<FeedDTO>> GetFeedById(int id)
            => _feedService.GetDataByIdAsync(id);

        [HttpPost("feeds")]
        public Task<bool> CreateFeed([FromBody] FeedDTO dto)
            => _feedService.CreateAsync(dto);

        [HttpPut("feeds/{id:int}")]
        public Task<bool> UpdateFeed(int id, [FromBody] FeedDTO dto)
            => _feedService.UpdateAsync(id, dto);

        [HttpDelete("feeds/{id:int}")]
        public Task<bool> DeleteFeed(int id)
            => _feedService.DeleteAsync(id);

        // ---------- USUARIOS ----------
        [HttpGet("usuarios")]
        public Task<IEnumerable<UsuarioDTO>> GetUsuarios()
            => _usuarioService.GetDataAsync();

        [HttpGet("usuarios/{id:int}")]
        public Task<IEnumerable<UsuarioDTO>> GetUsuarioById(int id)
            => _usuarioService.GetDataByIdAsync(id);

        [HttpPost("usuarios")]
        public Task<bool> CreateUsuario([FromBody] UsuarioDTO dto)
            => _usuarioService.CreateAsync(dto);

        [HttpPut("usuarios/{id:int}")]
        public Task<bool> UpdateUsuario(int id, [FromBody] UsuarioDTO dto)
            => _usuarioService.UpdateAsync(id, dto);

        [HttpDelete("usuarios/{id:int}")]
        public Task<bool> DeleteUsuario(int id)
            => _usuarioService.DeleteAsync(id);

        // ---------- ARTICULOS ----------
        [HttpGet("articulos")]
        public Task<IEnumerable<ArticuloDTO>> GetArticulos()
            => _articuloService.GetDataAsync();

        [HttpGet("articulos/{id:int}")]
        public Task<IEnumerable<ArticuloDTO>> GetArticuloById(int id)
            => _articuloService.GetDataByIdAsync(id);

        [HttpPost("articulos")]
        public Task<bool> CreateArticulo([FromBody] ArticuloDTO dto)
            => _articuloService.CreateAsync(dto);

        [HttpPut("articulos/{id:int}")]
        public Task<bool> UpdateArticulo(int id, [FromBody] ArticuloDTO dto)
            => _articuloService.UpdateAsync(id, dto);

        [HttpDelete("articulos/{id:int}")]
        public Task<bool> DeleteArticulo(int id)
            => _articuloService.DeleteAsync(id);

        // ---------- USUARIO FEED ----------
        [HttpGet("usuario-feed")]
        public Task<IEnumerable<UsuarioFeedDTO>> GetUsuarioFeeds()
            => _usuarioFeedService.GetDataAsync();

        [HttpGet("usuario-feed/{id:int}")]
        public Task<IEnumerable<UsuarioFeedDTO>> GetUsuarioFeedById(int id)
            => _usuarioFeedService.GetDataByIdAsync(id);

        [HttpPost("usuario-feed")]
        public Task<bool> CreateUsuarioFeed([FromBody] UsuarioFeedDTO dto)
            => _usuarioFeedService.CreateAsync(dto);

        [HttpPut("usuario-feed/{id:int}")]
        public Task<bool> UpdateUsuarioFeed(int id, [FromBody] UsuarioFeedDTO dto)
            => _usuarioFeedService.UpdateAsync(id, dto);

        [HttpDelete("usuario-feed/{id:int}")]
        public Task<bool> DeleteUsuarioFeed(int id)
            => _usuarioFeedService.DeleteAsync(id);

        // ---------- USUARIO ARTÍCULOS GUARDADOS ----------
        [HttpGet("usuario-articulos-guardados")]
        public Task<IEnumerable<UsuarioArticulosGuardadoDTO>> GetUsuarioArticulosGuardados()
            => _usuarioArticulosGuardadoService.GetDataAsync();

        [HttpGet("usuario-articulos-guardados/{id:int}")]
        public Task<IEnumerable<UsuarioArticulosGuardadoDTO>> GetUsuarioArticuloGuardadoById(int id)
            => _usuarioArticulosGuardadoService.GetDataByIdAsync(id);

        [HttpPost("usuario-articulos-guardados")]
        public Task<bool> CreateUsuarioArticuloGuardado([FromBody] UsuarioArticulosGuardadoDTO dto)
            => _usuarioArticulosGuardadoService.CreateAsync(dto);

        [HttpPut("usuario-articulos-guardados/{id:int}")]
        public Task<bool> UpdateUsuarioArticuloGuardado(int id, [FromBody] UsuarioArticulosGuardadoDTO dto)
            => _usuarioArticulosGuardadoService.UpdateAsync(id, dto);

        [HttpDelete("usuario-articulos-guardados/{id:int}")]
        public Task<bool> DeleteUsuarioArticuloGuardado(int id)
            => _usuarioArticulosGuardadoService.DeleteAsync(id);

    }
}
