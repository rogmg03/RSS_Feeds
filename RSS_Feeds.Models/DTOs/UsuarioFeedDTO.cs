using System;
using System.Text.Json.Serialization;

namespace RSS_Feeds.Models.DTOs
{
    public class UsuarioFeedDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("usuarioId")]
        public int UsuarioId { get; set; }

        [JsonPropertyName("feedId")]
        public int FeedId { get; set; }

        [JsonPropertyName("alias")]
        public string? Alias { get; set; }

        [JsonPropertyName("creadoEn")]
        public DateTime? CreadoEn { get; set; }
    }
}
