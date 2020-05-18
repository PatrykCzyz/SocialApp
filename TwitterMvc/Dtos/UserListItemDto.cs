using System;
using Microsoft.AspNetCore.Identity;

namespace TwitterMvc.Dtos
{
    public class UserListItem
    {
		public UserListItem(IdentityUser user)
		{
			Id = user.Id;
			Username = user.UserName;
		}

        public string Id { get; set; }
        public string Username { get; set; }
	}
}
