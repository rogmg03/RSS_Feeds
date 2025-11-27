using System;
using System.Text.Json.Serialization;

namespace RSS_Feeds.Models.DTOs
{
    public class ArticuloDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("feedId")]
        public int FeedId { get; set; }

        [JsonPropertyName("titulo")]
        public string Titulo { get; set; } = null!;

        [JsonPropertyName("link")]
        public string Link { get; set; } = null!;

        [JsonPropertyName("descripcion")]
        public string? Descripcion { get; set; }

        [JsonPropertyName("contenido")]
        public string? Contenido { get; set; }

        [JsonPropertyName("autor")]
        public string? Autor { get; set; }

        [JsonPropertyName("imagen")]
        public string? Imagen { get; set; }

        [JsonPropertyName("fechaPublicacion")]
        public DateTime? FechaPublicacion { get; set; }

        [JsonPropertyName("creadoEn")]
        public DateTime? CreadoEn { get; set; }
    }
}
