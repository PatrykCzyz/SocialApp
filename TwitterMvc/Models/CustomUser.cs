using System;
using Microsoft.AspNetCore.Identity;
using TwitterMvc.Dtos;
using TwitterMvc.Enums;

namespace TwitterMvc.Models
{
    public class CustomUser : IdentityUser
    {
        public CustomUser()
        {

        }

        public CustomUser(RegisterDto item)
        {
            UserName = item.UserName;
            Email = item.Email;
            Name = item.Name;
            Lastname = item.Lastname;
            Age = (int)item.Age;
            Gender = item.Gender;
            Country = item.Country;
        }

        public string Name { get; set; }
        public string Lastname { get; set; }
        public int Age { get; set; }
        public GenderEnum Gender { get; set; }
        public string Country { get; set; }
    }
}
