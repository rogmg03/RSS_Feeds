using Microsoft.AspNetCore.Mvc;
using RSS_Feeds.Core.BusinessLogic;
using RSS_Feeds.Data.Models;

namespace RSS_Feeds.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioFeedApiController(IUsuarioFeedBusiness usuarioFeedBusiness) : ControllerBase
    {
        // GET: api/UsuarioFeedApi
        [HttpGet]
        public async Task<IEnumerable<UsuarioFeed>> Get()
        {
            return await usuarioFeedBusiness.GetAsync();
        }

        // GET: api/UsuarioFeedApi/5
        [HttpGet("{id}")]
        public async Task<IEnumerable<UsuarioFeed>> Get(int id)
        {
            return await usuarioFeedBusiness.GetByIdAsync(id);
        }

        // GET: api/UsuarioFeedApi/usuario/1  (todas las suscripciones de un usuario)
        [HttpGet("usuario/{usuarioId}")]
        public async Task<IEnumerable<UsuarioFeed>> GetByUsuario(int usuarioId)
        {
            return await usuarioFeedBusiness.GetByUsuarioAsync(usuarioId);
        }

        // POST: api/UsuarioFeedApi   (suscribir usuario a feed)
        [HttpPost]
        public Task<bool> Post([FromBody] UsuarioFeed entity)
        {
            return usuarioFeedBusiness.SaveAsync(entity);
        }

        // PUT: api/UsuarioFeedApi/5
        [HttpPut("{id}")]
        public async Task<bool> Put(int id, [FromBody] UsuarioFeed entity)
        {
            if (entity is null) return false;

            if (entity.Id == 0)
                entity.Id = id;
            else if (entity.Id != id)
                return false;

            return await usuarioFeedBusiness.SaveAsync(entity);
        }

        // DELETE: api/UsuarioFeedApi/5
        [HttpDelete("{id}")]
        public Task<bool> Delete(int id)
        {
            return usuarioFeedBusiness.DeleteAsync(id);
        }
    }
}
