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
            errorList.Add("UserIsAlreadyFollowed", "You already followed this user.");
            errorList.Add("UserIsNotFollowed", "You don't follow this user.");
            errorList.Add("DontHaveFollowers", "You don't have any followers.");
            errorList.Add("DontHaveFollowing", "You don't follow anyone.");
            errorList.Add("EmptyMessage", "Message cannot be empty.");
        }
        public string GetError(string errorKey)
        {
            return errorList[errorKey];
        }
    }
}