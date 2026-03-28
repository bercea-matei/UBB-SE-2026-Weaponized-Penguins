using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace Boards_WP.Data.Models
{
    public class Comment
    {
        public int commentID { get; set; }
        public int postID { get; set; }
        public int? parentID { get; set; }
        public String description { get; set; } = String.Empty;
        public int score { get; set; }
        public DateTime creationTime { get; set; }
        public int indentation { get; set; }
        public Boolean isDeleted { get; set; }
        public String ownerName { get; set; } = String.Empty;
        public byte[]? ownerPicture { get; set; }
        public VoteType userCurrentVote { get; set; } = VoteType.None;

        public String getShareLink => $"boards://post/{postID}";

    }
}
