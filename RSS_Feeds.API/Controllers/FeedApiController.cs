using Microsoft.AspNetCore.Mvc;
using RSS_Feeds.Core.BusinessLogic;
using RSS_Feeds.Data.Models;

namespace RSS_Feeds.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedApiController(IFeedBusiness feedBusiness) : ControllerBase
    {
        // GET: api/FeedApi
        [HttpGet]
        public async Task<IEnumerable<Feed>> Get()
        {
            return await feedBusiness.GetAsync();
        }

        // GET: api/FeedApi/5
        [HttpGet("{id}")]
        public async Task<IEnumerable<Feed>> Get(int id)
        {
            return await feedBusiness.GetByIdAsync(id);
        }

        // POST: api/FeedApi
        [HttpPost]
        public Task<bool> Post([FromBody] Feed feed)
        {
            return feedBusiness.SaveAsync(feed);
        }

        // PUT: api/FeedApi/5
        [HttpPut("{id}")]
        public async Task<bool> Put(int id, [FromBody] Feed feed)
        {
            if (feed is null) return false;

            // Validamos consistencia de Id
            if (feed.Id == 0)
                feed.Id = id;
            else if (feed.Id != id)
                return false;

            return await feedBusiness.SaveAsync(feed);
        }

        // DELETE: api/FeedApi/5
        [HttpDelete("{id}")]
        public Task<bool> Delete(int id)
        {
            return feedBusiness.DeleteAsync(id);
        }
    }
}
