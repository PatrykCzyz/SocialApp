using System;

namespace TwitterMvc.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime AnsweredTime { get; set; }
        public Question Question { get; set; }
    }
}