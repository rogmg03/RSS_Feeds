using System;
using System.Text.Json.Serialization;

namespace RSS_Feeds.Models.DTOs
{
    public class UsuarioDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; } = null!;

        [JsonPropertyName("passwordHash")]
        public string PasswordHash { get; set; } = null!;

        [JsonPropertyName("creadoEn")]
        public DateTime? CreadoEn { get; set; }
    }
}

