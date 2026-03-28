using System;
using System.Collections.Generic;
using System.Text;

using Boards_WP.Data.Models;

namespace Boards_WP.Data.Services.Interfaces;

public interface ICommentsService
{
    public void addComment(Comment c);
    public void softDeleteComment(Comment c, int userID);
    public void increaseComment(Comment c, int currentUserID);
    public void decreaseComment(Comment c, int currentUserID);
    public List<Comment> getCommentsByPost(int postID, int currentUserID);

}
