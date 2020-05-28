using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public PostService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ReturnValues<bool>> CreatePost(string userId, PostDto postDto)
        {
            var post = new Post(userId, postDto);

            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();

            return new ReturnValues<bool>(true);
        }

        public Task<ReturnValues<bool>> EditPost(int postId, PostDto postDto)
        {
            throw new NotImplementedException();
        }

        public async Task<ReturnValues<List<GetPostDto>>> GetPosts(string userId)
        {
            var userExist = await _context.Users.AnyAsync(x => x.Id == userId);
            if (userId == null || !userExist)
            {
                return new ReturnValues<List<GetPostDto>>("User doesn't exist.");
            }
            
            var data = await _context.Posts.Where(post => post.UserId == userId).OrderByDescending(post => post.DateTime).ToListAsync();
            if (data.Count == 0)
            {
                return new ReturnValues<List<GetPostDto>>("There is no post yet!");
            }

            var result= data.Select(post => new GetPostDto(post)).ToList();
            
            return new ReturnValues<List<GetPostDto>>(result);
        }

        public Task<ReturnValues<bool>> RemovePost(int postId)
        {
            throw new NotImplementedException();
        }
    }
}
