using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TwitterMvc.Data.Context;
using TwitterMvc.Dtos.QuestionAndAnswerDtos;
using TwitterMvc.Helpers;
using TwitterMvc.Helpers.ErrorHandler;
using TwitterMvc.Models;
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

    public async Task<ReturnValues<bool>> SendQuestion(string senderId, string receiverId, string message)
    {
		var usersExist = await _context.CustomUsers.CountAsync(x => x.Id == senderId || x.Id == receiverId);
		if(usersExist != 2)
			return new ReturnValues<bool>(_errorService.GetError(Error.UserDosentExist));

		if(string.IsNullOrEmpty(message))
			return new ReturnValues<bool>(_errorService.GetError(Error.EmptyMessage));

		var question = new Question(senderId, receiverId, message);

		await _context.AddAsync(question);
		await _context.SaveChangesAsync();

		return new ReturnValues<bool>();
    }

    public async Task<ReturnValues<bool>> AnswerToQuestion(string userId, int questionId, string answerMessage)
    {
        if(string.IsNullOrEmpty(userId))
            return new ReturnValues<bool>(_errorService.GetError(Error.UserDosentExist));
        
        if (string.IsNullOrEmpty(answerMessage))
            return new ReturnValues<bool>(_errorService.GetError(Error.EmptyAnswer));
        
        var question = await _context.Questions
            .Include(i => i.Answer)
            .FirstOrDefaultAsync(x => x.Id == questionId && x.ReceiverId == userId);
        if(question == null)
            return new ReturnValues<bool>(_errorService.GetError(Error.QuestionDoesntExist));
        if(question.Answer != null)
            return new ReturnValues<bool>(_errorService.GetError(Error.AlreadyAnswered));

        var answer = new Answer(answerMessage, questionId);
        await _context.AddRangeAsync(answer);
        await _context.SaveChangesAsync();
        
        return new ReturnValues<bool>();
    }

    public Task<ReturnValues<List<GetAnswerdQuestionDto>>> GetAnsweredQuestions(string userId)
    {
      throw new System.NotImplementedException();
    }

    public Task<ReturnValues<List<GetQuestionDto>>> GetQuestions(string userId)
    {
      throw new System.NotImplementedException();
    }
  }
}