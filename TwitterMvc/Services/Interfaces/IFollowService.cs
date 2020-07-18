using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TwitterMvc.Dtos;
using TwitterMvc.Helpers;

namespace TwitterMvc.Services.Interfaces
{
    public interface IFollowService
    {
        public Task<ReturnValues<bool>> Follow(string userId, string userToFollowId);
        public Task<ReturnValues<bool>> UnFollow(string userId, string userToUnFollowId);
        public Task<ReturnValues<List<UserListItemDto>>> GetFollowers(string userId);
        public Task<ReturnValues<List<UserListItemDto>>> GetFollowing(string userId);
    }
}
