using System;
using TwitterMvc.Models;

namespace TwitterMvc.Dtos
{
    public class PostDto
    {
        public PostDto()
        {

        }

        public PostDto(Post post)
        {
            Title = post.Title;
            Content = post.Content;
            DateTime = post.DateTime;
        }

        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DateTime { get; set; }
    }
}
