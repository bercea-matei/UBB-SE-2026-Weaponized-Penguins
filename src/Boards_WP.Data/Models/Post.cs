using System;

namespace Boards_WP.Data.Models
{
    public class Post
    {
        public int postID { get; set; }
        public int ownerID { get; set; }
        public int communityID { get; set; }
        public String title { get; set; } = String.Empty;
        public String description { get; set; } = String.Empty;
        public byte[]? image { get; set; }
        public int score { get; set; }
        public int commentsNumber { get; set; }
        public DateTime creationTime { get; set; }
        public String ownerName { get; set; } = String.Empty;
        public String communityName { get; set; } = String.Empty;
        public byte[]? communityPicture { get; set; }
        public VoteType userCurrentVote { get; set; } = VoteType.None;
        public String getShareLink => $"boards://post/{postID}";
        public List<Tag> tags { get; set; } = new List<Tag>();
    }
}