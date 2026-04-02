//using System;
//using Xunit;
//using Boards_WP.Data;
//using Boards_WP.Data.Models;
//using Boards_WP.Data.Repositories;
//using Boards_WP.Data.Services;

//namespace Boards_WP.Tests
//{
//    public class CommentsServiceTests
//    {
//        private CommentsService CreateService()
//        {
//            var repo = new CommentsRepository(DatabaseConfig.ConnectionString);
//            var notifRepo = new NotificationRepository(DatabaseConfig.ConnectionString);
//            return new CommentsService(repo, notifRepo);
//        }

//        // AddComment

//        [Fact]
//        public void AddComment_WithEmptyDescription_ThrowsArgumentException()
//        {
//            var service = CreateService();
//            var badComment = new Comment { ParentPost = new Post { PostID = 1 }, Owner = new User { UserID = 1 }, Description = "" };

//            var exception = Assert.Throws<ArgumentException>(() => service.AddComment(badComment));
//            Assert.Equal("Comment description cannot be empty.", exception.Message);
//        }

//        [Fact]
//        public void AddComment_WithDescriptionOver618Chars_ThrowsArgumentException()
//        {
//            var service = CreateService();
//            var badComment = new Comment 
//            { 
//                ParentPost = new Post { PostID = 1 }, 
//                Owner = new User { UserID = 1 }, 
//                Description = new string('A', 619)
//            };

//            var exception = Assert.Throws<ArgumentException>(() => service.AddComment(badComment));
//            Assert.Contains("cannot exceed 618 characters", exception.Message);
//        }

//        [Fact]
//        public void AddComment_WithIndentationOver7_ThrowsArgumentException()
//        {
//            var service = CreateService();
//            var badComment = new Comment 
//            { 
//                ParentPost = new Post { PostID = 1 }, 
//                Owner = new User { UserID = 1 }, 
//                Description = "abdef",
//                Indentation = 8
//            };

//            var exception = Assert.Throws<ArgumentException>(() => service.AddComment(badComment));
//            Assert.Contains("cannot exceed 7 levels", exception.Message);
//        }

//        [Fact]
//        public void AddComment_AsReply_SetsParentAndIndentationCorrectly()
//        {
//            var service = CreateService();

//            var repo = new CommentsRepository(DatabaseConfig.ConnectionString);
//            var existingComments = repo.GetCommentsByPostID(1, 1);
//            var rootComment = existingComments[0];

//            var replyComment = new Comment
//            {
//                ParentPost = new Post { PostID = 1 },
//                Owner = new User { UserID = 1 },
//                Description = "This is a reply.",
//                ParentComment = rootComment,
//                Indentation = rootComment.Indentation + 1 
//            };

//            // Act
//            service.AddComment(replyComment);

//            // Assert
//            Assert.NotNull(replyComment.ParentComment);
//            Assert.Equal(rootComment.CommentID, replyComment.ParentComment.CommentID);
//            Assert.Equal(rootComment.Indentation + 1, replyComment.Indentation);
//        }

//        [Fact]
//        public void AddComment_GivenDifferentUsers_CreatesNotification()
//        {
//            var service = CreateService();
//            var repo = new CommentsRepository(DatabaseConfig.ConnectionString);
//            var notifRepo = new NotificationRepository(DatabaseConfig.ConnectionString);

//            // Create a fake second user to test the notification logic safely
//            using (var connection = new Microsoft.Data.SqlClient.SqlConnection(DatabaseConfig.ConnectionString))
//            {
//                // Ensure UserID 2 exists!
//                var insertUserCmd = new Microsoft.Data.SqlClient.SqlCommand(@"
//                    IF NOT EXISTS (SELECT * FROM Users WHERE userID = 2)
//                    BEGIN
//                        SET IDENTITY_INSERT Users ON;
//                        INSERT INTO Users (userID, username, email, passwordHash, status) 
//                        VALUES (2, 'UserTwo', 'user2@test.com', 'hash', 'Active');
//                        SET IDENTITY_INSERT Users OFF;
//                    END", connection);
//                connection.Open();
//                insertUserCmd.ExecuteNonQuery();
//            }

//            var notificationCountBefore = notifRepo.GetNotificationsByUserId(1).Count;

//            // Scenario: User 2 comments on User 1's post. Should generate a notification for User 1.
//            var newComment = new Comment
//            {
//                ParentPost = new Post { PostID = 1, Owner = new User { UserID = 1 } },
//                Owner = new User { UserID = 2 },
//                Description = "This is a notification test."
//            };

//            // Act
//            service.AddComment(newComment);

//            // Assert
//            var notificationsAfter = notifRepo.GetNotificationsByUserId(1);
//            Assert.True(notificationsAfter.Count > notificationCountBefore);

