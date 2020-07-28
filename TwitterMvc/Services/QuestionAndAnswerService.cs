using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using TwitterMvc.Data.Context;
using TwitterMvc.Dtos.QuestionAndAnswerDtos;
using TwitterMvc.Helpers;
using TwitterMvc.Services.Interfaces;

namespace TwitterMvc.Services
{
  public class QuestionAndAnswerService : IQuestionAndAnswerService
  {
    private readonly AppDbContext _context;
    private readonly IErrorService _errorService;
    private readonly IMapper _mapper;

    public QuestionAndAnswerService(AppDbContext context, IErrorService errorService, IMapper mapper)
    {
      _context = context;
      _errorService = errorService;
      _mapper = mapper;
    }

    public Task<ReturnValues<bool>> AnswerToQuestion(string userId, int questionId, string answer)
    {
      throw new System.NotImplementedException();
    }

    public Task<ReturnValues<List<GetAnswerdQuestionDto>>> GetAnsweredQuestions(string userId)
    {
      throw new System.NotImplementedException();
    }

    public Task<ReturnValues<List<GetQuestionDto>>> GetQuestions(string userId)
    {
      throw new System.NotImplementedException();
    }

    public Task<ReturnValues<bool>> SendQuestion(string senderId, string receiverId, string question)
    {
      throw new System.NotImplementedException();
    }
  }
}