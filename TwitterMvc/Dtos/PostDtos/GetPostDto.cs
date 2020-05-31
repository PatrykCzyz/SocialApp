using System;
using TwitterMvc.Models;

namespace TwitterMvc.Dtos
{
    public class GetPostDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DateTime { get; set; }
    }
}
