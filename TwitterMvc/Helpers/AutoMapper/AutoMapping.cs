using System;
using System.Collections.Generic;
using AutoMapper;
using TwitterMvc.Dtos;
using TwitterMvc.Dtos.AccountDtos;
using TwitterMvc.Dtos.QuestionAndAnswerDtos;
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
            CreateMap<Question, GetQuestionDto>();
            CreateMap<Answer, GetAnswerdQuestionDto>()
                .ForMember(m => m.ReceiverId, 
                    o => o.MapFrom(f => f.Question.ReceiverId))
                .ForMember(m => m.ReceiverName,
                    o => o.MapFrom(f => f.Question.Receiver.Name))
                .ForMember(m => m.SenderId, 
                    o => o.MapFrom(f => f.Question.SenderId))
                .ForMember(m => m.SenderName,
                    o => o.MapFrom(f => f.Question.Sender.Name));
        }
    }
}