using System;
using System.Collections.Generic;

namespace RSS_Feeds.Data.Models;

public partial class UsuarioFeed
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }

    public int FeedId { get; set; }

    public string? Alias { get; set; }

    public DateTime? CreadoEn { get; set; }

    public virtual Feed Feed { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