//            var lastNotification = notificationsAfter[notificationsAfter.Count - 1];
//            Assert.Equal(2, lastNotification.Actor.UserID);
//            Assert.Equal(1, lastNotification.Receiver.UserID);
//            Assert.Equal(NotificationType.CommentOnPost, lastNotification.ActionType);
//        }

//        // SoftDeleteComment 

//        [Fact]
//        public void SoftDeleteComment_ChangesDescriptionAndFlagsAsDeleted()
//        {
//            var service = CreateService();
//            var commentToTrash = new Comment { CommentID = 1, Description = "This is a test bla bla", IsDeleted = false };

//            service.SoftDeleteComment(commentToTrash, userID: 1);

//            Assert.True(commentToTrash.IsDeleted);
//            Assert.Equal("[deleted]", commentToTrash.Description);
//        }

//        // IncreaseScore

//        [Fact]
//        public void IncreaseComment_OnDeletedComment_ThrowsInvalidOperationException()
//        {
//            var service = CreateService();
//            var deletedComment = new Comment { CommentID = 1, IsDeleted = true };

//            var exception = Assert.Throws<InvalidOperationException>(() => service.IncreaseScore(deletedComment, 1));
//            Assert.Equal("Cannot vote on a deleted comment.", exception.Message);
//        }

//        [Fact]
//        public void IncreaseComment_FromNoneToLike_UpdatesStateCorrectly()
//        {
//            var service = CreateService();
//            var comment = new Comment { CommentID = 1, Score = 5, UserCurrentVote = VoteType.None, IsDeleted = false };

//            service.IncreaseScore(comment, currentUserID: 1);

//            Assert.Equal(VoteType.Like, comment.UserCurrentVote);
//            Assert.Equal(6, comment.Score);
//        }

//        [Fact]
//        public void IncreaseComment_FromLikeToNone_UpdatesStateCorrectly()
//        {
//            var service = CreateService();
//            var comment = new Comment { CommentID = 1, Score = 5, UserCurrentVote = VoteType.Like, IsDeleted = false };

//            service.IncreaseScore(comment, currentUserID: 1);

//            Assert.Equal(VoteType.None, comment.UserCurrentVote);
//            Assert.Equal(4, comment.Score);
//        }

//        [Fact]
//        public void IncreaseComment_FromDislikeToLike_UpdatesStateCorrectly()
//        {
//            var service = CreateService();
//            var comment = new Comment { CommentID = 1, Score = 5, UserCurrentVote = VoteType.Dislike, IsDeleted = false };

//            service.IncreaseScore(comment, currentUserID: 1);

//            Assert.Equal(VoteType.Like, comment.UserCurrentVote);
//            Assert.Equal(7, comment.Score); 
//        }

//        // DecreaseScore

//        [Fact]
//        public void DecreaseComment_OnDeletedComment_ThrowsInvalidOperationException()
//        {
//            var service = CreateService();
//            var deletedComment = new Comment { CommentID = 1, IsDeleted = true };

//            var exception = Assert.Throws<InvalidOperationException>(() => service.DecreaseScore(deletedComment, 1));
//            Assert.Equal("Cannot vote on a deleted comment.", exception.Message);
//        }

//        [Fact]
//        public void DecreaseComment_FromNoneToDislike_UpdatesStateCorrectly()
//        {
//            var service = CreateService();
//            var comment = new Comment { CommentID = 1, Score = 5, UserCurrentVote = VoteType.None, IsDeleted = false };

//            service.DecreaseScore(comment, currentUserID: 1);

//            Assert.Equal(VoteType.Dislike, comment.UserCurrentVote);
//            Assert.Equal(4, comment.Score);
//        }

//        [Fact]
//        public void DecreaseComment_FromLikeToDislike_UpdatesStateCorrectly()
//        {
//            var service = CreateService();
//            var comment = new Comment { CommentID = 1, Score = 5, UserCurrentVote = VoteType.Like, IsDeleted = false };

//            service.DecreaseScore(comment, currentUserID: 1);

//            Assert.Equal(VoteType.Dislike, comment.UserCurrentVote);
//            Assert.Equal(3, comment.Score);
//        }

//        [Fact]
//        public void DecreaseComment_FromDislikeToNone_UpdatesStateCorrectly()
//        {
//            var service = CreateService();
//            var comment = new Comment { CommentID = 1, Score = 5, UserCurrentVote = VoteType.Dislike, IsDeleted = false };

//            service.DecreaseScore(comment, currentUserID: 1);

//            Assert.Equal(VoteType.None, comment.UserCurrentVote);
//            Assert.Equal(6, comment.Score);
//        }
//    }
//}
