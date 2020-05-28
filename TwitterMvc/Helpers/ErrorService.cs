using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace TwitterMvc.Helpers
{
    public class ErrorService : IErrorService
    {
        private Dictionary<string, string> errorList;
        
        public ErrorService()
        {
            errorList = new Dictionary<string, string>();
            errorList.Add("UserDosentExist", "User doesn't exist.");
            errorList.Add("NoPost", "There is no post yet!");
            errorList.Add("PostDtoNotFilled", "You have to fill all fields.");
        }
        public string GetError(string errorKey)
        {
            return errorList[errorKey];
        }
    }
}