using System;
using Microsoft.AspNetCore.Identity;
using TwitterMvc.Models;

namespace TwitterMvc.Dtos
{
    public class UserListItem
    {
		public UserListItem(CustomUser user)
		{
			Id = user.Id;
			Username = user.UserName;
		}

        public string Id { get; set; }
        public string Username { get; set; }
	}
}
