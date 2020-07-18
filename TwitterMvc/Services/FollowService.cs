using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using TwitterMvc.Data.Context;
using TwitterMvc.Dtos;
using TwitterMvc.Helpers;
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

        public Task<ReturnValues<bool>> Follow(string userId, string userToFollowId)
        {
            throw new NotImplementedException();
        }

        public Task<ReturnValues<List<UserListItemDto>>> GetFollowers(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<ReturnValues<List<UserListItemDto>>> GetFollowing(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<ReturnValues<bool>> UnFollow(string userId, string userToUnFollowId)
        {
            throw new NotImplementedException();
        }
    }
}
