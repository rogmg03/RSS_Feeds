using RSS_Feeds.MVC.Models;

namespace RSS_Feeds.MVC.Services
{
    public interface IRssReaderService
    {
        Task<List<RssItem>> ReadAsync(string feedUrl, string feedTitle);
    }
}
