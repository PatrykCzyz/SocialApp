using System;
using Microsoft.AspNetCore.Identity;

namespace TwitterMvc.Dtos
{
    public class ProfileDto
    {
        public ProfileDto(IdentityUser user)
        {
            Username = user.UserName;
            Email = user.Email;
        }

        public string Username { get; set; }
        public string Email { get; set; }
    }
}
