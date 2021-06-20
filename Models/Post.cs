using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Chatter.Models
{
    public class Post
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("author")]
        public string Author { get; set; }

        [JsonPropertyName("created")]
        public DateTime Created { get; set; }

        [JsonPropertyName("isFlagged")]
        public bool IsFlagged { get; set; }

        [JsonPropertyName("userImage")]
        public string UserImage { get; set; }

        [JsonPropertyName("thread")]
        public string Thread { get; set; }

        [JsonPropertyName("subThread")]
        public string Subthread { get; set; }

        [JsonPropertyName("messageThread")]
        public int MessageThread { get; set; }

        [JsonPropertyName("like")]
        public bool Like { get; set; }

        [JsonPropertyName("likeCount")]
        public int LikeCount { get; set; }

        [JsonPropertyName("love")]
        public bool Love { get; set; }

        [JsonPropertyName("loveCount")]
        public int LoveCount { get; set; }

        [JsonPropertyName("postImage")]
        public string PostImage { get; set; }


    }
}
