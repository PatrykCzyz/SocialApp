using System;

namespace TwitterMvc.Models
{
    public class Question
    {
        public Question()
        {
            
        }
        
        public Question(string senderId, string recieverId, string message)
        {
            SenderId = senderId;
            ReceiverId = recieverId;
            Message = message;
        }

        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime SentTime { get; set; } = DateTime.Now;
        public string SenderId { get; set; }
        public CustomUser Sender { get; set; }
        public string ReceiverId { get; set; }
        public CustomUser Receiver { get; set; }
        public Answer Answer { get; set; }
    }
}