using System;
using TwitterMvc.Dtos;

namespace TwitterMvc.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DateTime { get; set; }

        public string UserId { get; set; }
        public CustomUser User { get; set; }
    }
}

