using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

using Boards_WP.Data.Models;
using Boards_WP.Data.Repositories.Interfaces;

namespace Boards_WP.Data.Repositories;
public class FakeCommentsRepository : ICommentsRepository
{
    private readonly List<Comment> comments=new List<Comment>();
    private readonly List<CommentsVotes> commentsVotes=new List<CommentsVotes>();
    private int nextID = 1;

    public void addComment(Comment c)
    {
        c.commentID = nextID++;
        comments.Add(c);
    }

    public void softDeleteComment(int commentID)
    {
        Comment? comment = comments.Find(c => c.commentID == commentID);
        if (comment != null)
        {
            comment.isDeleted=true;
        }
    }
    public void increaseScore(Comment c)
    {
        c.score=c.score+1;
    }

    public void decreaseScore(Comment c) {
        c.score=c.score-1;
    }
    public List<Comment> getCommentsByPostID(int postID, int userID)
    {
        return comments
            .Where(c => c.postID == postID)
            .OrderByDescending(c => c.creationTime)
            .Select(c =>
            {
                VoteType currentVote;
                var voteRecord = commentsVotes.FirstOrDefault(v => v.commentID == c.commentID && v.userID == userID);

                if (voteRecord != null)
                {
                    currentVote = voteRecord.vote;
                }
                else
                {
                    currentVote = VoteType.None;
                }

                return new Comment
                {
                    commentID = c.commentID,
                    postID = c.postID,
                    parentID = c.parentID,
                    description = c.description,
                    score = c.score,
                    creationTime = c.creationTime,
                    indentation = c.indentation,
                    isDeleted = c.isDeleted,
                    ownerName = c.ownerName,
                    ownerPicture = c.ownerPicture,
                    userCurrentVote = currentVote
                };
            })
            .ToList();
    }
    public void upsertUserCommentVote(int commentID, int currentUserID, VoteType vote)
    {
        CommentsVotes? existingVote = commentsVotes.FirstOrDefault(v => v.commentID == commentID && v.userID == currentUserID);

        if (existingVote != null)
        {
            if (existingVote.vote == vote)
            {
                commentsVotes.Remove(existingVote);
            }
            else
            {
                existingVote.vote = vote;
            }
        }
        else
        {
            commentsVotes.Add(new CommentsVotes
            {
                commentID = commentID,
                userID = currentUserID,
                vote = vote
            });
        }
    }
}
