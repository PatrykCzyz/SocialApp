using System;

namespace TwitterMvc.Helpers
{
    public class ReturnValues<T>
    {
        public ReturnValues(T content)
        {
            Succeeded = true;
            ErrorMessage = null;
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