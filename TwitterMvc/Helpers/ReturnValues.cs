using System;

namespace TwitterMvc.Helpers
{
    public class ReturnValues<T>
    {
        public ReturnValues()
        {
            Succeeded = true;
        }

        public ReturnValues(T content)
        {
            Succeeded = true;
            Content = content;
        }

        public ReturnValues(string errorMessage)
        {
            Succeeded = false;
            ErrorMessage = errorMessage;
            Content = default;
        }
        
        public T Content { get; set; }
        public bool Succeeded { get; set; }
        public string ErrorMessage { get; set; }
    }
}