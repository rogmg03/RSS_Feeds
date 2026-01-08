using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RSS_Feeds.Data.Models;

public partial class UsuarioFeed
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }

    public int FeedId { get; set; }

    public string? Alias { get; set; }

    public DateTime? CreadoEn { get; set; }

    [JsonIgnore]
    public virtual Feed? Feed { get; set; } = null!;
    [JsonIgnore]
    public virtual Usuario? Usuario { get; set; } = null!;
}
