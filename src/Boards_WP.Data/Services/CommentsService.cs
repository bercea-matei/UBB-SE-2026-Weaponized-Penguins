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

        // Group comments by their parent ID to build a thread structure
        var childrenMap = new Dictionary<int?, List<Comment>>();
        foreach (var c in comments)
        {
            int? parentId = c.ParentComment?.CommentID;
            if (!childrenMap.ContainsKey(parentId))
                childrenMap[parentId] = new List<Comment>();

            childrenMap[parentId].Add(c);
        }

        var sortedComments = new List<Comment>();

        // Recursively sort and flatten threads starting from the root (null parent)
        void AddSortedChildren(int? parentId)
        {
            if (childrenMap.TryGetValue(parentId, out var children))
            {
                // Sort this level using our custom "Best" algorithm
                var sorted = children.OrderByDescending(c => CalculateBestScore(c)).ToList();
                foreach (var child in sorted)
                {
                    sortedComments.Add(child);
                    // Add children of this comment recursively
                    AddSortedChildren(child.CommentID);
                }
            }
        }

        AddSortedChildren(null);

        return sortedComments;
    }

    private double CalculateBestScore(Comment comment)
    {
        // Simple "Best/Hot" algorithm combining Score and Time.
        // Logarithmic scale for score: the first 10 votes count as much as the next 100.
        double order = Math.Log10(Math.Max(Math.Abs(comment.Score), 1));

        int sign = 0;
        if (comment.Score > 0) sign = 1;
        else if (comment.Score < 0) sign = -1;

        // Time factor: newer comments get a higher score. Measure seconds since an epoch.
        double seconds = (comment.CreationTime - new DateTime(2020, 1, 1)).TotalSeconds;

        // 45000 seconds is about 12.5. A newer comment easily outranks older ones unless they have vastly higher scores.
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

