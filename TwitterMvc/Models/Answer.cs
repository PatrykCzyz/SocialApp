using System;

namespace TwitterMvc.Models
{
    public class Answer
    {
        public Answer()
        {
                
        }

        public Answer(string message, int questionId)
        {
            Message = message;
            QuestionId = questionId;
        }
        
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime AnsweredTime { get; set; } = DateTime.Now;
        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}