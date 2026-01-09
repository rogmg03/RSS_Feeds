using System.Text.Json.Serialization;

namespace RSS_Feeds.MVC.Models
{
    public class RssFinderFeedResponse
    {
        [JsonPropertyName("data")]
        public RssFinderData? Data { get; set; }
    }

    public class RssFinderData
    {
        [JsonPropertyName("searchInFinder")]
        public SearchInFinder? SearchInFinder { get; set; }
    }

    public class SearchInFinder
    {
        [JsonPropertyName("textResult")]
        public TextResult? TextResult { get; set; }

        [JsonPropertyName("urlResult")]
        public TextResult? UrlResult { get; set; }
    }

    public class TextResult
    {
        [JsonPropertyName("feeds")]
        public List<RssFinderFeedItem>? Feeds { get; set; }

        [JsonPropertyName("related")]
        public List<string>? Related { get; set; }
    }

    public class RssFinderFeedItem
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("icon")]
        public string? Icon { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("articles")]
        public object? Articles { get; set; }

        [JsonPropertyName("__typename")]
        public string? TypeName { get; set; }
    }
}


