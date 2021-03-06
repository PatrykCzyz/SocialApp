﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TwitterMvc.Dtos;
using TwitterMvc.Helpers;

namespace TwitterMvc.Services.Interfaces
{
    public interface IPostService
    {
        public Task<ReturnValues<bool>> CreatePost(string userId, PostDto postDto);
        public Task<ReturnValues<bool>> RemovePost(string userId, int postId);
        public Task<ReturnValues<bool>> EditPost(string userId, int postId, PostDto postDto);
        public Task<ReturnValues<List<GetPostDto>>> GetPosts(string userId);
    }
}
