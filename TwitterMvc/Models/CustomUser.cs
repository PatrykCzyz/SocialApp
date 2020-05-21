using System;
using Microsoft.AspNetCore.Identity;
using TwitterMvc.Enums;

namespace TwitterMvc.Models
{
    public class CustomUser : IdentityUser
    {
        public string Name { get; set; }
        public string Lastname { get; set; }
        public int Age { get; set; }
        public GenderEnum Gender { get; set; }
        public string Country { get; set; }
    }
}
