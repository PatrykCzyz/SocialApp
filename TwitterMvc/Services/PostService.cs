using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TwitterMvc.Data.Context;
using TwitterMvc.Dtos;
using TwitterMvc.Services.Interfaces;

namespace TwitterMvc.Services
{
    public class PostService : IPostService
    {
        private readonly IdentityDatabaseContext _context;

        public PostService(IdentityDatabaseContext context)
        {
            _context = context;
        }

        public Task CreatePost(PostDto postDto)
        {
            throw new NotImplementedException();
        }

        public Task EditPost(int postId, PostDto postDto)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PostDto>> GetPosts(string userId)
        {
            var data = await _context.Posts.Where(post => post.UserId == userId).ToListAsync();

            var result = data.Select(post => new PostDto(post)).ToList();

            return result;
        }

        public Task RemovePost(int postId)
        {
            throw new NotImplementedException();
        }
    }
}
