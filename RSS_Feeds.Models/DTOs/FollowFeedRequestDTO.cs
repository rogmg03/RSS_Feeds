using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSS_Feeds.Models.DTOs
{
    public class FollowFeedRequestDTO
    {
        public int FeedId { get; set; }
        public string? Alias { get; set; }
    }

}
