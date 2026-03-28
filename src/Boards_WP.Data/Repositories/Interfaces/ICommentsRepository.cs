using System;
using System.Collections.Generic;
using System.Text;

using Boards_WP.Data.Models;

namespace Boards_WP.Data.Repositories.Interfaces;

public interface ICommentsRepository
{
    void addComment(Comment c);
    void softDeleteComment(int commentID);
    void increaseScore(Comment c);
    void decreaseScore(Comment c);
    List<Comment> getCommentsByPostID(int postID, int userID);
    void upsertUserCommentVote(int commentID, int currentUserID, VoteType vote);
}
