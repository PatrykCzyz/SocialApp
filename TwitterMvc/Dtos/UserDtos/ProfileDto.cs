using System;
using Microsoft.AspNetCore.Identity;
using TwitterMvc.Enums;
using TwitterMvc.Models;

namespace TwitterMvc.Dtos.UserDtos
{
    public class ProfileDto
    {
        public ProfileDto(CustomUser user)
        {
            Username = user.UserName;
            Email = user.Email;
            Name = user.Name;
            Lastname = user.Lastname;
            Age = user.Age;
            Gender = user.Gender;
            Country = user.Country;
        }

        public string Username { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public int Age { get; set; }
        public GenderEnum Gender { get; set; }
        public string Country { get; set; }
    }
}
