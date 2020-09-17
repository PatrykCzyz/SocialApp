using System;

namespace TwitterMvc.Dtos.QuestionAndAnswerDtos
{
    public class GetQuestionDto
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string SenderName { get; set; }
        public string QuestionMessage { get; set; }
        public DateTime SentTime { get; set; }
    }
}