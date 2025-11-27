using System;
using System.Text.Json.Serialization;

namespace RSS_Feeds.Models.DTOs
{
    public class FeedDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; } = null!;

        [JsonPropertyName("titulo")]
        public string? Titulo { get; set; }

        [JsonPropertyName("descripcion")]
        public string? Descripcion { get; set; }

        [JsonPropertyName("idioma")]
        public string? Idioma { get; set; }

        [JsonPropertyName("categoria")]
        public string? Categoria { get; set; }

        [JsonPropertyName("ultimaLectura")]
        public DateTime? UltimaLectura { get; set; }
    }
}
