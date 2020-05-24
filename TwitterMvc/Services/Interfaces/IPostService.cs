using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TwitterMvc.Dtos;

namespace TwitterMvc.Services.Interfaces
{
    public interface IPostService
    {
        public Task CreatePost(PostDto postDto);
        public Task RemovePost(int postId);
        public Task EditPost(int postId, PostDto postDto);
        public Task<List<PostDto>> GetPosts();
    }
}
