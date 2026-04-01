using System;
using System.Collections.Generic;
using System.Linq;
using Boards_WP.Data.Models;
using Boards_WP.Data.Repositories.Interfaces;
using Boards_WP.Data.Services.Interfaces;

namespace Boards_WP.Data.Services;


public class CommentsService : ICommentsService
{
    private readonly ICommentsRepository commentsRepo;
    private readonly INotificationRepository notificationsRepo;
    const int MAX_DESCRIPTION_LENGTH = 618;
    const int MAX_INDENTATION_LEVEL = 7;

    public CommentsService(ICommentsRepository commentsRepo, INotificationRepository notificationsRepo)
    {
        this.commentsRepo = commentsRepo;
        this.notificationsRepo = notificationsRepo;
    }

    public void AddComment(Comment c)
    {
        ValidateComment(c);
        c.CreationTime = DateTime.Now;
        c.IsDeleted = false;
        commentsRepo.AddComment(c);

        if (c.ParentPost != null && c.Owner != null)
        {
            var notification = new Notification
            {
                RelatedPost = c.ParentPost,
                Receiver = c.ParentPost.Owner,
                Actor = c.Owner,
                ActionType = c.ParentComment == null ? NotificationType.CommentOnPost : NotificationType.ReplyToComment
            };
            if (notification.Receiver != null && notification.Receiver.UserID != notification.Actor.UserID)
            {
                notificationsRepo.AddNotification(notification);
            }
        }
    }

    public void SoftDeleteComment(Comment c, int userID)
    {
        c.Description = "[deleted]";
        c.IsDeleted= true;
        commentsRepo.SoftDeleteComment(c.CommentID);
    }
    public void IncreaseScore(Comment c, int currentUserID)
    {
        if (c.IsDeleted)
            throw new InvalidOperationException("Cannot vote on a deleted comment.");

        if (c.UserCurrentVote == VoteType.Like)
        {
            commentsRepo.DecreaseScore(c);
            commentsRepo.UpsertUserCommentVote(c.CommentID, currentUserID, VoteType.None);
            c.UserCurrentVote = VoteType.None;
        }
        else if (c.UserCurrentVote == VoteType.Dislike)
        {
            commentsRepo.IncreaseScore(c);
            commentsRepo.IncreaseScore(c);
            commentsRepo.UpsertUserCommentVote(c.CommentID, currentUserID, VoteType.Like);
            c.UserCurrentVote = VoteType.Like;
        }
        else
        {
            commentsRepo.IncreaseScore(c);
            commentsRepo.UpsertUserCommentVote(c.CommentID, currentUserID, VoteType.Like);
            c.UserCurrentVote = VoteType.Like;
        }
    }
    public void DecreaseScore(Comment c, int currentUserID)
    {
        if (c.IsDeleted)
            throw new InvalidOperationException("Cannot vote on a deleted comment.");

        if (c.UserCurrentVote == VoteType.Dislike)
        {
            commentsRepo.IncreaseScore(c);
            commentsRepo.UpsertUserCommentVote(c.CommentID, currentUserID, VoteType.None);
            c.UserCurrentVote = VoteType.None;
        }
        else if (c.UserCurrentVote == VoteType.Like)
        {
            commentsRepo.DecreaseScore(c);
            commentsRepo.DecreaseScore(c);
            commentsRepo.UpsertUserCommentVote(c.CommentID, currentUserID, VoteType.Dislike);
            c.UserCurrentVote = VoteType.Dislike;
        }
        else
        {
            commentsRepo.DecreaseScore(c);
            commentsRepo.UpsertUserCommentVote(c.CommentID, currentUserID, VoteType.Dislike);
            c.UserCurrentVote = VoteType.Dislike;
        }

    }
    public List<Comment> GetCommentsByPost(int postID, int currentUserID)
    {
        var comments = commentsRepo.GetCommentsByPostID(postID, currentUserID);

        // Dictionary doesn't allow null keys natively without throwing, so we use a special value (like -1)
        // or just avoid storing missing parents in the dictionary itself mapping.
        var childrenMap = new Dictionary<int, List<Comment>>();
        var rootComments = new List<Comment>();

        foreach (var c in comments)
        {
            int? parentId = c.ParentComment?.CommentID;

            if (parentId == null)
            {
                rootComments.Add(c);
            }
            else
            {
                if (!childrenMap.ContainsKey(parentId.Value))
                    childrenMap[parentId.Value] = new List<Comment>();

                childrenMap[parentId.Value].Add(c);
            }
        }

        var sortedComments = new List<Comment>();

        void AddSortedChildren(int parentId, List<Comment> currentLevelList)
        {
            var sorted = currentLevelList.OrderByDescending(c => CalculateBestScore(c)).ToList();
            foreach (var child in sorted)
            {
                sortedComments.Add(child);

                if (childrenMap.TryGetValue(child.CommentID, out var nestedChildren))
                {
                    AddSortedChildren(child.CommentID, nestedChildren);
                }
            }
        }

        AddSortedChildren(-1, rootComments); // Start with root comments

        return sortedComments;
    }

    private double CalculateBestScore(Comment comment)
    {

        double order = Math.Log10(Math.Max(Math.Abs(comment.Score), 1));

        int sign = 0;
        if (comment.Score > 0) sign = 1;
        else if (comment.Score < 0) sign = -1;


        double seconds = (comment.CreationTime - new DateTime(2020, 1, 1)).TotalSeconds;

        return (sign * order) + (seconds / 45000.0);
    }
    public static void ValidateComment(Comment c)
    {
        if (string.IsNullOrWhiteSpace(c.Description))
            throw new ArgumentException("Comment description cannot be empty.");
        if (c.Description.Length > MAX_DESCRIPTION_LENGTH)
            throw new ArgumentException("Comment description cannot exceed 618 characters.");
        if (c.Indentation> MAX_INDENTATION_LEVEL)
            throw new ArgumentException("Comment indentation cannot exceed 7 levels.");
    }
}

