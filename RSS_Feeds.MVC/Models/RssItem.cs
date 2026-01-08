namespace RSS_Feeds.MVC.Models
{
    public class RssItem
    {
        public string Title { get; set; } = "";
        public string Link { get; set; } = "";
        public string? Description { get; set; }
        public string? Author { get; set; }
        public DateTime? PublishedAt { get; set; }
        public string FeedTitle { get; set; } = "";
        public string FeedUrl { get; set; } = "";
        public string? ImageUrl { get; set; } = "";
    }
}
