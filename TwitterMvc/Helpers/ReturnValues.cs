using System;

namespace TwitterMvc.Helpers
{
    public class ReturnValues<T>
    {
        public T Result { get; set; }
        public string Error { get; set; }
    }
}