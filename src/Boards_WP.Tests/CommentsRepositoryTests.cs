using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Boards_WP.Data;
using Boards_WP.Data.Models;
using Boards_WP.Data.Repositories;

namespace Boards_WP.Tests
{
    public class CommentsRepositoryTests
    {
        [Fact]
        public void AddAndGetComment_ShouldWork()
        {
            // Arrange
            var repo = new CommentsRepository(DatabaseConfig.ConnectionString);

            // Assuming postID 1 and userID 1 exist from your manual script
            var newComment = new Comment
            {
                ParentPost = new Post { PostID = 1 },
                Owner = new User { UserID = 1 },
                Description = "This is a test comment from the unit test!",
                Score = 0,
                CreationTime = DateTime.UtcNow,
                Indentation = 0,
                IsDeleted = false
            };

            // Act: 1. Add the comment
            repo.AddComment(newComment);

            // Act: 2. Get comments for the post
            var comments = repo.GetCommentsByPostID(postID: 1, userID: 1);

            // Assert
            Assert.NotNull(comments);
            Assert.True(comments.Count > 0);

            var insertedComment = comments.First(c => c.Description == "This is a test comment from the unit test!");
            Assert.Equal("This is a test comment from the unit test!", insertedComment.Description);
            Assert.Equal(1, insertedComment.ParentPost.PostID);
            Assert.Equal(1, insertedComment.Owner.UserID);
        }

        [Fact]
        public void UpsertVote_ShouldChangeUserVote()
        {
            // Arrange
            var repo = new CommentsRepository(DatabaseConfig.ConnectionString);

            // Assuming post 1 and user 1
            var commentsBefore = repo.GetCommentsByPostID(postID: 1, userID: 1);
            if (commentsBefore.Count == 0)
            {
                AddAndGetComment_ShouldWork(); // Ensure at least one comment exists
                commentsBefore = repo.GetCommentsByPostID(postID: 1, userID: 1);
            }

            var targetComment = commentsBefore.First();

            // Act: Vote Like
            repo.UpsertUserCommentVote(targetComment.CommentID, currentUserID: 1, VoteType.Like);
            var commentsAfterLike = repo.GetCommentsByPostID(postID: 1, userID: 1);
            var likedComment = commentsAfterLike.First(c => c.CommentID == targetComment.CommentID);

            // Act: Change to Dislike
            repo.UpsertUserCommentVote(targetComment.CommentID, currentUserID: 1, VoteType.Dislike);
            var commentsAfterDislike = repo.GetCommentsByPostID(postID: 1, userID: 1);
            var dislikedComment = commentsAfterDislike.First(c => c.CommentID == targetComment.CommentID);

            // Assert
            Assert.Equal(VoteType.Like, likedComment.UserCurrentVote);
            Assert.Equal(VoteType.Dislike, dislikedComment.UserCurrentVote);
        }
    }
}
