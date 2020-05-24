using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TwitterMvc.Dtos;
using TwitterMvc.Services.Interfaces;

namespace TwitterMvc.Services
{
    public class PostService : IPostService
    {
        public PostService()
        {
        }

        public Task CreatePost(PostDto postDto)
        {
            throw new NotImplementedException();
        }

        public Task EditPost(int postId, PostDto postDto)
        {
            throw new NotImplementedException();
        }

        public Task<List<PostDto>> GetPosts()
        {
            throw new NotImplementedException();
        }

        public Task RemovePost(int postId)
        {
            throw new NotImplementedException();
        }
    }
}
