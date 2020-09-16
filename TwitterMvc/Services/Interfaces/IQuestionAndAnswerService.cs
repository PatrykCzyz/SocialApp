using System.Collections.Generic;
using System.Threading.Tasks;
using TwitterMvc.Dtos.QuestionAndAnswerDtos;
using TwitterMvc.Helpers;

namespace TwitterMvc.Services.Interfaces
{
    public interface IQuestionAndAnswerService
    {
        public Task<ReturnValues<bool>> SendQuestion(string senderId, string receiverId, string question);
        public Task<ReturnValues<bool>> AnswerToQuestion(string userId, int questionId, string answerMessage);
        public Task<ReturnValues<List<GetQuestionDto>>> GetQuestions(string userId);
        public Task<ReturnValues<List<GetAnswerdQuestionDto>>> GetAnsweredQuestions(string userId);
    }
}