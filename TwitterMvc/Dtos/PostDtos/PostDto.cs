using System;
using System.ComponentModel.DataAnnotations;
using TwitterMvc.Models;

namespace TwitterMvc.Dtos
{
    public class PostDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
