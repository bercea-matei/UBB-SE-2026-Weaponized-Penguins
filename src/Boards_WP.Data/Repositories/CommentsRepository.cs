using System;
using System.Collections.Generic;
using System.Text;

using Boards_WP.Data.Models;
using Boards_WP.Data.Repositories.Interfaces;

namespace Boards_WP.Data.Repositories;

public class CommentsRepository : ICommentsRepository
{
    private String commentsDbContext;
    private String commentsVotesDbContext;
    private String usersDbContext;

    public CommentsRepository(String commentsDbContext, String commentsVotesDbContext, String usersDbContext)
    {
        this.commentsDbContext = commentsDbContext;
        this.commentsVotesDbContext = commentsVotesDbContext;
        this.usersDbContext = usersDbContext;
    }

    public void addComment(Comment c)
    {
        // TODO
    }

    public void softDeleteComment(int commentID)
    {
        // TODO
    }
    public void increaseScore(Comment c)
    {
        // TODO
    }
    public void decreaseScore(Comment c)
    {
        // TODO
    }

    public List<Comment> getCommentsByPostID(int postID, int userID)
    {
        // TODO
        return new List<Comment>();
    }
    public void upsertUserCommentVote(int commentID, int currentUserID, VoteType vote)
    {
        // TODO
    }

}
