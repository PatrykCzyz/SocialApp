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

        public async Task CreatePost(string userId, PostDto postDto)
        {
            var post = new Post(userId, postDto);

            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
        }

        public Task EditPost(int postId, PostDto postDto)
        {
            throw new NotImplementedException();
        }

        public async Task<ReturnValues<List<GetPostDto>>> GetPosts(string userId)
        {
            var returnValues = new ReturnValues<List<GetPostDto>>();
            
            var data = await _context.Posts.Where(post => post.UserId == userId).OrderByDescending(post => post.DateTime).ToListAsync();

            returnValues.Result = data.Select(post => new GetPostDto(post)).ToList();

            return returnValues;
        }

        public Task RemovePost(int postId)
        {
            throw new NotImplementedException();
        }
    }
}
