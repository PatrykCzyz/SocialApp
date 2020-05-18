using System;
using Microsoft.AspNetCore.Identity;

namespace TwitterMvc.Dtos
{
    public class UserListItem
    {
		public UserListItem(IdentityUser user)
		{
			Username = user.UserName;
		}

		public string Username { get; set; }
	}
}
