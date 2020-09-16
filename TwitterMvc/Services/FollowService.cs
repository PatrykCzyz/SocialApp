using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TwitterMvc.Data.Context;
using TwitterMvc.Dtos.UserDtos;
using TwitterMvc.Helpers;
using TwitterMvc.Helpers.ErrorHandler;
using TwitterMvc.Models;
using TwitterMvc.Services.Interfaces;

namespace TwitterMvc.Services
{
    public class FollowService : IFollowService
    {
        private readonly AppDbContext _context;
        private readonly IErrorService _errorService;
        private readonly IMapper _mapper;

        public FollowService(AppDbContext context, IErrorService errorService, IMapper mapper)
        {
            _context = context;
            _errorService = errorService;
            _mapper = mapper;
        }

        public async Task<ReturnValues<bool>> Follow(string userId, string userToFollowId)
        {
            if(!await UsersExists(userId, userToFollowId))
                return new ReturnValues<bool>(_errorService.GetError(Error.UserDosentExist));

            if(await UserAlreadyFollowed(userId, userToFollowId))
                return new ReturnValues<bool>(_errorService.GetError(Error.UserIsAlreadyFollowed));

            await _context.AddAsync(new Follow(userId, userToFollowId));
            await _context.SaveChangesAsync();

            return new ReturnValues<bool>();
        }

        public async Task<ReturnValues<bool>> UnFollow(string userId, string userToUnFollowId)
        {
            if (!await UsersExists(userId, userToUnFollowId))
                return new ReturnValues<bool>(_errorService.GetError(Error.UserDosentExist));

            if (!await UserAlreadyFollowed(userId, userToUnFollowId))
                return new ReturnValues<bool>(_errorService.GetError(Error.UserIsNotFollowed));

            var follow = await _context.Follows.FirstAsync(x => x.UserId == userId && x.FollowUserId == userToUnFollowId);

            _context.Remove(follow);
            await _context.SaveChangesAsync();

            return new ReturnValues<bool>();
        }

        public async Task<ReturnValues<List<UserListItemDto>>> GetFollowers(string userId)
        {
            if (!await UserExist(userId))
                return new ReturnValues<List<UserListItemDto>>(_errorService.GetError(Error.UserDosentExist));

            var result = await _context.Follows.Where(x => x.FollowUserId == userId)
                .Select(x => new UserListItemDto(x.User)).ToListAsync();

            if (result.Count == 0)
                return new ReturnValues<List<UserListItemDto>>(_errorService.GetError(Error.DontHaveFollowers));

            return new ReturnValues<List<UserListItemDto>>(result);
        }

        public async Task<ReturnValues<List<UserListItemDto>>> GetFollowing(string userId)
        {
            if (!await UserExist(userId))
                return new ReturnValues<List<UserListItemDto>>(_errorService.GetError(Error.UserDosentExist));

            var result = await _context.Follows.Where(x => x.UserId == userId)
                .Select(x => new UserListItemDto(x.FollowUser)).ToListAsync();

            if (result.Count == 0)
                return new ReturnValues<List<UserListItemDto>>(_errorService.GetError(Error.DontHaveFollowing));

            return new ReturnValues<List<UserListItemDto>>(result);
        }

        public async Task<ReturnValues<bool>> Followed(string userId, string secondUserId)
        {
            if(!await UsersExists(userId, secondUserId))
            {
                return new ReturnValues<bool>(_errorService.GetError(Error.UserDosentExist));
            }
            return new ReturnValues<bool>(await UserAlreadyFollowed(userId, secondUserId));
        }

        #region Methods
        private async Task<bool> UsersExists(string userId, string secondUserId)
        {
            var usersExist = await _context.CustomUsers.Where(x => x.Id == userId || x.Id == secondUserId).CountAsync();
            if (usersExist != 2)
                return false;
            return true;
        }

        private async Task<bool> UserAlreadyFollowed(string userId, string secondUserId)
        {
            var alreadyFollowed = await _context.Follows.AnyAsync(x => x.UserId == userId && x.FollowUserId == secondUserId);
            if (alreadyFollowed)
                return true;
            return false;
        }

        private async Task<bool> UserExist(string userId)
        {
            return await _context.CustomUsers.AnyAsync(x => x.Id == userId);
        }
        #endregion
    }
}
