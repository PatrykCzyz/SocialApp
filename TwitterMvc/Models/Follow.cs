using System;
namespace TwitterMvc.Models
{
    public class Follow
    {
        public Follow()
        {

        }

        public Follow(string userId, string followUserId)
        {
            UserId = userId;
            FollowUserId = followUserId;
        }

        public int Id { get; set; }

        public string UserId { get; set; }
        public CustomUser User { get; set; }

        public string FollowUserId { get; set; }
        public CustomUser FollowUser { get; set; }
    }
}
