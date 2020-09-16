namespace TwitterMvc.Helpers.ErrorHandler
{
    public enum Error
    {
        /// <summary>
        /// "User doesn't exist."
        /// </summary>
        UserDosentExist,
        
        /// <summary>
        /// "There is no post yet!"
        /// </summary>
        NoPost,
        
        /// <summary>
        /// "You have to fill all fields."
        /// </summary>
        PostDtoNotFilled,
        
        /// <summary>
        /// "You can't remove this post."
        /// </summary>
        RemovePostFailed,
        
        /// <summary>
        /// "You can't edit this post."
        /// </summary>
        EditPostFailed,
        
        /// <summary>
        /// "You already followed this user."
        /// </summary>
        UserIsAlreadyFollowed,
        
        /// <summary>
        /// "You don't follow this user."
        /// </summary>
        UserIsNotFollowed,
        
        /// <summary>
        /// "You don't have any followers."
        /// </summary>
        DontHaveFollowers,
        
        /// <summary>
        /// "You don't follow anyone."
        /// </summary>
        DontHaveFollowing,
        
        /// <summary>
        /// "Message cannot be empty."
        /// </summary>
        EmptyMessage,
        
        /// <summary>
        /// "Question doesn't exist."
        /// </summary>
        QuestionDoesntExist,
        
        /// <summary>
        /// "Answer cannot be null or empty."
        /// </summary>
        EmptyAnswer,
        
        /// <summary>
        /// "You already answered to that question."
        /// </summary>
        AlreadyAnswered
    }
}