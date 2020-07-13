using System;
using System.Collections.Generic;
using AutoMapper;
using TwitterMvc.Dtos;
using TwitterMvc.Models;

namespace TwitterMvc.Helpers.AutoMapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Post, PostDto>();
            CreateMap<PostDto, Post>();
            CreateMap<Post, GetPostDto>();
            CreateMap<GetPostDto, PostDto>();
        }
    }
}