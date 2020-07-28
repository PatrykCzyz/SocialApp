using System;

namespace TwitterMvc.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime SentTime { get; set; }
        public string SenderId { get; set; }
        public CustomUser Sender { get; set; }
        public string ReceiverId { get; set; }
        public CustomUser Receiver { get; set; }
        public int AnswerId { get; set; }
        public Answer Answer { get; set; }
    }
}