using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TwitterMvc.Data.Context;
using TwitterMvc.Dtos;
using TwitterMvc.Helpers;
using TwitterMvc.Models;
using TwitterMvc.Services.Interfaces;

namespace TwitterMvc.Services
{
    public class PostService : IPostService
    {
        private readonly AppDbContext _context;
        private readonly IErrorService _errorService;
        private readonly IMapper _mapper;

        public PostService(AppDbContext context, IErrorService errorService, IMapper mapper)
        {
            _context = context;
            _errorService = errorService;
            _mapper = mapper;
        }

        public async Task<ReturnValues<bool>> CreatePost(string userId, PostDto postDto)
        {
            if (!await IsUserExist(userId))
                return new ReturnValues<bool>(_errorService.GetError("UserDosentExist"));
            
            if (!IsPostDtoValid(postDto))
                return new ReturnValues<bool>(_errorService.GetError("PostDtoNotFilled"));
            
            var post = GetEntityPost(userId, postDto);

            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();

            return new ReturnValues<bool>(true);
        }

        #region CreatePostMethods
        
        private static bool IsPostDtoValid(PostDto postDto)
        {
            return postDto != null && postDto.Title != null && postDto.Content != null;
        }
        
        private Post GetEntityPost(string userId, PostDto postDto)
        {
            var post = _mapper.Map<Post>(postDto);
            post.UserId = userId;
            post.DateTime = DateTime.Now;
            return post;
        }

        #endregion

        public Task<ReturnValues<bool>> EditPost(string userId, int postId, PostDto postDto)
        {
            throw new NotImplementedException();
        }

        public async Task<ReturnValues<List<GetPostDto>>> GetPosts(string userId)
        {
            if (!await IsUserExist(userId))
                return new ReturnValues<List<GetPostDto>>(_errorService.GetError("UserDosentExist"));
            
            var data = await _context.Posts.Where(post => post.UserId == userId).OrderByDescending(post => post.DateTime).ToListAsync();
            if (data.Count == 0)
                return new ReturnValues<List<GetPostDto>>(_errorService.GetError("NoPost"));

            var result = _mapper.Map<List<Post>, List<GetPostDto>>(data);
            
            return new ReturnValues<List<GetPostDto>>(result);
        }

        public async Task<ReturnValues<bool>> RemovePost(string userId, int postId)
        {
            if(!await IsUserExist(userId))
                return new ReturnValues<bool>(_errorService.GetError("UserDosentExist"));

            var post = await _context.Posts.FirstOrDefaultAsync(x => x.UserId == userId && x.Id == postId);
            if(post == null)
                return new ReturnValues<bool>(_errorService.GetError("RemovePostFailed"));

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return new ReturnValues<bool>(true);
        }

        #region Methods

        private async Task<bool> IsUserExist(string userId)
        {
            var userExist = await _context.CustomUsers.AnyAsync(x => x.Id == userId);
            return userExist;
        }

        #endregion
    }
}
