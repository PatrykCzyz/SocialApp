using System;

namespace TwitterMvc.Dtos.QuestionAndAnswerDtos
{
    public class GetAnswerdQuestionDto
    {
        public string QuestionMessage { get; set; }
        public string AnswerMessage { get; set; }
        public string SenderId { get; set; }
        public string SenderName { get; set; }
        public string ReceiverId { get; set; }
        public string ReceiverName { get; set; }
        public DateTime AnsweredTime { get; set; }
    }
}