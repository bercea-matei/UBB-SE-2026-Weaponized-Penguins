using System;
using System.Collections.Generic;
using System.Text;

namespace Boards_WP.Data.Models;

public class CommentsVotes
{
    public int UserID { get; set; }
    public int CommentID { get; set; }
    public VoteType Vote { get; set; } = VoteType.None;

}
