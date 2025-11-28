using Microsoft.AspNetCore.Mvc;
using RSS_Feeds.Core.BusinessLogic;
using RSS_Feeds.Data.Models;

namespace RSS_Feeds.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticuloApiController(IArticuloBusiness articuloBusiness) : ControllerBase
    {
        // GET: api/ArticuloApi
        [HttpGet]
        public async Task<IEnumerable<Articulo>> Get()
        {
            return await articuloBusiness.GetAsync();
        }

        // GET: api/ArticuloApi/5
        [HttpGet("{id}")]
        public async Task<IEnumerable<Articulo>> Get(int id)
        {
            return await articuloBusiness.GetByIdAsync(id);
        }

        // GET: api/ArticuloApi/feed/3  -> artículos de un feed
        [HttpGet("feed/{feedId}")]
        public async Task<IEnumerable<Articulo>> GetByFeed(int feedId)
        {
            return await articuloBusiness.GetByFeedAsync(feedId);
        }

        // POST: api/ArticuloApi
        [HttpPost]
        public Task<bool> Post([FromBody] Articulo articulo)
        {
            return articuloBusiness.SaveAsync(articulo);
        }

        // PUT: api/ArticuloApi/5
        [HttpPut("{id}")]
        public async Task<bool> Put(int id, [FromBody] Articulo articulo)
        {
            if (articulo is null) return false;

            if (articulo.Id == 0)
                articulo.Id = id;
            else if (articulo.Id != id)
                return false;

            return await articuloBusiness.SaveAsync(articulo);
        }

        // DELETE: api/ArticuloApi/5
        [HttpDelete("{id}")]
        public Task<bool> Delete(int id)
        {
            return articuloBusiness.DeleteAsync(id);
        }
    }
}
