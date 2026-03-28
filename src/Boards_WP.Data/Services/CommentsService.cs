using System;
using System.Collections.Generic;
using System.Text;

using Boards_WP.Data.Models;
using Boards_WP.Data.Repositories;

namespace Boards_WP.Data.Services;


internal class CommentsService : ICommentsService
{
    private readonly ICommentsRepository commentsRepo;
    //private readonly INotificationsRepository notificationsRepo;
    const int MAX_DESCRIPTION_LENGTH = 618;
    const int MAX_INDENTATION_LEVEL = 7;

    public CommentsService(ICommentsRepository commentsReop/*, INotificationsRepository notificationsRepo*/)
    {
        this.commentsRepo = commentsReop;
        //this.notificationsRepo = notificationsRepo;
    }

    public void addComment(Comment c)
    {
        validateComment(c);
        c.creationTime = DateTime.Now;
        c.isDeleted = false;
        commentsRepo.addComment(c);
        //notificationsRepo.addNotification(c);
    }

    public void softDeleteComment(Comment c, int userID)
    {

        if (c.ownerID == userID)
        {
            c.description = "[deleted]";
            c.isDeleted= true;
            commentsRepo.softDeleteComment(c.commentID);
        }
    }
    public void increaseComment(Comment c, int currentUserID)
    {
        if (c.isDeleted)
            throw new InvalidOperationException("Cannot vote on a deleted comment.");

        if (c.userCurrentVote == VoteType.Like)
        {
            commentsRepo.decreaseScore(c);
            commentsRepo.upsertUserCommentVote(c.commentID, currentUserID, VoteType.None);
            c.userCurrentVote = VoteType.None;
        }
        else if (c.userCurrentVote == VoteType.Dislike)
        {
            commentsRepo.increaseScore(c);
            commentsRepo.increaseScore(c);
            commentsRepo.upsertUserCommentVote(c.commentID, currentUserID, VoteType.Like);
            c.userCurrentVote = VoteType.Like;
        }
        else
        {
            commentsRepo.increaseScore(c);
            commentsRepo.upsertUserCommentVote(c.commentID, currentUserID, VoteType.Like);
            c.userCurrentVote = VoteType.Like;
        }
    }
    public void decreaseComment(Comment c, int currentUserID)
    {
        if (c.isDeleted)
            throw new InvalidOperationException("Cannot vote on a deleted comment.");

        if (c.userCurrentVote == VoteType.Dislike)
        {
            commentsRepo.increaseScore(c);
            commentsRepo.upsertUserCommentVote(c.commentID, currentUserID, VoteType.None);
            c.userCurrentVote = VoteType.None;
        }
        else if (c.userCurrentVote == VoteType.Like)
        {
            commentsRepo.decreaseScore(c);
            commentsRepo.decreaseScore(c);
            commentsRepo.upsertUserCommentVote(c.commentID, currentUserID, VoteType.Dislike);
            c.userCurrentVote = VoteType.Dislike;
        }
        else
        {
            commentsRepo.decreaseScore(c);
            commentsRepo.upsertUserCommentVote(c.commentID, currentUserID, VoteType.Dislike);
            c.userCurrentVote = VoteType.Dislike;
        }

    }
    public List<Comment> GetCommentsByPost(int postID, int currentUserID)
    {
        return commentsRepo.getCommentsByPostID(postID, currentUserID);
    }
    public void validateComment(Comment c)
    {
        if (string.IsNullOrWhiteSpace(c.description))
            throw new ArgumentException("Comment description cannot be empty.");
        if (c.description.Length > MAX_DESCRIPTION_LENGTH)
            throw new ArgumentException("Comment description cannot exceed 1000 characters.");
        if (c.indentation> MAX_INDENTATION_LEVEL)
            throw new ArgumentException("Comment indentation cannot exceed 7 levels.");
    }

    public List<Comment> getCommentsByPost(int postID, int currentUserID)
    {
        return commentsRepo.getCommentsByPostID(postID, currentUserID);
    }
}

