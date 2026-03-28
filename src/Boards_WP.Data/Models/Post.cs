using System;

namespace Boards_WP.Data.Models;

public class Post
{
    public int PostID { get; set; }
    public int OwnerID { get; set; }
    public int CommunityID { get; set; }
    public String Title { get; set; } = String.Empty;
    public String Description { get; set; } = String.Empty;
    public byte[]? Image { get; set; }
    public int Score { get; set; }
    public int CommentsNumber { get; set; }
    public DateTime CreationTime { get; set; }
    public String OwnerName { get; set; } = String.Empty;
    public String CommunityName { get; set; } = String.Empty;
    public byte[]? CommunityPicture { get; set; }
    public VoteType UserCurrentVote { get; set; } = VoteType.None;
    public String GetShareLink => $"boards://post/{postID}";
    public List<Tag> Tags { get; set; } = new List<Tag>();
}