using RSS_Feeds.MVC.Models;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace RSS_Feeds.MVC.Services
{
    public class RssReaderService : IRssReaderService
    {
        public async Task<List<RssItem>> ReadAsync(string feedUrl, string feedTitle)
        {
            using var http = new HttpClient();
            var xml = await http.GetStringAsync(feedUrl);

            var doc = XDocument.Parse(xml);

            XNamespace media = "http://search.yahoo.com/mrss/";
            XNamespace content = "http://purl.org/rss/1.0/modules/content/";

            return doc.Descendants("item")
                .Select(item =>
                {
                    var description = item.Element("description")?.Value;
                    var htmlContent = item.Element(content + "encoded")?.Value;

                    string? imageUrl = ExtractImage(item, media, description, htmlContent);

                    return new RssItem
                    {
                        Title = item.Element("title")?.Value ?? "",
                        Link = item.Element("link")?.Value ?? "",
                        Description = description,
                        Author = item.Element("author")?.Value,
                        PublishedAt = ParseDate(item.Element("pubDate")?.Value),
                        FeedTitle = feedTitle,
                        FeedUrl = feedUrl,
                        ImageUrl = imageUrl
                    };
                })
                .ToList();
        }

        private static string? ExtractImage(
    XElement item,
    XNamespace media,
    string? description,
    string? content)
        {
            // 1️ enclosure
            var enclosure = item.Elements("enclosure")
                .FirstOrDefault(e =>
                    e.Attribute("type")?.Value.StartsWith("image") == true);

            if (enclosure != null)
                return enclosure.Attribute("url")?.Value;

            // 2️ media:content
            var mediaContent = item.Elements(media + "content")
                .FirstOrDefault();

            if (mediaContent != null)
                return mediaContent.Attribute("url")?.Value;

            // 3️ media:thumbnail (BBC, CNN, etc.)
            var mediaThumbnail = item.Elements(media + "thumbnail")
                .FirstOrDefault();

            if (mediaThumbnail != null)
                return mediaThumbnail.Attribute("url")?.Value;

            // 4️ img en HTML
            var html = content ?? description;
            if (!string.IsNullOrWhiteSpace(html))
            {
                var match = Regex.Match(
                    html,
                    "<img[^>]+src=[\"']([^\"']+)[\"']",
                    RegexOptions.IgnoreCase);

                if (match.Success)
                    return match.Groups[1].Value;
            }

            return null;
        }

        private static DateTime? ParseDate(string? value)
        {
            if (DateTime.TryParse(value, out var dt))
                return dt;

            return null;
        }
    }
}
