using System;
using System.Collections.Generic;
using System.Text;

namespace Boards_WP.Data.Models;

public class CommentsVotes
{
    public int userID { get; set; }
    public int commentID { get; set; }
    public VoteType vote { get; set; } = VoteType.None;

}
