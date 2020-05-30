using System.Collections.Generic;

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
            errorList.Add("RemovePostFailed", "You can't remove this post.");
            errorList.Add("EditPostFailed", "You can't edit this post.");
        }
        public string GetError(string errorKey)
        {
            return errorList[errorKey];
        }
    }
}