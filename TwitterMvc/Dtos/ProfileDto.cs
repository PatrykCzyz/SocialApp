using System;
using Microsoft.AspNetCore.Identity;
using TwitterMvc.Models;

namespace TwitterMvc.Dtos
{
    public class ProfileDto
    {
        public ProfileDto(CustomUser user)
        {
            Username = user.UserName;
            Email = user.Email;
        }

        public string Username { get; set; }
        public string Email { get; set; }
    }
}
