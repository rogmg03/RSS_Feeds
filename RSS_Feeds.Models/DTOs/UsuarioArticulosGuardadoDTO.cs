using System;
using System.Text.Json.Serialization;

namespace RSS_Feeds.Models.DTOs
{
    public class UsuarioArticulosGuardadoDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("usuarioId")]
        public int UsuarioId { get; set; }

        [JsonPropertyName("articuloId")]
        public int ArticuloId { get; set; }

        [JsonPropertyName("fechaGuardado")]
        public DateTime? FechaGuardado { get; set; }

        [JsonPropertyName("notas")]
        public string? Notas { get; set; }
    }
}
