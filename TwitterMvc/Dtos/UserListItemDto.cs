using System;
using Microsoft.AspNetCore.Identity;
using TwitterMvc.Enums;
using TwitterMvc.Models;

namespace TwitterMvc.Dtos
{
    public class UserListItem
    {
		public UserListItem(CustomUser user)
		{
			Id = user.Id;
			Username = user.UserName;
			Gender = user.Gender;
		}

        public string Id { get; set; }
        public string Username { get; set; }
        public GenderEnum Gender { get; set; }
    }
}
