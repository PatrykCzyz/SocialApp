using System;
using System.Collections.Generic;
using AutoMapper;
using TwitterMvc.Dtos;
using TwitterMvc.Dtos.AccountDtos;
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
            CreateMap<CustomUser, EditDto>();
        }
    }
}