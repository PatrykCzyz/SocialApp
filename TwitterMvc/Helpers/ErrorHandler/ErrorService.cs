using System.Collections.Generic;

namespace TwitterMvc.Helpers.ErrorHandler
{
    public class ErrorService : IErrorService
    {
        private Dictionary<Error, string> errorList;
        
        public ErrorService()
        {
            errorList = new Dictionary<Error, string>();
            errorList.Add(Error.UserDosentExist, "User doesn't exist.");
            errorList.Add(Error.NoPost, "There is no post yet!");
            errorList.Add(Error.PostDtoNotFilled, "You have to fill all fields.");
            errorList.Add(Error.RemovePostFailed, "You can't remove this post.");
            errorList.Add(Error.EditPostFailed, "You can't edit this post.");
            errorList.Add(Error.UserIsAlreadyFollowed, "You already followed this user.");
            errorList.Add(Error.UserIsNotFollowed, "You don't follow this user.");
            errorList.Add(Error.DontHaveFollowers, "You don't have any followers.");
            errorList.Add(Error.DontHaveFollowing, "You don't follow anyone.");
            errorList.Add(Error.EmptyMessage, "Message cannot be empty.");
            errorList.Add(Error.QuestionDoesntExist, "Question doesn't exist.");
            errorList.Add(Error.EmptyAnswer, "Answer cannot be null or empty.");
            errorList.Add(Error.AlreadyAnswered, "You already answered to that question.");
        }
        public string GetError(Error errorKey)
        {
            var result = errorList.TryGetValue(errorKey, out var value);
            return result == true ? value : "Something went wrong.";
        }
    }
}