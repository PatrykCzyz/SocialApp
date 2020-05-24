using System;
using TwitterMvc.Dtos;

namespace TwitterMvc.Models
{
    public class Post
    {
        public Post()
        {

        }

        public Post(string userId, PostDto postDto)
        {
            Title = postDto.Title;
            Content = postDto.Content;
            DateTime = DateTime.Now;
            UserId = userId;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DateTime { get; set; }

        public string UserId { get; set; }
        public CustomUser User { get; set; }
    }
}

